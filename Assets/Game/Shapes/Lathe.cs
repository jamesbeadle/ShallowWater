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
            foreach (var point in profile) AddRing(turned, foot, point, sides);
            for (var ring = 0; ring + 1 < profile.Count; ring++) JoinRings(turned, ring * sides, sides);
            return turned;
        }

        private static void AddRing(Shape turned, WorldPoint foot, ProfilePoint point, int sides)
        {
            for (var side = 0; side < sides; side++)
            {
                var around = FullTurnRadians * side / sides;
                var radius = point.RadiusMetres;
                var place = new WorldPoint(foot.East + radius * Math.Cos(around), foot.Height + point.HeightMetres, foot.North + radius * Math.Sin(around));
                turned.Add(place, new SurfacePlace(around * radius, point.HeightMetres));
            }
        }

        private static void JoinRings(Shape turned, int firstOfTheRing, int sides)
        {
            for (var side = 0; side < sides; side++)
            {
                var here = firstOfTheRing + side;
                var next = firstOfTheRing + (side + 1) % sides;
                turned.AddQuad(here, next, here + sides, next + sides);
            }
        }
    }
}
