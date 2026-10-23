using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Stance
    {
        private const double FootHalfWidthMetres = 0.11;
        private const double ToeOutRadians = 0.14;
        private const double LeadingFootAheadMetres = 0.04;
        private const double LeadingToeOutRadians = 0.2;
        private const double SoftKneesMetres = 0.003;
        private const double WeightOnTheRightMetres = 0.02;
        private const double FreeHipDropRadians = 0.04;
        private const double ShouldersTiltRadians = 0.03;
        private const double ShiftSeconds = 7.3;
        private const double ShiftMetres = 0.02;
        private const double ShiftRollRadians = 0.03;
        private const double BreathSeconds = 3.7;
        private const double BreathRadians = 0.012;
        private const double LookSeconds = 9.7;
        private const double LookAgainSeconds = 23.1;
        private const double LookRadians = 0.3;
        private const double SlouchRadians = 0.05;
        private const double HeadForwardRadians = 0.06;
        private const double ArmsBackRadians = -0.05;
        private const double ArmsOutRadians = 0.12;
        private const double ElbowRadians = 0.18;
        private const double FullTurnRadians = 2 * Math.PI;
        private const double Level = 0;

        public static readonly FootStep LeftFoot = FootStep.Flat(-FootHalfWidthMetres, LeadingFootAheadMetres, LeadingToeOutRadians);
        public static readonly FootStep RightFoot = FootStep.Flat(FootHalfWidthMetres, Level, ToeOutRadians);

        public static FigurePose At(double seconds)
        {
            return Skirts.Following(Legs.Planted(Body(seconds), LeftFoot, RightFoot));
        }

        public static FigurePose Body(double seconds)
        {
            var shift = Math.Sin(seconds / ShiftSeconds * FullTurnRadians);
            var breath = Math.Sin(seconds / BreathSeconds * FullTurnRadians);
            var look = LookRadians * Math.Sin(seconds / LookSeconds * FullTurnRadians) * Math.Sin(seconds / LookAgainSeconds * FullTurnRadians);
            var hipsRoll = -FreeHipDropRadians - ShiftRollRadians * shift;
            var hips = new JointAngles(Level, hipsRoll, Level);
            var chest = new JointAngles(SlouchRadians + BreathRadians * breath, ShouldersTiltRadians - hipsRoll, Level);
            var head = new JointAngles(HeadForwardRadians - BreathRadians * breath, -ShouldersTiltRadians, look);
            var standing = FigurePose.Upright.WithHips(new Offset(WeightOnTheRightMetres + ShiftMetres * shift, -SoftKneesMetres, Level));
            var torso = standing.With(FigurePart.Hips, hips).With(FigurePart.Chest, chest).With(FigurePart.Head, head);
            return Hanging(Hanging(torso, FigureSide.Left), FigureSide.Right);
        }

        private static FigurePose Hanging(FigurePose pose, FigureSide side)
        {
            var hand = FigureJoints.Hands[side];
            var forearm = FigureJoints.Parents[hand];
            var upperArm = FigureJoints.Parents[forearm];
            return pose.With(upperArm, new JointAngles(ArmsBackRadians, ArmsOutRadians, Level)).With(forearm, JointAngles.Swung(ElbowRadians));
        }
    }
}
