using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Boot
    {
        private const double ShaftRadiusMetres = 0.058;
        private const double ShaftHeightMetres = 0.14;
        private const double ToeRadiusMetres = 0.07;
        private const double ToeForwardMetres = 0.075;
        private const double ToeLiftMetres = 0.04;
        private const int Sides = 12;
        private static readonly Offset ToeShape = new Offset(0.82, 0.62, 1.75);

        private static readonly ProfilePoint[] Shaft =
        {
            new ProfilePoint(0.066, 0), new ProfilePoint(0.066, 0.03),
            new ProfilePoint(ShaftRadiusMetres, 0.05), new ProfilePoint(ShaftRadiusMetres, ShaftHeightMetres)
        };

        public static Shape Shaped(double soleMetres)
        {
            var boot = Lathe.Turned(new WorldPoint(0, soleMetres, 0), Shaft, Sides);
            var toe = Scaled.Of(Ball.Of(new WorldPoint(0, 0, 0), ToeRadiusMetres, Sides), ToeShape);
            boot.Append(Scaled.Moved(toe, new Offset(0, soleMetres + ToeLiftMetres, ToeForwardMetres)));
            return boot;
        }
    }
}
