using System.Collections.Generic;

namespace ShallowWater.Game.Ground
{
    public static class CentrelinePiece
    {
        private const double OnTheCentreline = 0;
        private const double VertexClearanceMetres = 0.01;

        public static GroundLine Between(Centreline centreline, Span span)
        {
            var line = centreline.Line;
            var distances = line.DistancesAlong();
            var places = new List<GroundPoint> { centreline.Offset(span.From, OnTheCentreline) };
            for (var vertex = 1; vertex < line.SegmentCount; vertex++)
            {
                var isPastTheStart = distances[vertex] > span.From + VertexClearanceMetres;
                var isShortOfTheEnd = distances[vertex] < span.To - VertexClearanceMetres;
                if (isPastTheStart && isShortOfTheEnd) places.Add(line[vertex]);
            }
            places.Add(centreline.Offset(span.To, OnTheCentreline));
            return new GroundLine(places);
        }
    }
}
