using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Guns
{
    public static class TelescopeShapes
    {
        private const int Sides = 10;
        private const int MountSides = 4;
        public const double AxisAboveTheButtMetres = 0.082;
        private const double TubeRadiusMetres = 0.0125;
        private const double OcularBellRadiusMetres = 0.016;
        private const double ObjectiveBellRadiusMetres = 0.017;
        private const double LensDepthMetres = 0.002;
        private const double MountRadiusMetres = 0.006;
        private const double OnTheCentreLine = 0;
        private static readonly WorldPoint Ocular = new WorldPoint(OnTheCentreLine, AxisAboveTheButtMetres, 0.25);
        private static readonly WorldPoint OcularShoulder = new WorldPoint(OnTheCentreLine, AxisAboveTheButtMetres, 0.3);
        private static readonly WorldPoint ObjectiveShoulder = new WorldPoint(OnTheCentreLine, AxisAboveTheButtMetres, 0.47);
        private static readonly WorldPoint Objective = new WorldPoint(OnTheCentreLine, AxisAboveTheButtMetres, 0.53);
        private static readonly double[] OcularBellRadiiMetres = { OcularBellRadiusMetres, TubeRadiusMetres };
        private static readonly double[] ObjectiveBellRadiiMetres = { TubeRadiusMetres, ObjectiveBellRadiusMetres };
        private static readonly double[] MountsAlongTheBarrelMetres = { 0.36, 0.44 };

        public static void MountOn(SurfaceShapes rifle)
        {
            rifle.Add(Surface.GunMetal, TaperedTube.Along(new[] { Ocular, OcularShoulder }, OcularBellRadiiMetres, Sides));
            rifle.Add(Surface.GunMetal, Tube.Along(new[] { OcularShoulder, ObjectiveShoulder }, TubeRadiusMetres, Sides));
            rifle.Add(Surface.GunMetal, TaperedTube.Along(new[] { ObjectiveShoulder, Objective }, ObjectiveBellRadiiMetres, Sides));
            rifle.Add(Surface.LampGlass, Lens(Ocular, OcularBellRadiusMetres));
            rifle.Add(Surface.LampGlass, Lens(Objective, ObjectiveBellRadiusMetres));
            foreach (var alongMetres in MountsAlongTheBarrelMetres) rifle.Add(Surface.GunMetal, Mount(alongMetres));
        }

        private static Shape Lens(WorldPoint centre, double radiusMetres)
        {
            return Ball.Shaped(centre, new Offset(radiusMetres, radiusMetres, LensDepthMetres), Sides);
        }

        private static Shape Mount(double alongMetres)
        {
            var foot = new WorldPoint(OnTheCentreLine, RifleShapes.BarrelTopAboveTheButtMetres, alongMetres);
            var saddle = new WorldPoint(OnTheCentreLine, AxisAboveTheButtMetres, alongMetres);
            return Tube.Along(new[] { foot, saddle }, MountRadiusMetres, MountSides);
        }
    }
}
