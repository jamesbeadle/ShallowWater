using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Ground
{
    public static class Buildings
    {
        private const double HeightMetres = 7;

        public static SurfaceShapes Raised(IEnumerable<Area> footprints)
        {
            var surfaces = new SurfaceShapes();
            var top = Heights.GroundMetres + HeightMetres;
            foreach (var footprint in footprints)
            {
                var outline = footprint.Outline;
                surfaces.Add(Surface.Wall, Extrusion.Walls(outline, Heights.GroundMetres, top));
                surfaces.Add(Surface.Roof, Extrusion.Roof(outline, top));
            }
            return surfaces;
        }
    }
}
