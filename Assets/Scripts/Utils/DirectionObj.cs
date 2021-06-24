using System;

namespace Utils
{
    [Serializable]
    public class DirectionObj<T>
    {
        public T south;
        public T west;
        public T north;
        public T east;
        public T GetDirection(Direction direction) =>
            direction switch
            {
                Direction.S => south,
                Direction.W => west,
                Direction.N => north,
                Direction.E => east,
                _ => south
            };
        
        public void SetDirection(Direction direction, T obj)
        {
            switch (direction)
            {
                case Direction.S:
                    south = obj;
                    break;
                case Direction.W:
                    west = obj;
                    break;
                case Direction.N:
                    north = obj;
                    break;
                case Direction.E:
                    east = obj;
                    break;
            }
        }

        public override string ToString() => $"DirectionalObj:\nSouth: {south}\nWest: {west}\nNorth: {north}\nEast: {east}\n";
    }
}