using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Roads
{
    public readonly struct RoadClass
    {
        private const double Half = 0.5;

        public RoadClass(Surface surface, double widthMetres, double vergeWidthMetres, double heightMetres, Furniture furniture)
        {
            Surface = surface;
            WidthMetres = widthMetres;
            VergeWidthMetres = vergeWidthMetres;
            HeightMetres = heightMetres;
            Furniture = furniture;
        }

        public Surface Surface { get; }
        public double WidthMetres { get; }
        public double VergeWidthMetres { get; }
        public double HeightMetres { get; }
        public Furniture Furniture { get; }
        public double HalfWidthMetres => WidthMetres * Half;
        public bool HasCentreLine => Furniture != Furniture.None;
        public bool HasTelegraphPoles => Furniture == Furniture.CentreLineAndTelegraphPoles;
    }
}
