using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public static class OutlineCut
    {
        private const double OnTheLineMetres = 1e-6;
        private const double MergedMetres = 0.01;
        private const double OnTheLine = 0;

        public static ConvexOutline KeepRight(ConvexOutline outline, CuttingLine line, Boundary cut)
        {
            var corners = new List<GroundPoint>();
            var boundaries = new List<Boundary>();
            for (var edge = 0; edge < outline.EdgeCount; edge++)
            {
                var start = outline.EdgeStart(edge);
                var end = outline.EdgeEnd(edge);
                var isStartKept = line.LeftOf(start) <= OnTheLineMetres;
                var isEndKept = line.LeftOf(end) <= OnTheLineMetres;
                if (isStartKept) Add(corners, boundaries, start, outline.Boundaries[edge]);
                if (isStartKept == isEndKept) continue;
                var crossing = line.CrossingOf(start, end);
                Add(corners, boundaries, crossing, isStartKept ? cut : outline.Boundaries[edge]);
            }
            DropRepeatedLastCorner(corners, boundaries);
            return new ConvexOutline(corners, boundaries);
        }

        public static List<GroundPoint> Crossings(ConvexOutline outline, CuttingLine line)
        {
            var crossings = new List<GroundPoint>();
            for (var edge = 0; edge < outline.EdgeCount; edge++)
            {
                var start = outline.EdgeStart(edge);
                var end = outline.EdgeEnd(edge);
                var isStartOnTheLeft = line.LeftOf(start) > OnTheLine;
                var isEndOnTheLeft = line.LeftOf(end) > OnTheLine;
                if (isStartOnTheLeft == isEndOnTheLeft) continue;
                crossings.Add(line.CrossingOf(start, end));
            }
            return crossings;
        }

        private static void DropRepeatedLastCorner(List<GroundPoint> corners, List<Boundary> boundaries)
        {
            var last = corners.Count - 1;
            var isRepeated = last > 0 && corners[last].DistanceTo(corners[0]) < MergedMetres;
            if (!isRepeated) return;
            corners.RemoveAt(last);
            boundaries.RemoveAt(last);
        }

        private static void Add(List<GroundPoint> corners, List<Boundary> boundaries, GroundPoint corner, Boundary leaving)
        {
            var last = corners.Count - 1;
            var isRepeated = last >= 0 && corners[last].DistanceTo(corner) < MergedMetres;
            if (isRepeated)
            {
                boundaries[last] = leaving;
                return;
            }
            corners.Add(corner);
            boundaries.Add(leaving);
        }
    }
}
