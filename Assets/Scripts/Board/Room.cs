using System.Collections.Generic;
using Board.FloorTiles;
using Interfaces;
using Unity.Mathematics;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Board
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Vector2 id;
        [SerializeField] private Vector2 size;
        [SerializeField] private FloorScheme floorScheme;
        
        private readonly List<Vector3> positions = new List<Vector3>();
        private readonly List<GameObject> tileList = new List<GameObject>();
        private readonly DirectionObj<DoorTile> doorTiles = new DirectionObj<DoorTile>();
        private readonly List<GameObject> enemyList = new List<GameObject>();
        [SerializeField] private GameObject exitTile;
        [SerializeField] private GameObject entranceTile;

        public DirectionObj<Room> nearbyRooms = new DirectionObj<Room>();

        public void Init(Vector2 id, Vector2 size, FloorScheme floorScheme)
        {
            this.id = id;
            this.size = size;
            this.floorScheme = floorScheme;
        }
        
        public void ResetLists()
        {
            positions.Clear();
            
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    positions.Add(new Vector3(i, j, 0));
                }
            }

            foreach (GameObject obj in tileList) Destroy(obj);
            tileList.Clear();

            foreach (GameObject obj in enemyList) Destroy(obj);
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
                
                if(tile.TryGetComponent(out IDirectional directional)) directional.Direction = direction;
                if(tile.TryGetComponent(out IRoomTile roomTile)) roomTile.UpdateLook();
                if (tile.TryGetComponent(out DoorTile doorTile)) doorTiles.SetDirection(direction, doorTile);
            }
        }

        private Direction GetPrefab(Vector3 pos, out GameObject prefab)
        {
            if (0 < pos.x && pos.x < size.x - 1 && 0 < pos.y && pos.y < size.y - 1)
            {
                prefab = floorScheme.GetTile(TileType.Floor);
                return Direction.S;
            }

            TileType type = TileType.Wall;

            if (pos.x + pos.y == 0 || (int) (pos.x + pos.y) + 2 == size.x + size.y ||
                pos.x == 0 && (int) pos.y == size.y - 1 || (int) pos.x == size.x - 1 && pos.y == 0)
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
                if ((int) pos.x == size.x - 1) direction = Direction.E;
                if ((int) pos.y == size.y - 1) direction = Direction.N;

                if (nearbyRooms.GetDirection(direction) != null)
                {
                    switch (direction)
                    {
                        case Direction.S:
                        case Direction.N:
                            if ((int) pos.x == (int) Mathf.Floor(size.y / 2f))
                                type = TileType.Door;
                            break;
                        case Direction.W:
                        case Direction.E:
                            if ((int) pos.y == (int) Mathf.Floor(size.x / 2f))
                                type = TileType.Door;
                            break;
                    }
                }
            }
            
            prefab = floorScheme.GetTile(type);

            return direction;
        }

        public void SetupExit()
        {
            Vector3 roomPos = transform.position;
            Vector2 pos = new Vector2(roomPos.x + size.x / 2f, roomPos.y + size.y / 2f);
            
            exitTile = Instantiate(floorScheme.exitTile, new Vector3(pos.x, pos.y, roomPos.z - 1), Quaternion.identity);
            exitTile.transform.SetParent(transform);
            if(exitTile.TryGetComponent(out IRoomTile tile)) tile.UpdateLook();
        }

        public void SetupEntrance()
        {
            Vector3 roomPos = transform.position;
            Vector2 pos = new Vector2(roomPos.x + size.x / 2f, roomPos.y + size.y / 2f);
            
            entranceTile = Instantiate(floorScheme.entranceTile, new Vector3(pos.x, pos.y, roomPos.z - 1), Quaternion.identity);
            entranceTile.transform.SetParent(transform);
            if(entranceTile.TryGetComponent(out IRoomTile tile)) tile.UpdateLook();
        }

        public void SpawnEnemy()
        {
            if (Random.value > floorScheme.enemyChance) return;

            int enemyCount = Random.Range(floorScheme.enemyCount.Min, floorScheme.enemyCount.Max);
            GameObject enemyPreFab = floorScheme.enemies[Random.Range(0, floorScheme.enemies.Length)];

            for (int i = 0; i < enemyCount; i++)
            {
                Vector2 pos = new Vector2(Random.Range(1, size.x - 1), Random.Range(1, size.y - 1));
                GameObject enemy = Instantiate(enemyPreFab, (Vector3) pos + transform.position, Quaternion.identity);
                enemy.transform.SetParent(transform);
                enemyList.Add(enemy); 
            }
        }

        public void SetupDoors()
        {
            foreach (Direction direction in new []{Direction.S, Direction.W, Direction.N, Direction.E} )
            {
                Room nearbyRoom = nearbyRooms.GetDirection(direction);
                doorTiles.GetDirection(direction)?.SetUpDoor(nearbyRoom, nearbyRoom.GetDoorAtOppositeDirection(direction));
            }
        }

        public void PlacePlayer()
        {
            if (entranceTile != null && entranceTile.TryGetComponent(out EntranceTile tile)) tile.PlacePlayer();
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