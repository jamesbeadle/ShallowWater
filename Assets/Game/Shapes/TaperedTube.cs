using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public static class TaperedTube
    {
        private const double FullTurnRadians = 2 * Math.PI;

        public static Shape Along(IReadOnlyList<WorldPoint> path, IReadOnlyList<double> radiiMetres, int sides)
        {
            var tube = new Shape();
            var frames = TubeFrames.Along(path, false);
            var metresAlong = 0.0;
            for (var ring = 0; ring < path.Count; ring++)
            {
                metresAlong += ring == 0 ? 0 : Offset.Between(path[ring - 1], path[ring]).Length;
                Encircle(tube, path[ring], frames[ring], radiiMetres[ring], sides, metresAlong);
            }
            var pointsPerRing = sides + 1;
            for (var ring = 0; ring + 1 < path.Count; ring++) Stitch(tube, ring * pointsPerRing, pointsPerRing);
            return tube;
        }

        private static void Encircle(Shape tube, WorldPoint centre, Offset[] frame, double radiusMetres, int sides, double metresAlong)
        {
            for (var side = 0; side <= sides; side++)
            {
                var turn = FullTurnRadians * side / sides;
                var reach = frame[0] * (Math.Cos(turn) * radiusMetres) + frame[1] * (Math.Sin(turn) * radiusMetres);
                tube.Add(reach.From(centre), new SurfacePlace(metresAlong, turn));
            }
        }

        private static void Stitch(Shape tube, int firstOfTheRing, int pointsPerRing)
        {
            for (var side = 0; side + 1 < pointsPerRing; side++)
            {
                var here = firstOfTheRing + side;
                var above = here + pointsPerRing;
                tube.AddQuad(here + 1, here, above + 1, above);
            }
        }
    }
}
