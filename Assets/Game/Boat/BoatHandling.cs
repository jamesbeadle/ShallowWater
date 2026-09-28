using System.Collections.Generic;

namespace ShallowWater.Game.Boat
{
    public static class BoatHandling
    {
        public const double TopSpeedMetresPerSecond = 3.0;
        public const double DragPerMetre = 0.05;
        public const double DragPerSecond = 0.04;
        public const double EngineResponsePerSecond = 0.7;
        public const double ReversingPauseSeconds = 1.5;
        public const double TillerSwingPerSecond = 2.2;
        public const double RudderBite = 0.035;
        public const double PropWashMetresPerSecond = 1.5;
        public const double SwingDampingPerSecond = 0.9;
        public const double SideDragPerSecond = 1.4;
        public const double PivotAheadMetres = 3.5;

        public static readonly IReadOnlyDictionary<ThrottleNotch, double> SpeedAt = new Dictionary<ThrottleNotch, double>
        {
            { ThrottleNotch.FullAstern, -1.5 },
            { ThrottleNotch.HalfAstern, -0.8 },
            { ThrottleNotch.Stop, 0 },
            { ThrottleNotch.DeadSlow, 1.0 },
            { ThrottleNotch.HalfAhead, 1.9 },
            { ThrottleNotch.FullAhead, TopSpeedMetresPerSecond }
        };

        public static double DragAt(double speedMetresPerSecond)
        {
            return DragPerMetre * speedMetresPerSecond * System.Math.Abs(speedMetresPerSecond) + DragPerSecond * speedMetresPerSecond;
        }

        public static double FullThrust => DragAt(TopSpeedMetresPerSecond);
    }
}
