using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public static class CanalArms
    {
        public static GroundLine StartingNearest(IEnumerable<GroundLine> canals, GroundPoint place)
        {
            return canals.OrderBy(canal => place.DistanceTo(canal.Start)).First();
        }
    }
}
