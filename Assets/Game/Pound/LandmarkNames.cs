using System.Collections.Generic;

namespace ShallowWater.Game.Pound
{
    public static class LandmarkNames
    {
        private static readonly Dictionary<Landmark, string> Names = new Dictionary<Landmark, string>
        {
            { Landmark.HuddlesfordJunction, "Huddlesford Junction" },
            { Landmark.HopwasBridge, "Hopwas bridge" },
            { Landmark.FazeleyJunction, "Fazeley Junction" },
            { Landmark.GlascoteLocks, "Glascote locks" }
        };

        public static string Of(Landmark landmark)
        {
            return Names[landmark];
        }
    }
}
