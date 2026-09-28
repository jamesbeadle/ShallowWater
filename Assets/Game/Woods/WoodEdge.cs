using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public sealed class WoodEdge
    {
        private readonly IReadOnlyList<GroundPoint> corners;

        public WoodEdge(GroundRing outline)
        {
            corners = outline.Corners;
        }

        public double DistanceFrom(GroundPoint place)
        {
            var nearest = double.MaxValue;
            for (int corner = 0, previous = corners.Count - 1; corner < corners.Count; previous = corner++)
            {
                var distance = place.DistanceToSegment(corners[previous], corners[corner]);
                if (distance < nearest) nearest = distance;
            }
            return nearest;
        }
    }
}
