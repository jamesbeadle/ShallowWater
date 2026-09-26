using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Roads
{
    public static class HedgeEnds
    {
        private const double LeftSide = -1;
        private const double Backwards = -1;
        private const int FirstSegment = 0;

        public static void Close(SurfaceShapes surfaces, GroundLine piece, IReadOnlyList<Band> bands, double side)
        {
            var hedge = bands.Skip(bands.Count - RoadSection.HedgeBandCount).ToList();
            var surface = hedge[FirstSegment].Surface;
            var firstDirection = piece.Direction(FirstSegment);
            var lastDirection = piece.Direction(piece.SegmentCount - 1);
            surfaces.Add(surface, Cap(hedge, side, piece.Start, firstDirection, firstDirection * Backwards));
            surfaces.Add(surface, Cap(hedge, side, piece.End, lastDirection, lastDirection));
        }

        private static Shape Cap(List<Band> hedge, double side, GroundPoint centre, GroundPoint direction, GroundPoint facing)
        {
            var spread = direction.RightAngleClockwise;
            var corners = new List<WorldPoint> { Inner(hedge.First(), side, centre, spread) };
            corners.AddRange(hedge.Select(band => Outer(band, side, centre, spread)));
            return FacingPolygon.Towards(corners, facing);
        }

        private static WorldPoint Inner(Band band, double side, GroundPoint centre, GroundPoint spread)
        {
            return side == LeftSide ? band.RightBeside(centre, spread) : band.LeftBeside(centre, spread);
        }

        private static WorldPoint Outer(Band band, double side, GroundPoint centre, GroundPoint spread)
        {
            return side == LeftSide ? band.LeftBeside(centre, spread) : band.RightBeside(centre, spread);
        }
    }
}
