using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public readonly struct Frame
    {
        private const double Folded = -1;
        private const double Straight = 1;
        private const double HardlyTurned = 0.000001;
        private const double Unturned = 0;
        public static readonly Frame AtTheFeet = new Frame(new WorldPoint(0, 0, 0), Offset.East, Offset.Up, Offset.North);

        public Frame(WorldPoint origin, Offset right, Offset up, Offset forward)
        {
            Origin = origin;
            Right = right;
            Up = up;
            Forward = forward;
        }

        public WorldPoint Origin { get; }
        public Offset Right { get; }
        public Offset Up { get; }
        public Offset Forward { get; }

        public WorldPoint Place(Offset local)
        {
            return Turned(local).From(Origin);
        }

        public Offset Turned(Offset local)
        {
            return Right * local.Eastward + Up * local.Upward + Forward * local.Northward;
        }

        public Offset Local(Offset world)
        {
            return new Offset(world.Dot(Right), world.Dot(Up), world.Dot(Forward));
        }

        public Frame Child(Offset restOffset, JointAngles signed)
        {
            var origin = Place(restOffset);
            return new Frame(origin, Turned(Spun(Offset.East, signed)), Turned(Spun(Offset.Up, signed)), Turned(Spun(Offset.North, signed)));
        }

        public JointAngles SignedAnglesOf(Frame wanted)
        {
            var right = Local(wanted.Right);
            var up = Local(wanted.Up);
            var forward = Local(wanted.Forward);
            var swing = Math.Asin(Math.Clamp(up.Northward, Folded, Straight));
            var isLevel = Math.Abs(Math.Cos(swing)) < HardlyTurned;
            var levelReach = Math.Atan2(forward.Eastward * up.Northward, -forward.Upward * up.Northward);
            if (isLevel) return new JointAngles(swing, levelReach, Unturned);
            var reach = Math.Atan2(-up.Eastward, up.Upward);
            var twist = Math.Atan2(-right.Northward, forward.Northward);
            return new JointAngles(swing, reach, twist);
        }

        private static Offset Spun(Offset axis, JointAngles signed)
        {
            var twisted = axis.TurnedAbout(Offset.Up, signed.TwistRadians);
            var swung = twisted.TurnedAbout(Offset.East, signed.SwingRadians);
            return swung.TurnedAbout(Offset.North, signed.ReachRadians);
        }
    }
}
