namespace ShallowWater.Game.Roads
{
    public readonly struct RoadMargin
    {
        private const double Half = 0.5;

        public RoadMargin(double fromTheCentreMetres, double toTheHedgeMetres, double heightMetres, double hedgeHalfWidthMetres)
        {
            FromTheCentreMetres = fromTheCentreMetres;
            ToTheHedgeMetres = toTheHedgeMetres;
            HeightMetres = heightMetres;
            HedgeHalfWidthMetres = hedgeHalfWidthMetres;
        }

        public double FromTheCentreMetres { get; }
        public double ToTheHedgeMetres { get; }
        public double HeightMetres { get; }
        public double HedgeHalfWidthMetres { get; }
        public double MiddleMetres => (FromTheCentreMetres + ToTheHedgeMetres) * Half;
        public double HalfWidthMetres => (ToTheHedgeMetres - FromTheCentreMetres) * Half;
        public double HedgeCentreMetres => ToTheHedgeMetres + HedgeHalfWidthMetres;
    }
}
