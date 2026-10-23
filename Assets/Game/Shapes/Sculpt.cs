using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public static class Sculpt
    {
        public static Shape Rings(WorldPoint foot, IReadOnlyList<double> heightsMetres, Turn turn, Func<int, double, double> radiusAt)
        {
            var shape = new Shape();
            for (var ring = 0; ring < heightsMetres.Count; ring++) AddRing(shape, foot, heightsMetres[ring], ring, turn, radiusAt);
            for (var ring = 0; ring + 1 < heightsMetres.Count; ring++) JoinRings(shape, ring * turn.PointsPerRing, turn);
            return shape;
        }

        private static void AddRing(Shape shape, WorldPoint foot, double heightMetres, int ring, Turn turn, Func<int, double, double> radiusAt)
        {
            for (var side = 0; side < turn.PointsPerRing; side++)
            {
                var around = turn.AngleAt(side);
                var radius = radiusAt(ring, around);
                var place = new WorldPoint(foot.East + radius * Math.Cos(around), foot.Height + heightMetres, foot.North + radius * Math.Sin(around));
                shape.Add(place, new SurfacePlace(around * radius, heightMetres));
            }
        }

        private static void JoinRings(Shape shape, int firstOfTheRing, Turn turn)
        {
            for (var side = 0; side < turn.Sides; side++)
            {
                var here = firstOfTheRing + side;
                var next = firstOfTheRing + (side + 1) % turn.PointsPerRing;
                shape.AddQuad(here, next, here + turn.PointsPerRing, next + turn.PointsPerRing);
            }
        }
    }
}
