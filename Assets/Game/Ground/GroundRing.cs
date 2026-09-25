using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Ground
{
    public sealed class GroundRing
    {
        private const int FewestCornersOfARing = 3;

        private readonly List<GroundPoint> corners;

        public GroundRing(IEnumerable<GroundPoint> places)
        {
            var outline = new GroundLine(places);
            corners = outline.Points.ToList();
            var isCounterClockwise = TwiceSignedArea() > 0;
            if (isCounterClockwise) corners.Reverse();
            West = corners.Min(corner => corner.East);
            East = corners.Max(corner => corner.East);
            South = corners.Min(corner => corner.North);
            North = corners.Max(corner => corner.North);
        }

        public IReadOnlyList<GroundPoint> Corners => corners;
        public bool IsDrawable => corners.Count >= FewestCornersOfARing;
        public double West { get; }
        public double East { get; }
        public double South { get; }
        public double North { get; }

        public bool IsAround(GroundPoint point)
        {
            var isInside = false;
            for (int index = 0, previous = corners.Count - 1; index < corners.Count; previous = index++)
            {
                var isCrossed = IsCrossedEastOf(point, corners[previous], corners[index]);
                if (isCrossed) isInside = !isInside;
            }
            return isInside;
        }

        private static bool IsCrossedEastOf(GroundPoint point, GroundPoint from, GroundPoint to)
        {
            var isStraddling = (from.North > point.North) != (to.North > point.North);
            if (!isStraddling) return false;
            var crossingEast = from.East + (point.North - from.North) / (to.North - from.North) * (to.East - from.East);
            return point.East < crossingEast;
        }

        private double TwiceSignedArea()
        {
            var sum = 0.0;
            for (int index = 0, previous = corners.Count - 1; index < corners.Count; previous = index++)
            {
                sum += corners[previous].East * corners[index].North - corners[index].East * corners[previous].North;
            }
            return sum;
        }
    }
}
