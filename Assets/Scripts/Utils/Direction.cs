using System;
using UnityEngine;

namespace Utils
{
    // ReSharper disable InconsistentNaming
    public enum Direction
    {
        S = 0,
        E = 1,
        N = 2,
        W = 3,
        
        SW = 4,
        SE = 5,
        NW = 6,
        NE = 7
    }
    
    public static class DirectionUtils
    {
        public static Direction GetDirectionFromInterCardinal(this Direction direction) =>
            direction switch
            {
                Direction.SW => Direction.S,
                Direction.SE => Direction.W,
                Direction.NW => Direction.N,
                Direction.NE => Direction.E,
                _ => direction
            };

        public static Vector2 GetVectorDirection(this Direction direction) =>
            direction switch {
                Direction.S => Vector2.down,
                Direction.E => Vector2.right,
                Direction.N => Vector2.up,
                Direction.W => Vector2.left,
                
                Direction.SW => new Vector2(-1, -1),
                Direction.SE => new Vector2(1, -1),
                Direction.NW => new Vector2(-1, 1),
                Direction.NE => new Vector2(1, 1),
                _ => Vector2.zero
            };

        public static Direction GetOpposite(this Direction direction) => direction switch
        {
            Direction.S => Direction.N,
            Direction.E => Direction.W,
            Direction.N => Direction.S,
            Direction.W => Direction.E,
            Direction.SW => Direction.NE,
            Direction.SE => Direction.NW,
            Direction.NW => Direction.SE,
            Direction.NE => Direction.SW,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
        
        public static Direction[] Cardinals =>
            new[] {Direction.S, Direction.E, Direction.N, Direction.W};

        public static Direction[] InterCardinals =>
            new[] {Direction.SW, Direction.SE, Direction.NW, Direction.NE};
    }
}