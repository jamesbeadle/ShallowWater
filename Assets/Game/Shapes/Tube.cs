using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public static class Tube
    {
        private const double FullTurnRadians = 2 * Math.PI;

        public static Shape Along(IReadOnlyList<WorldPoint> path, double radiusMetres, int sides)
        {
            return Laid(path, radiusMetres, sides, false);
        }

        public static Shape Around(IReadOnlyList<WorldPoint> loop, double radiusMetres, int sides)
        {
            return Laid(loop, radiusMetres, sides, true);
        }

        private static Shape Laid(IReadOnlyList<WorldPoint> path, double radiusMetres, int sides, bool isClosed)
        {
            var tube = new Shape();
            var frames = TubeFrames.Along(path, isClosed);
            var along = 0.0;
            for (var index = 0; index < path.Count; index++)
            {
                along += index == 0 ? 0 : Offset.Between(path[index - 1], path[index]).Length;
                AddRing(tube, path[index], frames[index], radiusMetres, sides, along);
            }
            var joins = isClosed ? path.Count : path.Count - 1;
            for (var ring = 0; ring < joins; ring++) JoinRings(tube, ring, (ring + 1) % path.Count, sides);
            return tube;
        }

        private static void AddRing(Shape tube, WorldPoint centre, Offset[] frame, double radiusMetres, int sides, double along)
        {
            for (var side = 0; side < sides; side++)
            {
                var around = FullTurnRadians * side / sides;
                var reach = frame[0] * (Math.Cos(around) * radiusMetres) + frame[1] * (Math.Sin(around) * radiusMetres);
                tube.Add(reach.From(centre), new SurfacePlace(along, around * radiusMetres));
            }
        }

        private static void JoinRings(Shape tube, int ring, int nextRing, int sides)
        {
            for (var side = 0; side < sides; side++)
            {
                var nextSide = (side + 1) % sides;
                tube.AddQuad(ring * sides + nextSide, ring * sides + side, nextRing * sides + nextSide, nextRing * sides + side);
            }
        }
    }
}
