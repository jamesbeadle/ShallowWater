using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class Offshoots
    {
        private const double FullTurnRadians = 2 * Math.PI;
        private const double GoldenAngleRadians = 2.39996;
        private const double DegreesToRadians = Math.PI / 180;
        private const double PlacementJitter = 0.4;
        private const double TurnJitterRadians = 0.35;
        private const double MiddleOfTheWhorl = 0.5;

        public static List<LimbStart> Along(Limb parent, BranchLevel level, Func<double, double> lengthShareAt, Random random)
        {
            var starts = new List<LimbStart>();
            var whorls = (level.Count + level.PerWhorl - 1) / level.PerWhorl;
            var turn = random.NextDouble() * FullTurnRadians;
            for (var whorl = 0; whorl < whorls; whorl++)
            {
                var placing = (whorl + MiddleOfTheWhorl + Jitter(random) * PlacementJitter) / whorls;
                var share = level.FromShare + (1 - level.FromShare) * placing;
                var point = parent.At(share);
                var length = parent.LengthMetres * level.LengthShare * lengthShareAt(share);
                turn += GoldenAngleRadians;
                for (var member = 0; member < level.PerWhorl; member++)
                {
                    var around = turn + FullTurnRadians * member / level.PerWhorl + Jitter(random) * TurnJitterRadians;
                    starts.Add(Offshoot(point, level, around, length, random));
                }
            }
            return starts;
        }

        private static LimbStart Offshoot(LimbPoint point, BranchLevel level, double around, double length, Random random)
        {
            var heading = point.Heading;
            var side = heading.AnyRightAngle().TurnedAbout(heading, around);
            var angle = (level.AngleDegrees + Jitter(random) * level.AngleSpreadDegrees) * DegreesToRadians;
            var outward = heading * Math.Cos(angle) + side * Math.Sin(angle);
            return new LimbStart(point.Place, outward, length, point.RadiusMetres * level.RadiusShare);
        }

        private static double Jitter(Random random)
        {
            return random.NextDouble() * 2 - 1;
        }
    }
}
