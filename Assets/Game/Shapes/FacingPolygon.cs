using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public static class FacingPolygon
    {
        private const int FirstCorner = 0;
        private const double NoArea = 0;

        public static Shape Towards(IReadOnlyList<WorldPoint> corners, GroundPoint facing)
        {
            var isAnticlockwise = AreaSeenFrom(corners, facing) > NoArea;
            var ordered = isAnticlockwise ? corners.Reverse().ToList() : corners.ToList();
            var polygon = new Shape();
            foreach (var corner in ordered) polygon.Add(corner);
            for (var corner = 1; corner < ordered.Count - 1; corner++) polygon.AddTriangle(FirstCorner, corner, corner + 1);
            return polygon;
        }

        private static double AreaSeenFrom(IReadOnlyList<WorldPoint> corners, GroundPoint facing)
        {
            var area = 0.0;
            for (int index = 0, previous = corners.Count - 1; index < corners.Count; previous = index++)
            {
                var from = corners[previous];
                var to = corners[index];
                area += Across(from, facing) * to.Height - Across(to, facing) * from.Height;
            }
            return area;
        }

        private static double Across(WorldPoint point, GroundPoint facing)
        {
            return facing.East * point.North - facing.North * point.East;
        }
    }
}
