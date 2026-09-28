using System.Collections.Generic;
using ShallowWater.Game.Cottages;
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
                var cottage = new Cottage(footprint.Outline);
                CottageWalls.Raise(surfaces, cottage);
                CottageRoof.Lay(surfaces, cottage);
                CottageChimney.Build(surfaces, cottage);
            }
            return surfaces;
        }
    }
}
