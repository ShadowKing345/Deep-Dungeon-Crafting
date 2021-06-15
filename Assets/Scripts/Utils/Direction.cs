namespace Utils
{
    public enum Direction
    {
        S = 1,
        W = 2,
        N = 3,
        E = 4,
        
        SW = 5,
        SE = 6,
        NW = 7,
        NE = 8
    }

    public static class Extension
    {
        public static int GetDirectionFromInterCardinal(this Direction direction) =>
            direction switch
            {
                Direction.SW => 1,
                Direction.SE => 2,
                Direction.NW => 3,
                Direction.NE => 4,
                _ => (int) direction
            };
    }
}