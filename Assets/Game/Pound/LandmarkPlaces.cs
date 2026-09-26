using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public static class LandmarkPlaces
    {
        private static readonly Dictionary<Landmark, GroundPoint> Places = new Dictionary<Landmark, GroundPoint>
        {
            { Landmark.HopwasBridge, new GroundPoint(-50.3, -41.6) },
            { Landmark.FazeleyJunction, new GroundPoint(2426.0, -3098.6) },
        };

        public static GroundPoint Of(Landmark landmark)
        {
            return Places[landmark];
        }
    }
}
