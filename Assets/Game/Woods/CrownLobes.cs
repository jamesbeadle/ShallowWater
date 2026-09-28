using System;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class CrownLobes
    {
        private const double BaseReach = 0.78;
        private const double LobeReach = 0.3;
        private const double LobeSharpness = 3;
        private const double UndersideFlattening = 0.75;
        private const double Level = 0;

        private static readonly double[][] Lobes =
        {
            new[] { 0.0, 1.0, 0.0 }, new[] { 0.8, 0.5, 0.3 }, new[] { -0.6, 0.55, 0.55 },
            new[] { 0.2, 0.45, -0.85 }, new[] { -0.75, 0.35, -0.5 }, new[] { 0.9, -0.1, -0.4 },
            new[] { -0.9, -0.05, 0.35 }, new[] { 0.35, -0.2, 0.9 }, new[] { -0.2, -0.35, -0.9 }
        };

        public static WorldPoint Surface(double east, double up, double north, double radius)
        {
            var reach = radius * (BaseReach + LobeReach * Math.Pow(Nearest(east, up, north), LobeSharpness));
            var isUnderneath = up < Level;
            var height = isUnderneath ? up * UndersideFlattening : up;
            return new WorldPoint(east * reach, height * reach, north * reach);
        }

        private static double Nearest(double east, double up, double north)
        {
            return Lobes.Max(lobe => Math.Max(0, Towards(lobe, east, up, north)));
        }

        private static double Towards(double[] lobe, double east, double up, double north)
        {
            var length = Math.Sqrt(lobe[0] * lobe[0] + lobe[1] * lobe[1] + lobe[2] * lobe[2]);
            return (lobe[0] * east + lobe[1] * up + lobe[2] * north) / length;
        }
    }
}
