using System.Collections.Generic;

namespace ShallowWater.Game.Fields
{
    public static class FootpathLines
    {
        private const double ReachIntoTheNextHedgeMetres = 8;

        public static List<FootpathLine> Of(IEnumerable<Farm> farms)
        {
            var footpaths = new List<FootpathLine>();
            foreach (var farm in farms) AddFootpathsOf(farm.Outline, footpaths);
            return footpaths;
        }

        private static void AddFootpathsOf(ConvexOutline outline, List<FootpathLine> footpaths)
        {
            for (var edge = 0; edge < outline.EdgeCount; edge++)
            {
                var boundary = outline.Boundaries[edge];
                if (!boundary.HasFootpath) continue;
                var direction = outline.EdgeDirection(edge);
                var inward = direction.RightAngleClockwise * FieldStrips.FootpathCentreMetres;
                var reach = direction * ReachIntoTheNextHedgeMetres;
                footpaths.Add(new FootpathLine(outline.EdgeStart(edge) + inward - reach, outline.EdgeEnd(edge) + inward + reach));
            }
        }
    }
}
