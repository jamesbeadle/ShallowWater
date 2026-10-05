using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public readonly struct FieldStrip
    {
        public FieldStrip(Surface surface, double insideMetres, double outsideMetres, double centreMetres)
        {
            Surface = surface;
            InsideMetres = insideMetres;
            OutsideMetres = outsideMetres;
            CentreMetres = centreMetres;
        }

        public Surface Surface { get; }
        public double InsideMetres { get; }
        public double OutsideMetres { get; }
        public double CentreMetres { get; }
        public bool HasWidth => InsideMetres > OutsideMetres;
    }
}
