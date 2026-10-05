namespace ShallowWater.Game.Fields
{
    public readonly struct HedgeGap
    {
        public HedgeGap(double fromMetres, double toMetres, GapFurniture furniture)
        {
            FromMetres = fromMetres;
            ToMetres = toMetres;
            Furniture = furniture;
        }

        public double FromMetres { get; }
        public double ToMetres { get; }
        public GapFurniture Furniture { get; }
        public double WidthMetres => ToMetres - FromMetres;

        public bool IsOverlapping(double fromMetres, double toMetres)
        {
            return fromMetres < ToMetres && toMetres > FromMetres;
        }
    }
}
