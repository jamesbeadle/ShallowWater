using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public static class InsetOutline
    {
        private const double NearlyParallel = 0.05;

        public static List<GroundPoint> Corners(ConvexOutline outline, IReadOnlyList<double> distances)
        {
            var count = outline.EdgeCount;
            return Enumerable.Range(0, count).Select(corner => CornerAt(outline, distances, corner, (corner + count - 1) % count)).ToList();
        }

        public static bool IsFaithful(ConvexOutline outline, IReadOnlyList<GroundPoint> inset)
        {
            for (var edge = 0; edge < inset.Count; edge++)
            {
                var run = inset[(edge + 1) % inset.Count] - inset[edge];
                var isReversed = run.Dot(outline.EdgeDirection(edge)) <= 0;
                if (isReversed) return false;
            }
            return true;
        }

        private static GroundPoint CornerAt(ConvexOutline outline, IReadOnlyList<double> distances, int edge, int previous)
        {
            var direction = outline.EdgeDirection(edge);
            var previousDirection = outline.EdgeDirection(previous);
            var start = outline.EdgeStart(edge) + direction.RightAngleClockwise * distances[edge];
            var previousStart = outline.EdgeStart(previous) + previousDirection.RightAngleClockwise * distances[previous];
            var turn = previousDirection.Cross(direction);
            if (Math.Abs(turn) < NearlyParallel) return start;
            var reach = (start - previousStart).Cross(direction) / turn;
            return previousStart + previousDirection * reach;
        }
    }
}
