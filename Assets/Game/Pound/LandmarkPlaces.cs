using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public static class LandmarkPlaces
    {
        private static readonly Dictionary<Landmark, GroundPoint> Places = new Dictionary<Landmark, GroundPoint>
        {
            { Landmark.HopwasBridge, GroundPoint.HopwasBridge },
            { Landmark.FazeleyJunction, new GroundPoint(2443.7, -3059.4) },
        };

        public static GroundPoint Of(Landmark landmark)
        {
            return Places[landmark];
        }
    }
}
