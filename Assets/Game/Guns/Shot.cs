using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Wildlife;

namespace ShallowWater.Game.Guns
{
    public static class Shot
    {
        private const double AtTheMuzzle = 0;

        public static Quarry Hit(WorldPoint muzzle, Offset aim, IEnumerable<Quarry> targets, double rangeMetres)
        {
            var line = aim.Normalised;
            Quarry nearest = null;
            var nearestAlong = rangeMetres;
            foreach (var target in targets)
            {
                var along = AlongTheLineOf(muzzle, line, target);
                var isNearerAhead = along > AtTheMuzzle && along < nearestAlong;
                if (!isNearerAhead) continue;
                nearest = target;
                nearestAlong = along;
            }
            return nearest;
        }

        private static double AlongTheLineOf(WorldPoint muzzle, Offset line, Quarry target)
        {
            var toTheTarget = Offset.Between(muzzle, target.Position);
            var along = toTheTarget.Dot(line);
            var offTheLine = (toTheTarget - line * along).Length;
            var isStruck = offTheLine < target.BodyRadiusMetres;
            return isStruck ? along : AtTheMuzzle;
        }
    }
}
