using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Scaled
    {
        public static Shape Of(Shape shape, Offset scale)
        {
            var scaled = new Shape();
            var points = shape.Points;
            var places = shape.Places;
            for (var index = 0; index < shape.PointCount; index++)
            {
                var point = points[index];
                var moved = new WorldPoint(point.East * scale.Eastward, point.Height * scale.Upward, point.North * scale.Northward);
                scaled.Add(moved, places[index]);
            }
            var corners = shape.Triangles;
            for (var corner = 0; corner + 2 < corners.Count; corner += 3) scaled.AddTriangle(corners[corner], corners[corner + 1], corners[corner + 2]);
            return scaled;
        }

        public static Shape Moved(Shape shape, Offset by)
        {
            var moved = new Shape();
            var points = shape.Points;
            var places = shape.Places;
            for (var index = 0; index < shape.PointCount; index++) moved.Add(by.From(points[index]), places[index]);
            var corners = shape.Triangles;
            for (var corner = 0; corner + 2 < corners.Count; corner += 3) moved.AddTriangle(corners[corner], corners[corner + 1], corners[corner + 2]);
            return moved;
        }
    }
}
