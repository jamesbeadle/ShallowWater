using System;

namespace ShallowWater.Game.People
{
    public static class Turning
    {
        private const double GravityMetresPerSecondSquared = 9.81;
        private const double LeanShare = 0.6;
        private const double LeanAtMostRadians = 0.3;

        public static double LeanRadians(double turnRadiansPerSecond, double speedMetresPerSecond)
        {
            var sideways = speedMetresPerSecond * turnRadiansPerSecond;
            var lean = Math.Atan2(sideways, GravityMetresPerSecondSquared) * LeanShare;
            return Math.Clamp(lean, -LeanAtMostRadians, LeanAtMostRadians);
        }
    }
}
