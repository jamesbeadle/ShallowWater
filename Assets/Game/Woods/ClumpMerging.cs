using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class ClumpMerging
    {
        private const double SpreadShare = 1.0;
        private const double LargestCellShare = 1.5;

        public static IReadOnlyList<LeafClump> Into(IReadOnlyList<LeafClump> clumps, double cellMetres)
        {
            var groups = clumps.GroupBy(clump => CellOf(clump.Centre, cellMetres));
            return groups.Select(group => Merged(group.ToList(), cellMetres)).ToList();
        }

        private static (long, long, long) CellOf(WorldPoint centre, double cellMetres)
        {
            var east = (long)Math.Floor(centre.East / cellMetres);
            var up = (long)Math.Floor(centre.Height / cellMetres);
            var north = (long)Math.Floor(centre.North / cellMetres);
            return (east, up, north);
        }

        private static LeafClump Merged(IReadOnlyList<LeafClump> group, double cellMetres)
        {
            var centres = group.Select(clump => clump.Centre).ToList();
            var east = centres.Average(centre => centre.East);
            var height = centres.Average(centre => centre.Height);
            var middle = new WorldPoint(east, height, centres.Average(centre => centre.North));
            var largest = group.Max(clump => clump.SizeMetres);
            var spread = Math.Sqrt(group.Count) * group.Average(clump => clump.SizeMetres) * SpreadShare;
            var size = Math.Min(Math.Max(spread, largest), cellMetres * LargestCellShare);
            var outward = group.Aggregate(Offset.Up * 0, (sum, clump) => sum + clump.Outward).Normalised;
            return new LeafClump(middle, size, outward, group.Average(clump => clump.Depth), group[0].Seed);
        }
    }
}
