using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FigureLimbs
    {
        private const double ThighRadiusMetres = 0.1;
        private const double CalfRadiusMetres = 0.078;
        private const double CalfBelowTheKneeMetres = 0.12;
        private const double AnkleRadiusMetres = 0.056;
        private const double TurnUpRadiusMetres = 0.063;
        private const double TurnUpMetres = 0.045;
        private const double BootShaftRadiusMetres = 0.058;
        private const double BootShaftAboveTheAnkleMetres = 0.1;
        private const double BootShaftBelowTheAnkleMetres = 0.025;
        private const double ShoulderRadiusMetres = 0.078;
        private const double ElbowRadiusMetres = 0.066;
        private const double WristRadiusMetres = 0.054;
        private const double CuffRadiusMetres = 0.06;
        private const double CuffMetres = 0.06;
        private const double Level = 0;
        private static readonly WorldPoint Joint = new WorldPoint(Level, Level, Level);

        private static readonly ProfilePoint[] TurnUp =
        {
            new ProfilePoint(TurnUpRadiusMetres, -AskewForm.ShinMetres + BootShaftBelowTheAnkleMetres), new ProfilePoint(TurnUpRadiusMetres, -AskewForm.ShinMetres + BootShaftBelowTheAnkleMetres + TurnUpMetres)
        };

        private static readonly ProfilePoint[] BootShaft =
        {
            new ProfilePoint(BootShaftRadiusMetres, -AskewForm.ShinMetres - BootShaftBelowTheAnkleMetres), new ProfilePoint(BootShaftRadiusMetres, -AskewForm.ShinMetres + BootShaftAboveTheAnkleMetres)
        };

        private static readonly ProfilePoint[] Cuff =
        {
            new ProfilePoint(CuffRadiusMetres, -AskewForm.ForearmMetres), new ProfilePoint(CuffRadiusMetres, -AskewForm.ForearmMetres + CuffMetres)
        };

        public static SurfaceShapes Thigh(Outfit outfit)
        {
            return Limb(outfit.Trousers, new[] { Joint, Down(AskewForm.ThighMetres) }, new[] { ThighRadiusMetres, AskewForm.KneeRadiusMetres });
        }

        public static SurfaceShapes Shin(Outfit outfit)
        {
            var path = new[] { Joint, Down(CalfBelowTheKneeMetres), Down(AskewForm.ShinMetres) };
            var shin = Limb(outfit.Trousers, path, new[] { AskewForm.KneeRadiusMetres, CalfRadiusMetres, AnkleRadiusMetres });
            shin.Add(outfit.Trousers, Ball.Of(Joint, AskewForm.KneeRadiusMetres, AskewForm.LimbSides));
            shin.Add(outfit.Trousers, Lathe.Turned(Joint, TurnUp, AskewForm.LimbSides));
            shin.Add(Surface.Boots, Lathe.Turned(Joint, BootShaft, AskewForm.LimbSides));
            return shin;
        }

        public static SurfaceShapes UpperArm(Outfit outfit)
        {
            var upperArm = Limb(outfit.Coat, new[] { Joint, Down(AskewForm.UpperArmMetres) }, new[] { ShoulderRadiusMetres, ElbowRadiusMetres });
            upperArm.Add(outfit.Coat, Ball.Of(Joint, ShoulderRadiusMetres, AskewForm.LimbSides));
            return upperArm;
        }

        public static SurfaceShapes Forearm(Outfit outfit)
        {
            var forearm = Limb(outfit.Coat, new[] { Joint, Down(AskewForm.ForearmMetres) }, new[] { ElbowRadiusMetres, WristRadiusMetres });
            forearm.Add(outfit.Coat, Ball.Of(Joint, ElbowRadiusMetres, AskewForm.LimbSides));
            forearm.Add(outfit.Coat, Lathe.Turned(Joint, Cuff, AskewForm.LimbSides));
            return forearm;
        }

        private static WorldPoint Down(double metres)
        {
            return new WorldPoint(Level, -metres, Level);
        }

        private static SurfaceShapes Limb(Surface surface, WorldPoint[] path, double[] radiiMetres)
        {
            var limb = new SurfaceShapes();
            limb.Add(surface, TaperedTube.Along(path, radiiMetres, AskewForm.LimbSides));
            return limb;
        }
    }
}
