using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Guns
{
    public static class RifleShapes
    {
        private const int Sides = 8;
        private const double BarrelRadiusMetres = 0.011;
        private const double ForeEndRadiusMetres = 0.018;
        private const double BoltRadiusMetres = 0.012;
        private const double ForesightRadiusMetres = 0.002;
        private const double BacksightRadiusMetres = 0.003;
        public const double SightsAboveTheButtMetres = 0.06;
        private static readonly WorldPoint Butt = new WorldPoint(0, -0.03, 0);
        private static readonly WorldPoint Wrist = new WorldPoint(0, 0.0, 0.42);
        private static readonly WorldPoint ForeEndTip = new WorldPoint(0, 0.012, 0.78);
        private static readonly WorldPoint Breech = new WorldPoint(0, 0.032, 0.4);
        private static readonly WorldPoint Muzzle = new WorldPoint(0, 0.032, 1.08);
        private static readonly WorldPoint BoltHandle = new WorldPoint(0.035, 0.04, 0.38);
        private static readonly WorldPoint ForesightFoot = new WorldPoint(0, 0.043, 1.06);
        private static readonly WorldPoint ForesightTip = new WorldPoint(0, SightsAboveTheButtMetres, 1.06);
        private static readonly WorldPoint BacksightLeft = new WorldPoint(-0.012, SightsAboveTheButtMetres - BacksightRadiusMetres, 0.5);
        private static readonly WorldPoint BacksightRight = new WorldPoint(0.012, SightsAboveTheButtMetres - BacksightRadiusMetres, 0.5);
        private static readonly double[] StockRadiiMetres = { 0.055, 0.03 };

        public static SurfaceShapes Rifle()
        {
            var rifle = new SurfaceShapes();
            rifle.Add(Surface.GunStock, TaperedTube.Along(new[] { Butt, Wrist }, StockRadiiMetres, Sides));
            rifle.Add(Surface.GunStock, Tube.Along(new[] { Wrist, ForeEndTip }, ForeEndRadiusMetres, Sides));
            rifle.Add(Surface.GunMetal, Tube.Along(new[] { Breech, Muzzle }, BarrelRadiusMetres, Sides));
            rifle.Add(Surface.GunMetal, Ball.Of(BoltHandle, BoltRadiusMetres, Sides));
            rifle.Add(Surface.GunMetal, Tube.Along(new[] { ForesightFoot, ForesightTip }, ForesightRadiusMetres, Sides));
            rifle.Add(Surface.GunMetal, Tube.Along(new[] { BacksightLeft, BacksightRight }, BacksightRadiusMetres, Sides));
            return rifle;
        }
    }
}
