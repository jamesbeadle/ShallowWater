using System;

namespace ShallowWater.Game.Ground
{
    public sealed class TurningBearings
    {
        private const double FullTurnRadians = 2 * Math.PI;
        private const double TurningMetres = 10;

        private readonly double[] throughPoints;
        private readonly double[] alongSegments;

        public TurningBearings(GroundLine line)
        {
            throughPoints = Array.ConvertAll(line.Tangents(), tangent => tangent.Bearing);
            alongSegments = new double[line.SegmentCount];
            for (var segment = 0; segment < line.SegmentCount; segment++)
            {
                var direction = line.Direction(segment);
                alongSegments[segment] = direction.Bearing;
            }
        }

        public double At(int segment, double intoSegment, double segmentLength)
        {
            var straight = alongSegments[segment];
            var turning = Math.Min(TurningMetres, segmentLength / 2);
            var beforeTheEnd = segmentLength - intoSegment;
            if (intoSegment < turning) return Turned(throughPoints[segment], straight, intoSegment / turning);
            if (beforeTheEnd < turning) return Turned(throughPoints[segment + 1], straight, beforeTheEnd / turning);
            return straight;
        }

        private static double Turned(double from, double to, double fraction)
        {
            var turn = Math.IEEERemainder(to - from, FullTurnRadians);
            return from + turn * fraction;
        }
    }
}
