using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Room
{
    public class Room : MonoBehaviour
    {
        public Vector2 id;
        public int width;
        public int height;
        public FloorScheme floorScheme;
        private readonly List<Vector3> positions = new List<Vector3>();
        private readonly List<GameObject> tileList = new List<GameObject>();
        private readonly DirectionObj<DoorTile> doorTiles = new DirectionObj<DoorTile>();

        public DirectionObj<Room> nearbyRooms = new DirectionObj<Room>();

        public void Init(Vector2 id, int width, int height, FloorScheme floorScheme)
        {
            this.id = id;
            this.width = width;
            this.height = height;
            this.floorScheme = floorScheme;
        }
        
        public void ResetLists()
        {
            positions.Clear();
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    positions.Add(new Vector3(i, j, 0));
                }
            }

            foreach (GameObject obj in tileList) Destroy(obj);
            tileList.Clear();
        }

        public void FillTiles()
        {
            foreach (Vector3 pos in positions)
            {
                Direction direction = GetPrefab(pos, out GameObject prefab);
                GameObject tile = Instantiate(prefab, pos + transform.position, quaternion.identity);
                tile.transform.SetParent(transform);
                
                tileList.Add(tile);
                
                if(tile.TryGetComponent(out IDirectional directional)) directional.SetDirection(direction);
                if(tile.TryGetComponent(out IRoomTile roomTile)) roomTile.UpdateLook();
                if (tile.TryGetComponent(out DoorTile doorTile)) doorTiles.SetDirection(direction, doorTile);
            }
        }
        
        public Direction GetPrefab(Vector3 pos, out GameObject prefab)
        {
            if (0 < pos.x && pos.x < width - 1 && 0 < pos.y && pos.y < height - 1)
            {
                prefab = floorScheme.GetTile(TileType.Floor);
                return Direction.S;
            }

            TileType type = TileType.Wall;

            if (pos.x + pos.y == 0 || (int) (pos.x + pos.y) + 2 == height + width ||
                pos.x == 0 && (int) pos.y == height - 1 || (int) pos.x == width - 1 && pos.y == 0)
                type = TileType.Corner;

            Direction direction = Direction.S;

            if (type == TileType.Corner)
            {
                bool isEast = pos.x > 0;
                bool isNorth = pos.y > 0;

                if (isNorth) direction = isEast ? Direction.NE : Direction.NW;
                else direction = isEast ? Direction.SE : Direction.SW;
            }
            else
            {
                if (pos.x == 0) direction = Direction.W;
                if ((int) pos.x == width - 1) direction = Direction.E;
                if ((int) pos.y == height - 1) direction = Direction.N;

                if (nearbyRooms.GetDirection((int) direction) != null)
                {
                    switch (direction)
                    {
                        case Direction.S:
                        case Direction.N:
                            if ((int) pos.x == (int) Mathf.Floor(height / 2f))
                                type = TileType.Door;
                            break;
                        case Direction.W:
                        case Direction.E:
                            if ((int) pos.y == (int) Mathf.Floor(width / 2f))
                                type = TileType.Door;
                            break;
                    }
                }
            }
            
            prefab = floorScheme.GetTile(type);

            return direction;
        }

        public void SetupDoors()
        {
            foreach (Direction direction in new []{Direction.S, Direction.W, Direction.N, Direction.E} )
            {
                Room nearbyRoom = nearbyRooms.GetDirection(direction);
                doorTiles.GetDirection(direction)?.SetUpDoor(nearbyRoom, nearbyRoom.GetDoorAtOppositeDirection(direction));
            }
        }

        private DoorTile GetDoorAtOppositeDirection(Direction direction) =>
            direction switch
            {
                Direction.S => doorTiles.GetDirection(Direction.N),
                Direction.W => doorTiles.GetDirection(Direction.E),
                Direction.N => doorTiles.GetDirection(Direction.S),
                Direction.E => doorTiles.GetDirection(Direction.W),
                _ => null
            };
    }
}