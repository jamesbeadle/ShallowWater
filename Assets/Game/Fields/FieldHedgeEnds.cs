using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class FieldHedgeEnds
    {
        private const double Backwards = -1;

        public static void Close(SurfaceShapes surfaces, GroundLine run, IReadOnlyList<Band> bands)
        {
            var surface = bands[0].Surface;
            var direction = run.Direction(0);
            surfaces.Add(surface, Cap(bands, run.Start, direction, direction * Backwards));
            surfaces.Add(surface, Cap(bands, run.End, direction, direction));
        }

        private static Shape Cap(IReadOnlyList<Band> bands, GroundPoint centre, GroundPoint direction, GroundPoint facing)
        {
            var spread = direction.RightAngleClockwise;
            var corners = new List<WorldPoint> { bands[0].LeftBeside(centre, spread) };
            corners.AddRange(bands.Select(band => band.RightBeside(centre, spread)));
            return FacingPolygon.Towards(corners, facing);
        }
    }
}
