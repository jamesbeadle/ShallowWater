using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public sealed class SurfaceShapes
    {
        private readonly Dictionary<Surface, Shape> shapes = new Dictionary<Surface, Shape>();

        public IReadOnlyDictionary<Surface, Shape> BySurface => shapes;

        public void Add(Surface surface, Shape shape)
        {
            var isNewSurface = !shapes.ContainsKey(surface);
            if (isNewSurface) shapes[surface] = new Shape();
            shapes[surface].Append(shape);
        }

        public void AddRibbon(GroundLine line, Band band)
        {
            Add(band.Surface, Ribbon.Along(line, band));
        }

        public void AddSolid(Surface surface, GroundRing footprint, double bottomMetres, double topMetres)
        {
            Add(surface, Extrusion.Walls(footprint, bottomMetres, topMetres));
            Add(surface, Extrusion.Roof(footprint, topMetres));
        }
    }
}
