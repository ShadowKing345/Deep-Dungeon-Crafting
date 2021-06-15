using System;
using UnityEngine;

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

        public IntMinMax floorWidth;
        public IntMinMax floorHeight;
        
        public IntMinMax numberOfRooms;
        public IntMinMax roomWidth;
        public IntMinMax roomHeight;
        

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

    [Serializable]
    public class IntMinMax
    {
        public int min;
        public int max;
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