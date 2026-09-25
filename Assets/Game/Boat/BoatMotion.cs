using System;

namespace ShallowWater.Game.Boat
{
    public sealed class BoatMotion
    {
        public WaterPosition Position { get; private set; }
        public double HeadingRadians { get; private set; }
        public double SpeedMetresPerSecond { get; private set; }

        public BoatMotion(WaterPosition position, double headingRadians)
        {
            Position = position;
            HeadingRadians = headingRadians;
        }

        public void Advance(double seconds, double throttle, double rudder)
        {
            SpeedMetresPerSecond = NextSpeed(seconds, throttle);
            HeadingRadians += rudder * TurnRate() * seconds;
            var along = Math.Cos(HeadingRadians) * SpeedMetresPerSecond * seconds;
            var across = Math.Sin(HeadingRadians) * SpeedMetresPerSecond * seconds;
            Position = Position.Moved(along, across);
        }

        public void HoldAt(WaterPosition position)
        {
            Position = position;
            SpeedMetresPerSecond = 0;
        }

        private double NextSpeed(double seconds, double throttle)
        {
            var wantedSpeed = throttle * BoatHandling.TopSpeedMetresPerSecond;
            var pull = (wantedSpeed - SpeedMetresPerSecond) * BoatHandling.ThrottleResponsePerSecond;
            var drag = SpeedMetresPerSecond * BoatHandling.WaterDragPerSecond;
            return SpeedMetresPerSecond + (pull - drag) * seconds;
        }

        private double TurnRate()
        {
            var speedThroughWater = Math.Abs(SpeedMetresPerSecond);
            var hasSteerage = speedThroughWater > BoatHandling.SteerageSpeedMetresPerSecond;
            if (!hasSteerage) return 0;
            var isGoingAstern = SpeedMetresPerSecond < 0;
            if (isGoingAstern) return -BoatHandling.RudderTurnRadiansPerSecond;
            return BoatHandling.RudderTurnRadiansPerSecond;
        }
    }
}
