using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public sealed class SurfaceShapes
    {
        private readonly Dictionary<Surface, Shape> shapes = new Dictionary<Surface, Shape>();

        public IReadOnlyDictionary<Surface, Shape> BySurface => shapes;

        public void AddRibbon(GroundLine line, Band band)
        {
            var isNewSurface = !shapes.ContainsKey(band.Surface);
            if (isNewSurface) shapes[band.Surface] = new Shape();
            shapes[band.Surface].Append(Ribbon.Along(line, band));
        }
    }
}
