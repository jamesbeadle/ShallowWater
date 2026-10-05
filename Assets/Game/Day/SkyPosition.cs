namespace ShallowWater.Game.Day
{
    public readonly struct SkyPosition
    {
        private const double HalfTurnDegrees = 180;

        public double HeightDegrees { get; }
        public double BearingDegrees { get; }

        public SkyPosition(double heightDegrees, double bearingDegrees)
        {
            HeightDegrees = heightDegrees;
            BearingDegrees = bearingDegrees;
        }

        public SkyPosition Opposite => new SkyPosition(-HeightDegrees, BearingDegrees + HalfTurnDegrees);
    }
}
