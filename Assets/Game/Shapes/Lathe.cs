using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public static class Lathe
    {
        private const double FullTurnRadians = 2 * Math.PI;

        public static Shape Turned(WorldPoint foot, IReadOnlyList<ProfilePoint> profile, int sides)
        {
            var turned = new Shape();
            foreach (var point in profile) AddRing(turned, foot, point, sides, 0, FullTurnRadians, sides);
            for (var ring = 0; ring + 1 < profile.Count; ring++) JoinRings(turned, ring * sides, sides, sides);
            return turned;
        }

        public static Shape TurnedThrough(WorldPoint foot, IReadOnlyList<ProfilePoint> profile, int sides, double fromRadians, double toRadians)
        {
            var turned = new Shape();
            var pointsPerRing = sides + 1;
            foreach (var point in profile) AddRing(turned, foot, point, sides, fromRadians, toRadians - fromRadians, pointsPerRing);
            for (var ring = 0; ring + 1 < profile.Count; ring++) JoinRings(turned, ring * pointsPerRing, sides, pointsPerRing);
            return turned;
        }

        private static void AddRing(Shape turned, WorldPoint foot, ProfilePoint point, int sides, double fromRadians, double sweepRadians, int pointsPerRing)
        {
            for (var side = 0; side < pointsPerRing; side++)
            {
                var around = fromRadians + sweepRadians * side / sides;
                var radius = point.RadiusMetres;
                var place = new WorldPoint(foot.East + radius * Math.Cos(around), foot.Height + point.HeightMetres, foot.North + radius * Math.Sin(around));
                turned.Add(place, new SurfacePlace(around * radius, point.HeightMetres));
            }
        }

        private static void JoinRings(Shape turned, int firstOfTheRing, int sides, int pointsPerRing)
        {
            for (var side = 0; side < sides; side++)
            {
                var here = firstOfTheRing + side;
                var next = firstOfTheRing + (side + 1) % pointsPerRing;
                turned.AddQuad(here, next, here + pointsPerRing, next + pointsPerRing);
            }
        }
    }
}
