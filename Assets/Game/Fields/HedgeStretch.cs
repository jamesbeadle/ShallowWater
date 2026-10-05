namespace ShallowWater.Game.Fields
{
    public readonly struct HedgeStretch
    {
        public HedgeStretch(double fromMetres, double toMetres)
        {
            FromMetres = fromMetres;
            ToMetres = toMetres;
        }

        public double FromMetres { get; }
        public double ToMetres { get; }
        public double LengthMetres => ToMetres - FromMetres;
    }
}
