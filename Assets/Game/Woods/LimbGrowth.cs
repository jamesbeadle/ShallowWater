using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class LimbGrowth
    {
        private const double FullTurnRadians = 2 * Math.PI;

        public static Limb Grown(LimbStart start, BranchLevel level, int order, Random random)
        {
            var path = new List<WorldPoint> { start.Foot };
            var radii = new List<double> { start.RadiusMetres };
            var heading = start.Heading;
            var stepMetres = start.LengthMetres / level.Segments;
            var rise = Offset.Up * (level.Rise * stepMetres);
            for (var step = 1; step <= level.Segments; step++)
            {
                heading = (heading + rise + Wander(heading, random) * level.Kink).Normalised;
                path.Add((heading * stepMetres).From(path[step - 1]));
                var share = (double)step / level.Segments;
                radii.Add(start.RadiusMetres * (1 - share * (1 - level.TipShare)));
            }
            return new Limb(path, radii, order);
        }

        private static Offset Wander(Offset heading, Random random)
        {
            var side = heading.AnyRightAngle();
            return side.TurnedAbout(heading, random.NextDouble() * FullTurnRadians) * random.NextDouble();
        }
    }
}
