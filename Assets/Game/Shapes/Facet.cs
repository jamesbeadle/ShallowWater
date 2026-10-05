using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public static class Facet
    {
        private const double TowardsTheViewer = 0;

        public static void Add(Shape shape, IReadOnlyList<WorldPoint> corners, WorldPoint seenFrom)
        {
            var places = new SurfacePlace[corners.Count];
            for (var corner = 0; corner < corners.Count; corner++) places[corner] = SurfacePlace.Unmarked;
            Add(shape, corners, places, seenFrom);
        }

        public static void Add(Shape shape, IReadOnlyList<WorldPoint> corners, IReadOnlyList<SurfacePlace> places, WorldPoint seenFrom)
        {
            Add(shape, corners, places, seenFrom, Fitting.None);
        }

        public static void Add(Shape shape, IReadOnlyList<WorldPoint> corners, IReadOnlyList<SurfacePlace> places, WorldPoint seenFrom, Fitting fitting)
        {
            var first = shape.PointCount;
            for (var corner = 0; corner < corners.Count; corner++) shape.Add(corners[corner], places[corner], fitting);
            var isFacingTheViewer = FacingSeenFrom(corners, seenFrom) > TowardsTheViewer;
            for (var corner = 1; corner + 1 < corners.Count; corner++) AddFanTriangle(shape, first, first + corner, isFacingTheViewer);
        }

        private static void AddFanTriangle(Shape shape, int first, int near, bool isFacingTheViewer)
        {
            var far = near + 1;
            if (isFacingTheViewer)
            {
                shape.AddTriangle(first, near, far);
                return;
            }
            shape.AddTriangle(first, far, near);
        }

        private static double FacingSeenFrom(IReadOnlyList<WorldPoint> corners, WorldPoint seenFrom)
        {
            double east = 0, up = 0, north = 0;
            for (int index = 0, previous = corners.Count - 1; index < corners.Count; previous = index++)
            {
                var from = corners[previous];
                var to = corners[index];
                east += (from.Height - to.Height) * (from.North + to.North);
                up += (from.North - to.North) * (from.East + to.East);
                north += (from.East - to.East) * (from.Height + to.Height);
            }
            var corner = corners[0];
            return east * (seenFrom.East - corner.East) + up * (seenFrom.Height - corner.Height) + north * (seenFrom.North - corner.North);
        }
    }
}
