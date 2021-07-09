using System;
using UnityEngine;
using Utils;

namespace Room
{
    [CreateAssetMenu(menuName = "SO/Floor", fileName = "New Floor")]
    public class FloorScheme : ScriptableObject
    {
        [Header("Basic")]
        public GameObject floorTiles;
        [Space]
        public GameObject cornerTile;
        public GameObject wallTile;
        [Space]
        [Header("Interactable")]
        public GameObject doorTile;
        public GameObject exitTile;
        public GameObject starterTile;
        [Space]
        [Header("Enemies")]
        public GameObject[] enemies;
        [Space]
        [Header("Floor Generation Settings")]
        public MinMax<int> floorWidth;
        public MinMax<int> floorHeight;
        public MinMax<int> numberOfRooms;
        [Space]
        [Header("Room Generation Settings")]
        public MinMax<int> roomWidth;
        public MinMax<int> roomHeight;
        public MinMax<int> enemyCount;
        public float enemyChance;

        public GameObject GetTile(TileType type) =>
            type switch
            {
                TileType.Floor => floorTiles,
                TileType.Wall => wallTile,
                TileType.Corner => cornerTile,
                TileType.Door => doorTile,
                TileType.Entrance => starterTile,
                TileType.Exit => exitTile,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }

    public enum TileType
    {
        Floor,
        Wall,
        Corner,
        Door,
        Exit,
        Entrance
    }
}