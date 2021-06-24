namespace Utils
{
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

    public static class Extension
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
    }
}