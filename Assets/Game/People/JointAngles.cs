using System;

namespace ShallowWater.Game.People
{
    public readonly struct JointAngles
    {
        public static readonly JointAngles Still = new JointAngles(0, 0, 0);

        public JointAngles(double swingRadians, double reachRadians, double twistRadians)
        {
            SwingRadians = swingRadians;
            ReachRadians = reachRadians;
            TwistRadians = twistRadians;
        }

        public double SwingRadians { get; }
        public double ReachRadians { get; }
        public double TwistRadians { get; }

        public static JointAngles Swung(double swingRadians)
        {
            return new JointAngles(swingRadians, 0, 0);
        }

        public static JointAngles operator +(JointAngles first, JointAngles second)
        {
            return new JointAngles(first.SwingRadians + second.SwingRadians, first.ReachRadians + second.ReachRadians, first.TwistRadians + second.TwistRadians);
        }

        public static JointAngles operator *(JointAngles angles, double share)
        {
            return new JointAngles(angles.SwingRadians * share, angles.ReachRadians * share, angles.TwistRadians * share);
        }

        public JointAngles Blend(JointAngles towards, double share)
        {
            return this * (1 - share) + towards * share;
        }

        public double LargestDifferenceFrom(JointAngles other)
        {
            var swing = Math.Abs(SwingRadians - other.SwingRadians);
            var reach = Math.Abs(ReachRadians - other.ReachRadians);
            var twist = Math.Abs(TwistRadians - other.TwistRadians);
            return Math.Max(swing, Math.Max(reach, twist));
        }
    }
}
