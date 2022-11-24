using System;

namespace Utils
{
    [Serializable]
    public class DirectionalObj<T>
    {
        public T south;
        public T west;
        public T north;
        public T east;

        public T GetDirection(Direction direction)
        {
            return direction switch
            {
                Direction.S => south,
                Direction.W => west,
                Direction.N => north,
                Direction.E => east,
                _ => south
            };
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
            }
        }

        public override string ToString()
        {
            return $"DirectionalObj:\nSouth: {south}\nWest: {west}\nNorth: {north}\nEast: {east}\n";
        }
    }

    [Serializable]
    public class OctaDirectionalObj<T>
    {
        public T s;
        public T w;
        public T n;
        public T e;

        public T sw;
        public T se;
        public T nw;
        public T ne;

        public T GetDirection(Direction direction)
        {
            return direction switch
            {
                Direction.S => s,
                Direction.E => e,
                Direction.N => n,
                Direction.W => w,
                Direction.SW => sw,
                Direction.SE => se,
                Direction.NW => nw,
                Direction.NE => ne,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }


        public void SetDirection(Direction direction, T obj)
        {
            switch (direction)
            {
                case Direction.S:
                    s = obj;
                    break;
                case Direction.E:
                    e = obj;
                    break;
                case Direction.N:
                    n = obj;
                    break;
                case Direction.W:
                    w = obj;
                    break;
                case Direction.SW:
                    sw = obj;
                    break;
                case Direction.SE:
                    se = obj;
                    break;
                case Direction.NW:
                    nw = obj;
                    break;
                case Direction.NE:
                    ne = obj;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}