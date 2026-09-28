using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Boat
{
    public sealed class BoatMotion
    {
        private const double Rebound = 0.15;
        private const double BankFriction = 0.3;
        private const double TwelfthOfTheLengthSquared = BoatSize.LengthMetres * BoatSize.LengthMetres / 12;

        private readonly Engine engine = new Engine();
        private readonly Tiller tiller = new Tiller();

        public BoatMotion(GroundPoint position, double bearing)
        {
            Position = position;
            Bearing = bearing;
        }

        public GroundPoint Position { get; private set; }
        public double Bearing { get; private set; }
        public GroundPoint Velocity { get; private set; }
        public double SwingRadiansPerSecond { get; private set; }
        public GroundPoint Heading => GroundPoint.Facing(Bearing);
        public GroundPoint Starboard => Heading.RightAngleClockwise;
        public double SpeedMetresPerSecond => Velocity.Dot(Heading);
        public double Rudder => tiller.Rudder;
        public double ThrustShare => engine.ThrustShare;

        public void Advance(double seconds, ThrottleNotch notch, double wantedRudder)
        {
            engine.Run(seconds, notch);
            tiller.Put(seconds, wantedRudder);
            Swing(seconds);
            Drive(seconds);
            var pivotSwing = Starboard * (-BoatHandling.PivotAheadMetres * SwingRadiansPerSecond * seconds);
            Position = Position + Velocity * seconds + pivotSwing;
        }

        public GroundPoint PointOf(HullPoint point)
        {
            return Position + Heading * point.Ahead + Starboard * point.ToStarboard;
        }

        public void Shift(GroundPoint by)
        {
            Position += by;
        }

        public void Strike(GroundPoint offset, GroundPoint awayFromTheBank)
        {
            var spin = new GroundPoint(offset.North, -offset.East) * SwingRadiansPerSecond;
            var closing = (Velocity + spin).Dot(awayFromTheBank);
            if (closing >= 0) return;
            var leverage = offset.Cross(awayFromTheBank);
            var impulse = -(1 + Rebound) * closing / (1 + leverage * leverage / TwelfthOfTheLengthSquared);
            Velocity += awayFromTheBank * impulse;
            SwingRadiansPerSecond -= leverage * impulse / TwelfthOfTheLengthSquared;
            Scrape(awayFromTheBank.RightAngleClockwise, spin, impulse * BankFriction);
        }

        private void Scrape(GroundPoint alongTheBank, GroundPoint spin, double mostFriction)
        {
            var sliding = (Velocity + spin).Dot(alongTheBank);
            var friction = Math.Clamp(-sliding, -mostFriction, mostFriction);
            Velocity += alongTheBank * friction;
        }

        private void Swing(double seconds)
        {
            var flow = SpeedMetresPerSecond + Math.Max(engine.ThrustShare, 0) * BoatHandling.PropWashMetresPerSecond;
            var turning = BoatHandling.RudderBite * tiller.Rudder * flow - BoatHandling.SwingDampingPerSecond * SwingRadiansPerSecond;
            SwingRadiansPerSecond += turning * seconds;
            Bearing += SwingRadiansPerSecond * seconds;
        }

        private void Drive(double seconds)
        {
            var ahead = SpeedMetresPerSecond;
            var aside = Velocity.Dot(Starboard);
            ahead += (engine.Thrust - BoatHandling.DragAt(ahead)) * seconds;
            aside -= aside * Math.Min(1, BoatHandling.SideDragPerSecond * seconds);
            Velocity = Heading * ahead + Starboard * aside;
        }
    }
}
