using System;
using System.Collections.Generic;
using System.Linq;
using Room;
using UnityEditor;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Managers
{
    public class BoardManager : MonoBehaviour
    {
        public static BoardManager instance;

        [SerializeField] private FloorScheme floorScheme;
        [SerializeField] private List<Vector2> floorList = new List<Vector2>();
        [SerializeField] private List<Vector2> roomPositions = new List<Vector2>() ;
        private readonly Dictionary<Vector2, Room.Room> roomLookUp = new Dictionary<Vector2, Room.Room>();

        [SerializeField] private int gridWidth;
        [SerializeField] private int gridHeight;

        [SerializeField] private Transform boardParent;

        [SerializeField] private int numberOfRooms;
        [SerializeField] private int roomWidth;
        [SerializeField] private int roomHeight;

        private readonly Direction[] _directions = {Direction.S, Direction.W, Direction.E, Direction.N};
        private const int EmergencyLoopExit = 32;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        public void ResetLists()
        {
            floorList.Clear();
            roomPositions.Clear();
            foreach (KeyValuePair<Vector2, Room.Room> keyValuePair in roomLookUp) Destroy(keyValuePair.Value.gameObject);
            roomLookUp.Clear();
        }

        public void InitVariables()
        {
            gridWidth = Random.Range(floorScheme.floorWidth.Min, floorScheme.floorWidth.Max + 1);
            gridHeight = Random.Range(floorScheme.floorHeight.Min, floorScheme.floorHeight.Max + 1);
            
            roomWidth = Random.Range(floorScheme.roomWidth.Min, floorScheme.roomWidth.Max + 1);
            roomHeight = Random.Range(floorScheme.roomHeight.Min, floorScheme.roomHeight.Max + 1);

            numberOfRooms = Random.Range(floorScheme.numberOfRooms.Min, floorScheme.numberOfRooms.Max);
        }

        public void CreateRoomLayout()
        {
            for (int i = 0; i < gridWidth; i++)
                for (int j = 0; j < gridHeight; j++)
                    floorList.Add(new Vector2(i, j));

            if (floorList.Count <= 0) return;
            
            int roomCount = numberOfRooms;
            int loopExit = EmergencyLoopExit + roomCount;
            List<int> roomIndex = new List<int>();
            int currentPos = Random.Range(0, floorList.Count);

            roomIndex.Add(currentPos);
            roomCount--;

            List<Direction> directionHold = new List<Direction>(_directions);
            
            while (roomCount > 0 && loopExit > 0 )
            {
                Direction direction = directionHold[Random.Range(0, directionHold.Count)];

                if (TryGetIndexFromDirection(currentPos, gridWidth, gridHeight, direction, out currentPos))
                {
                    if (roomIndex.Contains(currentPos)) continue;
                    
                    roomIndex.Add(currentPos);
                    directionHold = new List<Direction>(_directions);
                    roomCount--;
                }
                
                directionHold.RemoveAt(directionHold.IndexOf(direction));

                if (directionHold.Count <= 0)
                {
                    directionHold = new List<Direction>(_directions);
                    currentPos = roomIndex[Random.Range(0, roomIndex.Count)];
                }

                loopExit--;
            }

            foreach (int index in roomIndex)
            {
                roomPositions.Add(floorList[index]);
            }
        }

        public void CreateRoomObj()
        {
            if (boardParent == null) return;
            
            foreach (Vector2 roomPos in roomPositions)
            {
                GameObject roomObj = new GameObject($"Room{roomPos.ToString()}");
                
                roomObj.transform.SetParent(boardParent);
                roomObj.transform.SetPositionAndRotation(new Vector3(roomPos.x * roomWidth, roomPos.y * roomHeight, 1), Quaternion.identity);
                
                Room.Room room = roomObj.AddComponent<Room.Room>();
                roomLookUp.Add(roomPos, room);
            }
        }

        public void LinkRoomsTogether()
        {
            foreach (KeyValuePair<Vector2, Room.Room> kvPair in roomLookUp)
            {
                Vector2 pos = kvPair.Key;
                int index = floorList.IndexOf(pos);
                DirectionObj<Room.Room> directionObj = new DirectionObj<Room.Room>();

                foreach (Direction direction in _directions)
                {
                    Room.Room room = null;

                    if (TryGetIndexFromDirection(index, gridWidth, gridHeight, direction, out int adjacentRoomIndex))
                        roomLookUp.TryGetValue(floorList[adjacentRoomIndex], out room);

                    directionObj.SetDirection( direction, room);
                }

                kvPair.Value.nearbyRooms = directionObj;
            }
        }

        public void GenerateRoomsTiles()
        {
            foreach (KeyValuePair<Vector2, Room.Room> kvPair in roomLookUp)
            {
                Room.Room room = kvPair.Value;

                room.Init(kvPair.Key, roomWidth, roomHeight, floorScheme);
                room.ResetLists();
                room.FillTiles();
                room.SpawnEnemy();
            }

            foreach (Room.Room room in roomLookUp.Values)
            {
                room.SetupDoors();
            }
        }

        private static bool TryGetIndexFromDirection(int i, int width, int height, Direction direction, out int result)
        {
            switch (direction)
            {
                case Direction.S when i % width - 1 >= 0:
                    result = i - 1;
                    return true;
                case Direction.W when i - width >= 0:
                    result = i - width;
                    return true;
                case Direction.N when i % width + 1 < width:
                    result = i + 1;
                    return true;
                case Direction.E when i + width < width * height:
                    result = i + width;
                    return true;
                default:
                    result = i;
                    return false;
            }
        }

        public Vector2 GetPlayerSpawnPoint()
        {
            if (roomLookUp.Count <= 0) return Vector2.zero;

            return roomLookUp.First().Value.transform.position + new Vector3(roomWidth / 2f, roomHeight / 2f);
        }

    }
}