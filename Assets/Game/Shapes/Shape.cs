using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public sealed class Shape
    {
        private readonly List<WorldPoint> points = new List<WorldPoint>();
        private readonly List<int> triangles = new List<int>();

        public IReadOnlyList<WorldPoint> Points => points;
        public IReadOnlyList<int> Triangles => triangles;
        public int PointCount => points.Count;

        public int Add(WorldPoint point)
        {
            points.Add(point);
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
            foreach (var corner in other.triangles) triangles.Add(firstNewPoint + corner);
        }
    }
}
