namespace ShallowWater.Game.Ground
{
    public readonly struct GroundPoint
    {
        public double East { get; }
        public double North { get; }

        public GroundPoint(double east, double north)
        {
            East = east;
            North = north;
        }
    }
}
