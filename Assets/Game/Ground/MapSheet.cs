namespace ShallowWater.Game.Ground
{
    public sealed class MapSheet
    {
        public double West { get; }
        public double East { get; }
        public double South { get; }
        public double North { get; }

        public MapSheet(double west, double east, double south, double north)
        {
            West = west;
            East = east;
            South = south;
            North = north;
        }

        public double WidthMetres => East - West;
        public double DepthMetres => North - South;
        public GroundPoint Centre => new GroundPoint((West + East) / 2, (South + North) / 2);
    }
}
