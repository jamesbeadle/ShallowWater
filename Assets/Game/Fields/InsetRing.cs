using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public readonly struct InsetRing
    {
        private readonly IReadOnlyList<GroundPoint> outer;
        private readonly IReadOnlyList<GroundPoint> inner;

        public InsetRing(IReadOnlyList<GroundPoint> outer, IReadOnlyList<GroundPoint> inner)
        {
            this.outer = outer;
            this.inner = inner;
        }

        public GroundPoint[] CornersAlong(int edge)
        {
            var next = (edge + 1) % outer.Count;
            return new[] { outer[edge], outer[next], inner[next], inner[edge] };
        }
    }
}
