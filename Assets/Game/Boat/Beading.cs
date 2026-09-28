using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class Beading
    {
        public static IReadOnlyList<WorldPoint> Across(double along, double side, double halfWidthMetres, Rise rise, double proudMetres)
        {
            var inner = side * halfWidthMetres;
            var outer = inner + side * proudMetres;
            var profile = new[]
            {
                new WorldPoint(inner, rise.FootMetres, along), new WorldPoint(outer, rise.FootMetres, along),
                new WorldPoint(outer, rise.TopMetres, along), new WorldPoint(inner, rise.TopMetres, along)
            };
            var isOnThePort = side == BoatSides.Port;
            return isOnThePort ? profile : profile.Reverse().ToArray();
        }
    }
}
