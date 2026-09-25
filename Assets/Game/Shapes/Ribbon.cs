using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public static class Ribbon
    {
        private const double SharpestMiter = 4;
        private const int PointsPerCrossing = 2;

        public static Shape Along(GroundLine line, Band band)
        {
            var shape = new Shape();
            var tangents = line.Tangents();
            for (var index = 0; index < line.Count; index++)
            {
                var spread = MiterAt(line, tangents, index);
                shape.Add(band.LeftBeside(line[index], spread));
                shape.Add(band.RightBeside(line[index], spread));
            }
            for (var segment = 0; segment < line.SegmentCount; segment++) Join(shape, segment * PointsPerCrossing);
            return shape;
        }

        private static GroundPoint MiterAt(GroundLine line, GroundPoint[] tangents, int index)
        {
            var across = tangents[index].RightAngleClockwise;
            var segment = index < line.SegmentCount ? index : index - 1;
            var segmentAcross = line.Direction(segment).RightAngleClockwise;
            var squareness = across.Dot(segmentAcross);
            var stretch = 1 / Math.Max(squareness, 1 / SharpestMiter);
            return across * stretch;
        }

        private static void Join(Shape shape, int nearLeft)
        {
            var nearRight = nearLeft + 1;
            var farLeft = nearLeft + PointsPerCrossing;
            var farRight = farLeft + 1;
            shape.AddQuad(nearLeft, nearRight, farLeft, farRight);
        }
    }
}
