using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public sealed class Shape
    {
        private readonly List<WorldPoint> points = new List<WorldPoint>();
        private readonly List<SurfacePlace> places = new List<SurfacePlace>();
        private readonly List<int> triangles = new List<int>();

        public IReadOnlyList<WorldPoint> Points => points;
        public IReadOnlyList<SurfacePlace> Places => places;
        public IReadOnlyList<int> Triangles => triangles;
        public int PointCount => points.Count;

        public int Add(WorldPoint point)
        {
            return Add(point, SurfacePlace.Unmarked);
        }

        public int Add(WorldPoint point, SurfacePlace place)
        {
            points.Add(point);
            places.Add(place);
            return points.Count - 1;
        }

        public void AddTriangle(int first, int second, int third)
        {
            triangles.Add(first);
            triangles.Add(second);
            triangles.Add(third);
        }

        public void AddQuad(int nearLeft, int nearRight, int farLeft, int farRight)
        {
            AddTriangle(nearLeft, farLeft, nearRight);
            AddTriangle(nearRight, farLeft, farRight);
        }

        public void Append(Shape other)
        {
            var firstNewPoint = points.Count;
            points.AddRange(other.points);
            places.AddRange(other.places);
            foreach (var corner in other.triangles) triangles.Add(firstNewPoint + corner);
        }
    }
}
