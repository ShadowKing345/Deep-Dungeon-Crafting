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

        public T GetDirection(int direction) =>
            direction switch
            {
                1 => south,
                2 => west,
                3 => north,  
                4 => east,
                _ => south
            };

        public T GetDirection(Direction direction) =>
            direction switch
            {
                Direction.S => south,
                Direction.W => west,
                Direction.N => north,
                Direction.E => east,
                _ => south
            };

        public void SetDirection(int direction, T obj)
        {
            switch (direction)
            {
                case 1:
                    south = obj;
                    break;
                case 2:
                    west = obj;
                    break;
                case 3:
                    north = obj;
                    break;
                case 4:
                    east = obj;
                    break;
            }
        }

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
                default:
                    break;
            }
        }

        public override string ToString() => $"DirectionalObj:\nSouth: {south}\nWest: {west}\nNorth: {north}\nEast: {east}\n";
    }
}