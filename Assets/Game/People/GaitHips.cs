using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class GaitHips
    {
        private const double TwistRadians = 0.07;
        private const double RollRadians = 0.04;
        private const double SwayMetres = 0.025;
        private const double KneeKeptSoftMetres = 0.01;
        private const double ReachMetres = AskewForm.LegMetres - KneeKeptSoftMetres;
        private const double FullTurnRadians = 2 * Math.PI;
        private const double Level = 0;
        private static readonly CycleCurve RunBounce = new CycleCurve((0, -0.02), (0.15, -0.06), (0.3, 0), (0.4, 0.04), (0.5, -0.02), (0.65, -0.06), (0.8, 0), (0.9, 0.04));

        public static double TwistAt(double cycleShare, GaitRhythm rhythm)
        {
            return TwistRadians * Math.Cos(cycleShare * FullTurnRadians) * rhythm.StrideShare;
        }

        public static double RollAt(double cycleShare, GaitRhythm rhythm)
        {
            return RollRadians * Math.Sin(cycleShare * FullTurnRadians) * rhythm.StrideShare;
        }

        public static FigurePose At(double cycleShare, GaitRhythm rhythm, double leanRadians, FootStep left, FootStep right)
        {
            var angles = new JointAngles(Level, RollAt(cycleShare, rhythm) + leanRadians, TwistAt(cycleShare, rhythm));
            var sway = -SwayMetres * Math.Sin(cycleShare * FullTurnRadians) * rhythm.StrideShare;
            var onTheGround = Frame.AtTheFeet.Child(new Offset(sway, Level, Level), JointAxes.Signed(FigurePart.Hips, angles));
            var standing = AskewForm.HipMetres - rhythm.CrouchMetres + RunBounce.At(cycleShare) * rhythm.Run;
            var reachable = Math.Min(Reachable(onTheGround, FigurePart.LeftThigh, left), Reachable(onTheGround, FigurePart.RightThigh, right));
            var height = Math.Min(standing, reachable);
            var hips = FigurePose.Upright.With(FigurePart.Hips, angles);
            return hips.WithHips(new Offset(sway, height - AskewForm.HipMetres, Level));
        }

        private static double Reachable(Frame hipsOnTheGround, FigurePart thigh, FootStep step)
        {
            var joint = hipsOnTheGround.Place(FigureJoints.FromTheParent[thigh]);
            var ankle = step.Ankle;
            var across = ankle.East - joint.East;
            var along = ankle.North - joint.North;
            var squaredLevel = ReachMetres * ReachMetres - across * across - along * along;
            return ankle.Height - joint.Height + Math.Sqrt(Math.Max(Level, squaredLevel));
        }
    }
}
