namespace ShallowWater.Game.Shapes
{
    public readonly struct Fitting
    {
        public static readonly Fitting None = new Fitting(0, 0, 0, 0);

        public Fitting(double widthMetres, double heightMetres, int pattern, int paint)
        {
            WidthMetres = widthMetres;
            HeightMetres = heightMetres;
            Pattern = pattern;
            Paint = paint;
        }

        public double WidthMetres { get; }
        public double HeightMetres { get; }
        public int Pattern { get; }
        public int Paint { get; }
    }
}
