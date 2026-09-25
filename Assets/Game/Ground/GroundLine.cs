using System.Collections.Generic;

namespace ShallowWater.Game.Ground
{
    public sealed class GroundLine
    {
        private const int FewestPointsOfALine = 2;

        private readonly List<GroundPoint> points = new List<GroundPoint>();

        public GroundLine(IEnumerable<GroundPoint> places)
        {
            foreach (var place in places) AddUnlessRepeated(place);
        }

        public int Count => points.Count;
        public int SegmentCount => points.Count - 1;
        public GroundPoint this[int index] => points[index];
        public bool IsDrawable => points.Count >= FewestPointsOfALine;

        public GroundPoint Direction(int segment)
        {
            var run = points[segment + 1] - points[segment];
            return run * (1 / run.Length);
        }

        public double[] DistancesAlong()
        {
            var distances = new double[points.Count];
            for (var index = 1; index < points.Count; index++)
            {
                var step = points[index].DistanceTo(points[index - 1]);
                distances[index] = distances[index - 1] + step;
            }
            return distances;
        }

        public GroundPoint[] Tangents()
        {
            var tangents = new GroundPoint[points.Count];
            for (var index = 0; index < points.Count; index++) tangents[index] = TangentAt(index);
            return tangents;
        }

        private GroundPoint TangentAt(int index)
        {
            var isFirst = index == 0;
            var isLast = index == SegmentCount;
            if (isFirst) return Direction(index);
            if (isLast) return Direction(index - 1);
            var bisector = Direction(index - 1) + Direction(index);
            var isHairpin = bisector.Length < double.Epsilon;
            if (isHairpin) return Direction(index);
            return bisector * (1 / bisector.Length);
        }

        private void AddUnlessRepeated(GroundPoint place)
        {
            var isRepeated = points.Count > 0 && points[points.Count - 1].IsSamePlaceAs(place);
            if (isRepeated) return;
            points.Add(place);
        }
    }
}
