using System.Collections.Generic;

namespace ShallowWater.Game.Ground
{
    public static class CentrelinePiece
    {
        private const double OnTheCentreline = 0;
        private const double VertexClearanceMetres = 0.01;
        private const int FirstInteriorVertex = 1;
        private const int Halves = 2;

        public static GroundLine Between(Centreline centreline, Span span)
        {
            var line = centreline.Line;
            var distances = centreline.Distances;
            var places = new List<GroundPoint> { centreline.Offset(span.From, OnTheCentreline) };
            var lastInteriorVertex = line.SegmentCount - 1;
            for (var vertex = FirstVertexAfter(distances, span.From + VertexClearanceMetres); vertex <= lastInteriorVertex; vertex++)
            {
                var isShortOfTheEnd = distances[vertex] < span.To - VertexClearanceMetres;
                if (!isShortOfTheEnd) break;
                places.Add(line[vertex]);
            }
            places.Add(centreline.Offset(span.To, OnTheCentreline));
            return new GroundLine(places);
        }

        private static int FirstVertexAfter(IReadOnlyList<double> distances, double along)
        {
            var low = FirstInteriorVertex;
            var high = distances.Count - 1;
            while (low < high)
            {
                var middle = (low + high) / Halves;
                var isPast = distances[middle] > along;
                high = isPast ? middle : high;
                low = isPast ? low : middle + 1;
            }
            return low;
        }
    }
}
