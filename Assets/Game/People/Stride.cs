using System;

namespace ShallowWater.Game.People
{
    public static class Stride
    {
        public const double StandingKnee = 0.05;
        public const double RestingElbow = 0.3;
        private const double WalkThigh = 0.42;
        private const double RunThigh = 0.8;
        private const double WalkKnee = 0.65;
        private const double RunKnee = 1.5;
        private const double WalkArm = 0.3;
        private const double RunArm = 0.75;
        private const double WalkElbow = 0.25;
        private const double RunElbow = 1.4;
        private const double WalkBobMetres = 0.025;
        private const double RunBobMetres = 0.06;
        private const double WalkLean = 0.03;
        private const double RunLean = 0.2;
        private const double BendSharpness = 1.5;

        public static double Thigh(double phase, double run)
        {
            return Between(WalkThigh, RunThigh, run) * Math.Sin(phase);
        }

        public static double Knee(double phase, double run, double going)
        {
            var swinging = Math.Pow(Math.Max(0, Math.Cos(phase)), BendSharpness);
            return -(StandingKnee + Between(WalkKnee, RunKnee, run) * swinging * going);
        }

        public static double Arm(double phase, double run)
        {
            return Between(WalkArm, RunArm, run) * Math.Sin(phase);
        }

        public static double Elbow(double run, double going)
        {
            return RestingElbow + (Between(WalkElbow, RunElbow, run) - RestingElbow) * going;
        }

        public static double Bob(double phase, double run)
        {
            return Between(WalkBobMetres, RunBobMetres, run) * Math.Cos(2 * phase);
        }

        public static double Lean(double run)
        {
            return Between(WalkLean, RunLean, run);
        }

        private static double Between(double walking, double running, double run)
        {
            return walking + (running - walking) * run;
        }
    }
}
