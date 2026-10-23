using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public static class RoundedCorners
    {
        private const int PointsPerCorner = 6;
        private const double Half = 0.5;
        private const double Whole = 1;

        public static List<WorldPoint> Of(IReadOnlyList<WorldPoint> path, double radiusMetres)
        {
            var rounded = new List<WorldPoint> { path[0] };
            var last = path.Count - 1;
            for (var corner = 1; corner < last; corner++) rounded.AddRange(Rounded(path[corner - 1], path[corner], path[corner + 1], radiusMetres));
            rounded.Add(path[last]);
            return rounded;
        }

        private static IEnumerable<WorldPoint> Rounded(WorldPoint before, WorldPoint corner, WorldPoint after, double radiusMetres)
        {
            var into = Offset.Between(before, corner);
            var outOf = Offset.Between(corner, after);
            var cut = Math.Min(radiusMetres, Math.Min(into.Length, outOf.Length) * Half);
            var from = (into * (-cut / into.Length)).From(corner);
            var to = (outOf * (cut / outOf.Length)).From(corner);
            for (var point = 0; point <= PointsPerCorner; point++) yield return Bent(from, corner, to, (double)point / PointsPerCorner);
        }

        private static WorldPoint Bent(WorldPoint from, WorldPoint corner, WorldPoint to, double share)
        {
            var remaining = Whole - share;
            var towardsTheCorner = Offset.Between(from, corner) * (2 * remaining * share);
            var acrossTheBend = Offset.Between(from, to) * (share * share);
            return (towardsTheCorner + acrossTheBend).From(from);
        }
    }
}
