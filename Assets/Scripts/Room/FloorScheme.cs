using System;
using UnityEngine;
using Utils;

namespace Room
{
    [CreateAssetMenu(menuName = "SO/Floor", fileName = "New Floor")]
    public class FloorScheme : ScriptableObject
    {
        public GameObject floorTiles;

        public GameObject cornerTile;
        public GameObject wallTile;

        public GameObject doorTile;
        public GameObject exitTile;
        public GameObject starterTile;
        
        public GameObject[] objectTiles;

        public MinMax<int> floorWidth;
        public MinMax<int> floorHeight;
        public MinMax<int> numberOfRooms;
        
        public MinMax<int> roomWidth;
        public MinMax<int> roomHeight;
        

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