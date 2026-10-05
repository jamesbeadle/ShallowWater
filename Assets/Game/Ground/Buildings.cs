using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Houses;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Ground
{
    public static class Buildings
    {
        public static SurfaceShapes Raised(IEnumerable<Area> footprints)
        {
            var surfaces = new SurfaceShapes();
            foreach (var footprint in footprints)
            {
                var row = Row.Of(footprint.Outline);
                foreach (var run in RowPlan.RunsOn(row)) RunShapes.Raise(surfaces, run);
            }
            return surfaces;
        }

        public static IEnumerable<GroundRing> Footings(IEnumerable<Area> footprints)
        {
            foreach (var footprint in footprints)
            {
                var row = Row.Of(footprint.Outline);
                foreach (var ring in RowPlan.RunsOn(row).SelectMany(RunFootings.Of)) yield return ring;
            }
        }
    }
}
