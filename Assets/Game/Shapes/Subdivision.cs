using System.Linq;

namespace ShallowWater.Game.Shapes
{
    public static class Subdivision
    {
        private const int CornersOfATriangle = 3;
        private const double Halfway = 0.5;

        public static Shape Finer(Shape shape, double longestEdgeMetres)
        {
            var finer = new Shape();
            for (var index = 0; index < shape.PointCount; index++) finer.Add(shape.Points[index], shape.Places[index], shape.Fittings[index]);
            var corners = shape.Triangles;
            for (var first = 0; first < corners.Count; first += CornersOfATriangle)
            {
                Split(finer, corners[first], corners[first + 1], corners[first + 2], longestEdgeMetres);
            }
            return finer;
        }

        private static void Split(Shape shape, int first, int second, int third, double longestEdgeMetres)
        {
            var longestFirst = LongestEdgeFirst(shape, first, second, third);
            var isFineEnough = EdgeLength(shape, longestFirst[0], longestFirst[1]) <= longestEdgeMetres;
            if (isFineEnough)
            {
                shape.AddTriangle(first, second, third);
                return;
            }
            Halve(shape, longestFirst[0], longestFirst[1], longestFirst[2], longestEdgeMetres);
        }

        private static void Halve(Shape shape, int from, int to, int apex, double longestEdgeMetres)
        {
            var middle = shape.Add(Midpoint(shape, from, to), MidPlace(shape, from, to), shape.Fittings[from]);
            Split(shape, from, middle, apex, longestEdgeMetres);
            Split(shape, middle, to, apex, longestEdgeMetres);
        }

        private static int[] LongestEdgeFirst(Shape shape, int first, int second, int third)
        {
            var turns = new[] { new[] { first, second, third }, new[] { second, third, first }, new[] { third, first, second } };
            return turns.OrderByDescending(turn => EdgeLength(shape, turn[0], turn[1])).First();
        }

        private static double EdgeLength(Shape shape, int from, int to)
        {
            var points = shape.Points;
            return Offset.Between(points[from], points[to]).Length;
        }

        private static WorldPoint Midpoint(Shape shape, int from, int to)
        {
            var points = shape.Points;
            return (Offset.Between(points[from], points[to]) * Halfway).From(points[from]);
        }

        private static SurfacePlace MidPlace(Shape shape, int from, int to)
        {
            var places = shape.Places;
            var start = places[from];
            var end = places[to];
            return new SurfacePlace((start.Along + end.Along) * Halfway, (start.Across + end.Across) * Halfway);
        }
    }
}
