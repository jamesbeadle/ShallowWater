using System.Collections.Generic;
using ShallowWater.Game.Boat;

namespace ShallowWater.Unity.Helm
{
    public static class NotchNames
    {
        private static readonly Dictionary<ThrottleNotch, string> Names = new Dictionary<ThrottleNotch, string>
        {
            { ThrottleNotch.FullAstern, "Full astern" },
            { ThrottleNotch.HalfAstern, "Half astern" },
            { ThrottleNotch.Stop, "Stop" },
            { ThrottleNotch.DeadSlow, "Dead slow ahead" },
            { ThrottleNotch.HalfAhead, "Half ahead" },
            { ThrottleNotch.FullAhead, "Full ahead" }
        };

        public static string Of(ThrottleNotch notch)
        {
            return Names[notch];
        }
    }
}
