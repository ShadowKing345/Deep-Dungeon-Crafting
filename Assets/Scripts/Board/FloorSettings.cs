using UnityEngine;
using Utils;

namespace Board
{
    [CreateAssetMenu(menuName = "SO/Floor", fileName = "New Floor")]
    public class FloorSettings : ScriptableObject
    {
        [SerializeField] private int startingFloorNumber;
        [Header("Special Rooms")] [SerializeField]
        private GameObject entrance;

        [SerializeField] private GameObject exit;
        [SerializeField] private GameObject roof;
        [Header("Rooms")] [SerializeField] private GameObject[] rooms;

        [Space] [Header("Settings")] [SerializeField]
        private MinMax<int> gridWidth;

        [SerializeField] private MinMax<int> gridHeight;
        [Space] [SerializeField] private Vector2Int roomSize;
        [SerializeField] private MinMax<int> roomCount;
        [SerializeField] private int spacing = 3;
        // [Space] [SerializeField] private GameObject[] enemies;
        // [SerializeField] private MinMax<int> enemyCount;
        // [Range(0, 1f)] [SerializeField] private float enemyChance;

        public int StartingFloorNumber => startingFloorNumber;
        public GameObject Entrance => entrance;
        public GameObject Exit => exit;
        public GameObject Roof => roof;
        public GameObject[] Rooms => rooms;
        public MinMax<int> GridWidth => gridWidth;
        public MinMax<int> GridHeight => gridHeight;
        public Vector2Int RoomSize => roomSize;
        public MinMax<int> RoomCount => roomCount;
        public int Spacing => spacing;
        // public GameObject[] Enemies => enemies;
        // public MinMax<int> EnemyCount => enemyCount;
        // public float EnemyChance => enemyChance;
    }
}