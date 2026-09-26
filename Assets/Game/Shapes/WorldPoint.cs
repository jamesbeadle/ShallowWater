namespace ShallowWater.Game.Shapes
{
    public readonly struct WorldPoint
    {
        public double East { get; }
        public double Height { get; }
        public double North { get; }

        public WorldPoint(double east, double height, double north)
        {
            East = east;
            Height = height;
            North = north;
        }
    }
}
