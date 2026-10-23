using System;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.People
{
    public readonly struct GaitRhythm
    {
        private const double WalkStanceShare = 0.6;
        private const double RunStanceShare = 0.3;
        private const double WalkAnkleAheadMetres = 0.33;
        private const double WalkAnkleBehindMetres = 0.48;
        private const double RunAnkleAheadMetres = 0.35;
        private const double RunAnkleBehindMetres = 0.52;
        private const double WalkStepHalfWidthMetres = 0.06;
        private const double RunStepHalfWidthMetres = 0.05;
        private const double WalkCrouchMetres = 0.012;
        private const double RunCrouchMetres = 0.06;
        private const double Whole = 1;
        private const double Still = 0;

        private GaitRhythm(double speedMetresPerSecond, double cyclesPerSecond)
        {
            Going = Math.Min(Whole, speedMetresPerSecond / WalkingPace.WalkMetresPerSecond);
            var runShare = (speedMetresPerSecond - WalkingPace.WalkMetresPerSecond) / (WalkingPace.RunMetresPerSecond - WalkingPace.WalkMetresPerSecond);
            Run = Math.Clamp(runShare, Still, Whole);
            CyclesPerSecond = cyclesPerSecond;
        }

        public double Going { get; }
        public double Run { get; }
        public double CyclesPerSecond { get; }
        public double StanceShare => Between(WalkStanceShare, RunStanceShare);
        public double AnkleAheadMetres => Between(WalkAnkleAheadMetres * StrideShare, RunAnkleAheadMetres);
        public double AnkleBehindMetres => Between(WalkAnkleBehindMetres * StrideShare, RunAnkleBehindMetres);
        public double StepHalfWidthMetres => Between(WalkStepHalfWidthMetres, RunStepHalfWidthMetres);
        public double CrouchMetres => Between(WalkCrouchMetres * Going, RunCrouchMetres);
        public double StrideMetres => (AnkleAheadMetres + AnkleBehindMetres) / StanceShare;
        public double GroundMovedInTheSwingMetres => StrideMetres * (Whole - StanceShare);
        private double StrideShare => Math.Sqrt(Going);

        public static GaitRhythm At(double speedMetresPerSecond)
        {
            var stride = new GaitRhythm(speedMetresPerSecond, Still);
            var isMoving = speedMetresPerSecond > Still;
            if (!isMoving) return stride;
            return new GaitRhythm(speedMetresPerSecond, speedMetresPerSecond / stride.StrideMetres);
        }

        public double Between(double walking, double running)
        {
            return walking + (running - walking) * Run;
        }
    }
}
