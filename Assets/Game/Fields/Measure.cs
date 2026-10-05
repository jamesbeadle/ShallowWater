namespace ShallowWater.Game.Fields
{
    public readonly struct Measure
    {
        public Measure(double lengthMetres, double widthMetres)
        {
            LengthMetres = lengthMetres;
            WidthMetres = widthMetres;
        }

        public double LengthMetres { get; }
        public double WidthMetres { get; }
    }
}
