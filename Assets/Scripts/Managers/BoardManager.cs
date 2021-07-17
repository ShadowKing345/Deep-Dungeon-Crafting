using System;
using System.Collections.Generic;
using System.Linq;
using Board;
using Pathfinding;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Managers
{
    public class BoardManager : MonoBehaviour
    {
        private static BoardManager _instance;
        public static BoardManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<BoardManager>();
                return _instance;
            }
            private set
            {
                if(_instance != null && _instance != value) Destroy(value);
                _instance = value;
            }
        }

        private GameManager _gameManager;
        private AstarPath _astarPath;

        public int FloorNumber { get; set; }

        [SerializeField] private FloorScheme floorScheme;
        [SerializeField] private List<Vector2> floorList = new List<Vector2>();
        [SerializeField] private List<Vector2> roomPositions = new List<Vector2>() ;

        [Space]
        [SerializeField] private Vector2 gridSize;

        [Space]
        [SerializeField] private int numberOfRooms;
        [SerializeField] private Vector2 roomSize;

        [Space] [SerializeField] private Transform board;

        private readonly Dictionary<Vector2, Room> roomLookUp = new Dictionary<Vector2, Room>();
        private readonly Direction[] _directions = {Direction.S, Direction.W, Direction.E, Direction.N};
        private const int EmergencyLoopExit = 32;

        private void OnEnable()
        {
            Instance = this;
            _gameManager = GameManager.Instance;
            floorScheme ??= _gameManager.FloorScheme;
            
            _astarPath = AstarPath.active != null ? AstarPath.active : GetComponent<AstarPath>();
        }
        
        public void ResetBoard()
        {
            floorList.Clear();
            roomPositions.Clear();
            roomLookUp.Clear();
            foreach (Transform child in board) Destroy(child.gameObject);
        }

        public void InitVariables()
        {
            gridSize = new Vector2(Random.Range(floorScheme.floorWidth.Min, floorScheme.floorWidth.Max + 1), Random.Range(floorScheme.floorHeight.Min, floorScheme.floorHeight.Max + 1)) + Vector2.one * FloorNumber;
            roomSize = new Vector2(Random.Range(floorScheme.roomWidth.Min, floorScheme.roomWidth.Max + 1), Random.Range(floorScheme.roomHeight.Min, floorScheme.roomHeight.Max + 1)) + Vector2.one * Mathf.FloorToInt(FloorNumber / 3f);

            numberOfRooms = Random.Range(floorScheme.numberOfRooms.Min, floorScheme.numberOfRooms.Max) + FloorNumber;
        }

        public void CreateRoomLayout()
        {
            for (int i = 0; i < gridSize.x; i++)
                for (int j = 0; j < gridSize.y; j++)
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

                if (TryGetIndexFromDirection(currentPos, (int) gridSize.x, (int) gridSize.y, direction, out currentPos))
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
            if (board == null) return;
            
            foreach (Vector2 roomPos in roomPositions)
            {
                GameObject roomObj = new GameObject($"Room{roomPos.ToString()}");
                
                roomObj.transform.SetParent(board);
                roomObj.transform.SetPositionAndRotation(new Vector3(roomPos.x * roomSize.x, roomPos.y * roomSize.y, board.position.z), Quaternion.identity);
                
                Board.Room room = roomObj.AddComponent<Board.Room>();
                roomLookUp.Add(roomPos, room);
            }
        }

        public void LinkRoomsTogether()
        {
            foreach (KeyValuePair<Vector2, Board.Room> kvPair in roomLookUp)
            {
                Vector2 pos = kvPair.Key;
                int index = floorList.IndexOf(pos);
                DirectionObj<Board.Room> directionObj = new DirectionObj<Board.Room>();

                foreach (Direction direction in _directions)
                {
                    Board.Room room = null;

                    if (TryGetIndexFromDirection(index, (int) gridSize.x, (int) gridSize.y, direction, out int adjacentRoomIndex))
                        roomLookUp.TryGetValue(floorList[adjacentRoomIndex], out room);

                    directionObj.SetDirection( direction, room);
                }

                kvPair.Value.nearbyRooms = directionObj;
            }
        }

        public void GenerateRoomsTiles()
        {
            foreach (KeyValuePair<Vector2, Board.Room> kvPair in roomLookUp)
            {
                Board.Room room = kvPair.Value;

                room.Init(kvPair.Key, roomSize, floorScheme);
                room.ResetLists();
                room.FillTiles();
            }
            
            foreach (Board.Room room in roomLookUp.Values)
            {
                room.SetupDoors();
            }
        }

        public void SetupPathFinder()
        {
            GridGraph gg = _astarPath.data.gridGraph;
            
            Vector2 size = roomSize * gridSize;

            gg.SetDimensions((int) size.x, (int) size.y, gg.nodeSize);
            gg.center = size / 2f;
               
            _astarPath.Scan();
        }

        public void GenerateRoomObject()
        {
            Board.Room[] rooms = roomLookUp.Values.ToArray();
            for(int i = 1; i < rooms.Length -1; i++)
            {
                rooms[i].SpawnEnemy();
            }
            
            rooms[0].SetupEntrance();
            rooms[rooms.Length - 1].SetupExit();
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

        public void PlacePlayer() => roomLookUp.FirstOrDefault().Value.PlacePlayer();
    }
}