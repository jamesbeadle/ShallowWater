using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Pound
{
    public static class ChainSag
    {
        private const double ParabolaAtItsMiddle = 0.25;

        public static List<WorldPoint> Between(WorldPoint from, WorldPoint to, double sagMetres, int links)
        {
            var points = new List<WorldPoint>();
            for (var link = 0; link <= links; link++)
            {
                var fraction = (double)link / links;
                var droop = sagMetres * fraction * (1 - fraction) / ParabolaAtItsMiddle;
                points.Add(Towards(from, to, fraction, droop));
            }
            return points;
        }

        private static WorldPoint Towards(WorldPoint from, WorldPoint to, double fraction, double droop)
        {
            var east = from.East + (to.East - from.East) * fraction;
            var height = from.Height + (to.Height - from.Height) * fraction - droop;
            var north = from.North + (to.North - from.North) * fraction;
            return new WorldPoint(east, height, north);
        }
    }
}
