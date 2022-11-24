using System.Collections.Generic;
using System.Linq;
using Board;
using UnityEngine;
using Utils;

namespace Managers
{
    public class BoardManager : MonoBehaviour
    {
        private const int EmergencyLoopExit = 100;
        private static BoardManager _instance;

        [SerializeField] private FloorSettings floorSettings;
        [SerializeField] private Transform board;

        [Space] [Header("Internal Variables")] [SerializeField]
        private Vector2Int gridSize;

        [SerializeField] private Vector2Int actualGridSize;
        [SerializeField] private int roomNumber;
        private readonly List<Vector2Int> roomPositions = new();

        private readonly Dictionary<Vector2Int, GameObject> rooms = new();

        private GameManager gameManager;

        public static BoardManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<BoardManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value) Destroy(value);
                _instance = value;
            }
        }

        public FloorSettings FloorSettings
        {
            get => floorSettings;
            set => floorSettings = value;
        }

        public int CurrentFloor { get; set; }

        private void Awake()
        {
            Instance = this;
            gameManager = GameManager.Instance;
        }

        public void ResetLists()
        {
            rooms.Clear();
            roomPositions.Clear();
            GameObjectUtils.ClearChildren(board);
        }

        public void InitializeVariables()
        {
            gridSize = new Vector2Int(Random.Range(floorSettings.GridWidth.Min, floorSettings.GridWidth.Max),
                           Random.Range(floorSettings.GridHeight.Min, floorSettings.GridHeight.Max)) +
                       Vector2Int.one * CurrentFloor / 3;
            roomNumber = Random.Range(floorSettings.RoomCount.Min, floorSettings.RoomCount.Max + 1) +
                         gameManager.CurrentFloor / 3;
        }

        public void GenerateLayout()
        {
            var roomCount = roomNumber;
            var loopExit = EmergencyLoopExit + roomCount;

            var currentPos = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));

            roomPositions.Add(currentPos);
            roomCount--;

            var directionHold = new List<Direction>(DirectionUtils.Cardinals);

            while (roomCount > 0 && loopExit > 0)
            {
                var direction = directionHold[Random.Range(0, directionHold.Count)];

                if (TryGetIndexFromDirection(currentPos, gridSize, direction, out currentPos) &&
                    !roomPositions.Contains(currentPos))
                {
                    roomPositions.Add(currentPos);
                    directionHold = new List<Direction>(DirectionUtils.Cardinals);
                    roomCount--;
                }

                directionHold.RemoveAt(directionHold.IndexOf(direction));

                if (directionHold.Count <= 0)
                {
                    directionHold = new List<Direction>(DirectionUtils.Cardinals);
                    currentPos = roomPositions[Random.Range(0, roomPositions.Count)];
                }

                loopExit--;
            }

            var max = new Vector2Int();
            max = roomPositions.Aggregate(max, Vector2Int.Max);
            var min = max;
            min = roomPositions.Aggregate(min, Vector2Int.Min);

            for (var i = 0; i < roomPositions.Count; i++) roomPositions[i] = roomPositions[i] - min;
            actualGridSize = max - min + Vector2Int.one;
        }

        public void CreateRooms()
        {
            var spacing = 3;
            rooms.Add(roomPositions[0], Instantiate(floorSettings.Entrance, board));
            rooms.Add(roomPositions[roomPositions.Count - 1], Instantiate(floorSettings.Exit, board));

            for (var i = 1; i < roomPositions.Count - 1; i++)
                rooms.Add(roomPositions[i],
                    Instantiate(floorSettings.Rooms[Random.Range(0, floorSettings.Rooms.Length)], board));

            for (var i = 0; i < gridSize.x; i++)
            for (var j = 0; j < gridSize.y; j++)
            {
                var pos = new Vector2Int(i, j);

                if (!rooms.TryGetValue(pos, out var obj))
                    continue;

                obj.transform.position = (Vector2) pos * (floorSettings.RoomSize + Vector2Int.one * spacing);
            }
        }

        public void Scan()
        {
        }

        public void ConnectRooms()
        {
            var d = new Dictionary<Vector2Int, DirectionalObj<Room>>();

            foreach (var kvPair in rooms)
            {
                var roomRoom = new DirectionalObj<Room>();
                foreach (var direction in DirectionUtils.Cardinals)
                    if (rooms.TryGetValue(kvPair.Key + Vector2Int.CeilToInt(direction.AsVector()),
                            out var obj))
                        if (obj.TryGetComponent(out Room room))
                            roomRoom.SetDirection(direction, room);

                d.Add(kvPair.Key, roomRoom);
            }

            foreach (var kvPair in rooms)
                if (kvPair.Value.TryGetComponent(out Room room))
                    room.ConnectedRooms = d[kvPair.Key];
        }

        public void PlacePlayer()
        {
            if (rooms.First().Value.TryGetComponent(out Room room)) room.EntranceTile.PlacePlayer();
        }

        public void FillInRoof()
        {
            var roofContainer = new GameObject("Roof Container");
            roofContainer.transform.SetParent(board);
            roofContainer.transform.position = new Vector3(0, 0, 10);
            var spacing = 3;

            var tileCount = actualGridSize * floorSettings.RoomSize + (actualGridSize - Vector2Int.one) * spacing;

            for (var i = -spacing - 2; i < tileCount.x + spacing + 2; i++)
            for (var j = -spacing - 2; j < tileCount.y + spacing + 2; j++)
                Instantiate(floorSettings.Roof, roofContainer.transform, false).transform.position =
                    roofContainer.transform.position + new Vector3(i, j);
        }

        private static bool TryGetIndexFromDirection(Vector2Int currentPos, Vector2Int gridSize, Direction direction,
            out Vector2Int newPos)
        {
            var directionVector2 = direction.AsVector();
            newPos = currentPos + Vector2Int.CeilToInt(directionVector2);

            return 0 <= newPos.x && newPos.x < gridSize.x && 0 <= newPos.y && newPos.y < gridSize.y;
        }
    }
}