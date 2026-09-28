using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public sealed class Walker
    {
        private const double FullTurnRadians = 2 * Math.PI;

        public Walker(GroundPoint position, double bearing)
        {
            Position = position;
            Bearing = bearing;
        }

        public GroundPoint Position { get; private set; }
        public double Bearing { get; private set; }
        public GroundPoint Velocity { get; private set; }
        public double SpeedMetresPerSecond => Velocity.Length;

        public void Stride(double seconds, GroundPoint wish, bool isRunning, Land land)
        {
            var pace = isRunning ? WalkingPace.RunMetresPerSecond : WalkingPace.WalkMetresPerSecond;
            var takeUp = Math.Min(1, WalkingPace.TakeUpPerSecond * seconds);
            Velocity += (wish * pace - Velocity) * takeUp;
            var stepped = land.Stepped(Position, Velocity * seconds);
            Velocity = (stepped - Position) * (1 / seconds);
            Position = stepped;
            var isMoving = SpeedMetresPerSecond > WalkingPace.StandingStillMetresPerSecond;
            if (isMoving) Turn(seconds, Velocity.Bearing);
        }

        public void StandAt(GroundPoint place, double bearing)
        {
            Position = place;
            Bearing = bearing;
            Velocity = new GroundPoint(0, 0);
        }

        private void Turn(double seconds, double towards)
        {
            var remaining = Math.IEEERemainder(towards - Bearing, FullTurnRadians);
            Bearing += remaining * Math.Min(1, WalkingPace.TurnPerSecond * seconds);
        }
    }
}
