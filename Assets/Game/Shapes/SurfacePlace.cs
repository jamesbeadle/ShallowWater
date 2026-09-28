namespace ShallowWater.Game.Shapes
{
    public readonly struct SurfacePlace
    {
        public static readonly SurfacePlace Unmarked = new SurfacePlace(0, 0);

        public SurfacePlace(double along, double across)
        {
            Along = along;
            Across = across;
        }

        public double Along { get; }
        public double Across { get; }
    }
}
