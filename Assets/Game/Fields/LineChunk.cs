using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class LineChunk
    {
        private const double TightMetres = 0;

        private readonly Bounds bounds;

        public LineChunk(List<GroundPoint> points)
        {
            Points = points;
            bounds = Bounds.Around(points, TightMetres);
        }

        public IReadOnlyList<GroundPoint> Points { get; }

        public bool IsOverlapping(Bounds other)
        {
            return bounds.IsOverlapping(other);
        }
    }
}
