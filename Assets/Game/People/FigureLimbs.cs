using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FigureLimbs
    {
        private const double AnkleAboveTheGroundMetres = 0.06;
        private const double ThighRadiusMetres = 0.09;
        private const double KneeRadiusMetres = 0.068;
        private const double CalfRadiusMetres = 0.066;
        private const double AnkleRadiusMetres = 0.052;
        private const double ShoulderRadiusMetres = 0.066;
        private const double ElbowRadiusMetres = 0.058;
        private const double WristRadiusMetres = 0.048;
        private const double CuffRadiusMetres = 0.054;
        private const double CuffMetres = 0.05;
        private const double Level = 0;
        private static readonly WorldPoint Joint = new WorldPoint(Level, Level, Level);
        private static readonly WorldPoint Wrist = new WorldPoint(Level, -AskewForm.ForearmMetres, Level);

        private static readonly ProfilePoint[] Cuff =
        {
            new ProfilePoint(CuffRadiusMetres, -AskewForm.ForearmMetres), new ProfilePoint(CuffRadiusMetres, -AskewForm.ForearmMetres + CuffMetres)
        };

        public static SurfaceShapes Thigh(Outfit outfit)
        {
            return Limb(outfit.Trousers, AskewForm.ThighMetres, ThighRadiusMetres, KneeRadiusMetres);
        }

        public static SurfaceShapes Shin(Outfit outfit)
        {
            var shin = Limb(outfit.Trousers, AskewForm.ShinMetres, CalfRadiusMetres, AnkleRadiusMetres);
            shin.Add(outfit.Trousers, Ball.Of(Joint, KneeRadiusMetres, AskewForm.LimbSides));
            shin.Add(Surface.Boots, Boot.Shaped(-AskewForm.ShinMetres - AnkleAboveTheGroundMetres));
            return shin;
        }

        public static SurfaceShapes UpperArm(Outfit outfit)
        {
            var upperArm = Limb(outfit.Coat, AskewForm.UpperArmMetres, ShoulderRadiusMetres, ElbowRadiusMetres);
            upperArm.Add(outfit.Coat, Ball.Of(Joint, ShoulderRadiusMetres, AskewForm.LimbSides));
            return upperArm;
        }

        public static SurfaceShapes Forearm(Outfit outfit)
        {
            var forearm = Limb(outfit.Coat, AskewForm.ForearmMetres, ElbowRadiusMetres, WristRadiusMetres);
            forearm.Add(outfit.Coat, Ball.Of(Joint, ElbowRadiusMetres, AskewForm.LimbSides));
            forearm.Add(outfit.Coat, Lathe.Turned(Joint, Cuff, AskewForm.LimbSides));
            forearm.Add(Surface.Skin, Hand.Hanging(Wrist));
            return forearm;
        }

        private static SurfaceShapes Limb(Surface surface, double lengthMetres, double topRadiusMetres, double bottomRadiusMetres)
        {
            var path = new[] { Joint, new WorldPoint(Level, -lengthMetres, Level) };
            var limb = new SurfaceShapes();
            limb.Add(surface, TaperedTube.Along(path, new[] { topRadiusMetres, bottomRadiusMetres }, AskewForm.LimbSides));
            return limb;
        }
    }
}
