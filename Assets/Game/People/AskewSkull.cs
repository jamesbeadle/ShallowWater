using System;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewSkull
    {
        private const int Sides = 24;
        private const double FaceForward = Math.PI / 2;
        private const double StubbleSpreadRadians = Math.PI * 0.6;
        private const double HairSpreadRadians = Math.PI * 0.62;
        private const double StubbleProudMetres = 0.0015;
        private const double HairProudMetres = 0.002;
        private const int StubbleFirstLevel = 2;
        private const int StubbleLastLevel = 6;
        private const int HairFirstLevel = 4;
        private const int HairLastLevel = 12;
        private static readonly WorldPoint Neck = new WorldPoint(0, 0, 0);

        private static readonly SkullLevel[] Levels =
        {
            new SkullLevel(-0.05, 0.058, 0.052, 0.052), new SkullLevel(0, 0.062, 0.056, 0.054), new SkullLevel(0.02, 0.078, 0.063, 0.056),
            new SkullLevel(0.042, 0.1, 0.071, 0.06), new SkullLevel(0.065, 0.1, 0.079, 0.068), new SkullLevel(0.09, 0.099, 0.08, 0.076),
            new SkullLevel(0.115, 0.098, 0.077, 0.083), new SkullLevel(0.14, 0.1, 0.083, 0.088), new SkullLevel(0.156, 0.093, 0.082, 0.09),
            new SkullLevel(0.175, 0.105, 0.082, 0.092), new SkullLevel(0.2, 0.1, 0.082, 0.094), new SkullLevel(0.23, 0.09, 0.08, 0.09),
            new SkullLevel(0.255, 0.07, 0.065, 0.07), new SkullLevel(0.275, 0, 0, 0)
        };

        private static readonly SkullBump[] Bumps =
        {
            new SkullBump(0.009, 0.7, 0.142, 0.35, 0.016), new SkullBump(0.009, 1.3, 0.068, 0.4, 0.022),
            new SkullBump(0.005, 0.45, 0.176, 0.5, 0.012), new SkullBump(-0.006, 0.38, 0.156, 0.22, 0.012)
        };

        public static Shape Skin()
        {
            return Sculpt.Rings(Neck, Heights(0, Levels.Length - 1), Turn.Whole(Sides), (ring, around) => RadiusAt(ring, around));
        }

        public static Shape Stubble()
        {
            var turn = Turn.Through(FaceForward - StubbleSpreadRadians, FaceForward + StubbleSpreadRadians, Sides);
            return Sculpt.Rings(Neck, Heights(StubbleFirstLevel, StubbleLastLevel), turn, (ring, around) => RadiusAt(ring + StubbleFirstLevel, around) + StubbleProudMetres);
        }

        public static Shape Hair()
        {
            var turn = Turn.Through(FaceForward + HairSpreadRadians, FaceForward + 2 * Math.PI - HairSpreadRadians, Sides);
            return Sculpt.Rings(Neck, Heights(HairFirstLevel, HairLastLevel), turn, (ring, around) => RadiusAt(ring + HairFirstLevel, around) + HairProudMetres);
        }

        private static double[] Heights(int firstLevel, int lastLevel)
        {
            return Levels.Skip(firstLevel).Take(lastLevel - firstLevel + 1).Select(level => level.HeightMetres).ToArray();
        }

        private static double RadiusAt(int ring, double around)
        {
            var level = Levels[ring];
            var bumps = Bumps.Sum(bump => bump.At(level.HeightMetres, around));
            return level.RadiusTowards(around) + bumps;
        }
    }
}
