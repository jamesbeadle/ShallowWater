using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewLimbs
    {
        private const double AnkleAboveTheGroundMetres = 0.06;
        private const double ThighRadiusMetres = 0.085;
        private const double KneeRadiusMetres = 0.064;
        private const double CalfRadiusMetres = 0.062;
        private const double AnkleRadiusMetres = 0.052;
        private const double ShoulderRadiusMetres = 0.064;
        private const double ElbowRadiusMetres = 0.052;
        private const double WristRadiusMetres = 0.044;
        private const double Level = 0;
        private static readonly WorldPoint Joint = new WorldPoint(Level, Level, Level);

        private static readonly ProfilePoint[] Hand =
        {
            new ProfilePoint(0.03, 0), new ProfilePoint(0.045, -0.04), new ProfilePoint(0.035, -0.08), new ProfilePoint(0, -0.095)
        };

        public static SurfaceShapes Thigh()
        {
            return Limb(Surface.Trousers, AskewForm.ThighMetres, ThighRadiusMetres, KneeRadiusMetres);
        }

        public static SurfaceShapes Shin()
        {
            var shin = Limb(Surface.Trousers, AskewForm.ShinMetres, CalfRadiusMetres, AnkleRadiusMetres);
            shin.Add(Surface.Trousers, Ball.Of(Joint, KneeRadiusMetres, AskewForm.LimbSides));
            shin.Add(Surface.Boots, Boot.Shaped(-AskewForm.ShinMetres - AnkleAboveTheGroundMetres));
            return shin;
        }

        public static SurfaceShapes UpperArm()
        {
            var upperArm = Limb(Surface.Jacket, AskewForm.UpperArmMetres, ShoulderRadiusMetres, ElbowRadiusMetres);
            upperArm.Add(Surface.Jacket, Ball.Of(Joint, ShoulderRadiusMetres, AskewForm.LimbSides));
            return upperArm;
        }

        public static SurfaceShapes Forearm()
        {
            var forearm = Limb(Surface.Jacket, AskewForm.ForearmMetres, ElbowRadiusMetres, WristRadiusMetres);
            forearm.Add(Surface.Jacket, Ball.Of(Joint, ElbowRadiusMetres, AskewForm.LimbSides));
            forearm.Add(Surface.Skin, Lathe.Turned(new WorldPoint(Level, -AskewForm.ForearmMetres, Level), Hand, AskewForm.LimbSides));
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
