using System.Collections.Generic;

namespace ShallowWater.Game.Boat
{
    public static class BoatHandling
    {
        public const double TopSpeedMetresPerSecond = 2.2;
        public const double EngineResponsePerSecond = 0.7;
        public const double ReversingPauseSeconds = 1.5;
        public const double TillerSwingPerSecond = 2.2;

        public static readonly IReadOnlyDictionary<ThrottleNotch, double> TurnsAt = new Dictionary<ThrottleNotch, double>
        {
            { ThrottleNotch.FullAstern, -1.0 },
            { ThrottleNotch.HalfAstern, -0.6 },
            { ThrottleNotch.Stop, 0 },
            { ThrottleNotch.DeadSlow, 0.45 },
            { ThrottleNotch.HalfAhead, 0.7 },
            { ThrottleNotch.FullAhead, 1.0 }
        };
    }
}
