namespace ShallowWater.Game.Boat
{
    public readonly struct HullPoint
    {
        public HullPoint(double ahead, double toStarboard)
        {
            Ahead = ahead;
            ToStarboard = toStarboard;
        }

        public double Ahead { get; }
        public double ToStarboard { get; }
    }
}
