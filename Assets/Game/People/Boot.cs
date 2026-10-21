using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Boot
    {
        private const double ShaftRadiusMetres = 0.06;
        private const double ShaftHeightMetres = 0.15;
        private const double ToeLiftMetres = 0.038;
        private const double ToeForwardMetres = 0.08;
        private const double HeelBackMetres = -0.05;
        private const int Sides = 12;
        private static readonly Offset ToeShape = new Offset(0.058, 0.044, 0.125);
        private static readonly Offset HeelShape = new Offset(0.052, 0.03, 0.04);

        private static readonly ProfilePoint[] Shaft =
        {
            new ProfilePoint(0.068, 0), new ProfilePoint(0.068, 0.03),
            new ProfilePoint(ShaftRadiusMetres, 0.05), new ProfilePoint(ShaftRadiusMetres, ShaftHeightMetres)
        };

        public static Shape Shaped(double soleMetres)
        {
            var boot = Lathe.Turned(new WorldPoint(0, soleMetres, 0), Shaft, Sides);
            boot.Append(Ball.Shaped(new WorldPoint(0, soleMetres + ToeLiftMetres, ToeForwardMetres), ToeShape, Sides));
            boot.Append(Ball.Shaped(new WorldPoint(0, soleMetres + HeelShape.Upward, HeelBackMetres), HeelShape, Sides));
            return boot;
        }
    }
}
