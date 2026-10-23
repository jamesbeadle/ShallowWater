using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Boot
    {
        private const double AnkleRadiusMetres = 0.052;
        private const double SoleBottomMetres = -AskewForm.AnkleAboveTheSoleMetres;
        private const double SoleTopMetres = SoleBottomMetres + 0.02;
        private const double HeelTopMetres = SoleBottomMetres + 0.038;
        private const double HeelFrontMetres = -0.005;
        private const double HeelHalfWidthMetres = 0.047;
        private const int Sides = 10;
        private static readonly WorldPoint Ankle = new WorldPoint(0, 0, 0);
        private static readonly WorldPoint ToeCap = new WorldPoint(0, -0.038, 0.17);
        private static readonly Offset ToeCapShape = new Offset(0.04, 0.024, 0.032);

        private static readonly GroundPoint[] Outline =
        {
            new GroundPoint(-0.03, -AskewForm.HeelBehindTheAnkleMetres), new GroundPoint(0.03, -AskewForm.HeelBehindTheAnkleMetres), new GroundPoint(0.048, -0.04),
            new GroundPoint(0.052, 0.08), new GroundPoint(0.046, 0.14), new GroundPoint(0.028, AskewForm.ToeAheadOfTheAnkleMetres),
            new GroundPoint(-0.028, AskewForm.ToeAheadOfTheAnkleMetres), new GroundPoint(-0.046, 0.14), new GroundPoint(-0.052, 0.08), new GroundPoint(-0.048, -0.04)
        };

        private static readonly WorldPoint[] Upper =
        {
            new WorldPoint(0, -0.008, -0.035), new WorldPoint(0, -0.022, 0.06), new WorldPoint(0, -0.036, 0.16)
        };

        private static readonly double[] UpperRadii = { 0.05, 0.046, 0.036 };

        public static SurfaceShapes Shaped()
        {
            var boot = new SurfaceShapes();
            boot.Add(Surface.Boots, Ball.Of(Ankle, AnkleRadiusMetres, Sides));
            boot.AddBlock(Surface.Boots, new GroundRing(Outline), SoleBottomMetres, SoleTopMetres);
            boot.AddBlock(Surface.Boots, Heel(), SoleBottomMetres, HeelTopMetres);
            boot.Add(Surface.Boots, TaperedTube.Along(Upper, UpperRadii, Sides));
            boot.Add(Surface.Boots, Ball.Shaped(ToeCap, ToeCapShape, Sides));
            return boot;
        }

        private static GroundRing Heel()
        {
            var back = -AskewForm.HeelBehindTheAnkleMetres;
            var corners = new[]
            {
                new GroundPoint(-HeelHalfWidthMetres, back), new GroundPoint(HeelHalfWidthMetres, back),
                new GroundPoint(HeelHalfWidthMetres, HeelFrontMetres), new GroundPoint(-HeelHalfWidthMetres, HeelFrontMetres)
            };
            return new GroundRing(corners);
        }
    }
}
