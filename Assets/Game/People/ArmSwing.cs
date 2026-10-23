using System;

namespace ShallowWater.Game.People
{
    public static class ArmSwing
    {
        private const double WalkSwingRadians = 0.38;
        private const double RunSwingRadians = 0.75;
        private const double ForwardBiasRadians = 0.06;
        private const double WalkOutRadians = 0.1;
        private const double RunOutRadians = 0.22;
        private const double WalkElbowRadians = 0.25;
        private const double RunElbowRadians = 1.35;
        private const double WalkPumpRadians = 0.22;
        private const double RunPumpRadians = 0.3;
        private const double HandLagShare = 0.2;
        private const double Half = 0.5;
        private const double Whole = 1;
        private const double Opposite = -1;
        private const double FullTurnRadians = 2 * Math.PI;
        private const double Level = 0;

        public static FigurePose On(FigurePose pose, double cycleShare, GaitRhythm rhythm)
        {
            var turn = cycleShare * FullTurnRadians;
            var right = Swinging(pose, FigureSide.Right, Math.Cos(turn), Math.Sin(turn), rhythm);
            return Swinging(right, FigureSide.Left, Math.Cos(turn) * Opposite, Math.Sin(turn) * Opposite, rhythm);
        }

        private static FigurePose Swinging(FigurePose pose, FigureSide side, double forward, double trailing, GaitRhythm rhythm)
        {
            var going = rhythm.Going;
            var amplitude = rhythm.Between(WalkSwingRadians, RunSwingRadians);
            var upperArm = new JointAngles((ForwardBiasRadians + amplitude * forward) * going, rhythm.Between(WalkOutRadians, RunOutRadians) * going, Level);
            var pump = rhythm.Between(WalkPumpRadians, RunPumpRadians) * (Whole + forward) * Half;
            var forearm = JointAngles.Swung((rhythm.Between(WalkElbowRadians, RunElbowRadians) + pump) * going);
            var hand = JointAngles.Swung(HandLagShare * amplitude * trailing * going);
            var wrist = FigureJoints.Hands[side];
            var lower = FigureJoints.Parents[wrist];
            var upper = FigureJoints.Parents[lower];
            return pose.With(upper, upperArm).With(lower, forearm).With(wrist, hand);
        }
    }
}
