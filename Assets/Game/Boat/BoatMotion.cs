using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Boat
{
    public sealed class BoatMotion
    {
        private const double Rebound = 0.15;
        private const double BankFriction = 0.3;

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
        public double Thrust { get; private set; }
        public GroundPoint Heading => GroundPoint.Facing(Bearing);
        public GroundPoint Starboard => Heading.RightAngleClockwise;
        public double SpeedMetresPerSecond => Velocity.Dot(Heading);
        public BoatWay Way => new BoatWay(SpeedMetresPerSecond, Velocity.Dot(Starboard), SwingRadiansPerSecond);
        public double Rudder => tiller.Rudder;
        public double EngineTurns => engine.Turns;
        public double ThrustShare => Thrust / Propeller.FullThrustNewtons;

        public void Advance(double seconds, ThrottleNotch notch, double wantedRudder, BoatForces outside)
        {
            engine.Run(seconds, notch);
            tiller.Put(seconds, wantedRudder);
            var way = Way;
            Thrust = Propeller.ThrustAt(engine.Turns, way.Ahead);
            Move(seconds, way, ForcesOn(way) + outside);
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
            var impulse = -(1 + Rebound) * closing / (1 + leverage * leverage / BoatMass.SwingRadiusSquared);
            Velocity += awayFromTheBank * impulse;
            SwingRadiansPerSecond -= leverage * impulse / BoatMass.SwingRadiusSquared;
            Scrape(awayFromTheBank.RightAngleClockwise, spin, impulse * BankFriction);
        }

        private void Scrape(GroundPoint alongTheBank, GroundPoint spin, double mostFriction)
        {
            var sliding = (Velocity + spin).Dot(alongTheBank);
            var friction = Math.Clamp(-sliding, -mostFriction, mostFriction);
            Velocity += alongTheBank * friction;
        }

        private BoatForces ForcesOn(BoatWay way)
        {
            var flowAft = Propeller.FlowPastTheRudder(Thrust, way.Ahead);
            var driving = new BoatForces(Thrust - HullDrag.At(way.Ahead), 0, 0);
            var steering = RudderForce.On(tiller.Rudder, flowAft, way);
            return driving + Propeller.Walk(Thrust, way.Ahead) + HullCrossFlow.On(way) + steering;
        }

        private void Move(double seconds, BoatWay way, BoatForces forces)
        {
            var carriedAside = BoatMass.AsideKilograms * way.Aside * way.Swing;
            var carriedAhead = BoatMass.AheadKilograms * way.Ahead * way.Swing;
            var ahead = way.Ahead + (forces.Ahead + carriedAside) / BoatMass.AheadKilograms * seconds;
            var aside = way.Aside + (forces.ToStarboard - carriedAhead) / BoatMass.AsideKilograms * seconds;
            SwingRadiansPerSecond = way.Swing + forces.Swinging / BoatMass.SwingInertia * seconds;
            Bearing += SwingRadiansPerSecond * seconds;
            Velocity = Heading * ahead + Starboard * aside;
            Position += Velocity * seconds;
        }
    }
}
