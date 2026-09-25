using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Ground
{
    public sealed class Area
    {
        private readonly List<GroundRing> holes;

        public Area(GroundRing outline, IEnumerable<GroundRing> holes)
        {
            Outline = outline;
            this.holes = holes.Where(hole => hole.IsDrawable).ToList();
        }

        public GroundRing Outline { get; }
        public bool IsDrawable => Outline.IsDrawable;

        public bool IsAround(GroundPoint point)
        {
            var isInsideTheOutline = Outline.IsAround(point);
            if (!isInsideTheOutline) return false;
            return !holes.Any(hole => hole.IsAround(point));
        }
    }
}
