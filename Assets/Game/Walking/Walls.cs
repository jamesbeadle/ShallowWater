using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public static class Walls
    {
        public static bool IsBlocking(GroundRing footprint, GroundPoint place)
        {
            if (footprint.IsAround(place)) return true;
            var corners = footprint.Corners;
            return IsTouching(corners, place);
        }

        private static bool IsTouching(IReadOnlyList<GroundPoint> corners, GroundPoint place)
        {
            for (int corner = 0, previous = corners.Count - 1; corner < corners.Count; previous = corner++)
            {
                var isTouching = place.DistanceToSegment(corners[previous], corners[corner]) < WalkingPace.BodyRadiusMetres;
                if (isTouching) return true;
            }
            return false;
        }
    }
}
