using System;
using System.Collections.Generic;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.People
{
    public static class Gait
    {
        private const double WalkStrideMetres = 1.45;
        private const double RunStrideMetres = 2.9;
        private const double HalfACycle = Math.PI;

        public static double CyclesPerSecond(double speedMetresPerSecond)
        {
            var runShare = RunShare(speedMetresPerSecond);
            return speedMetresPerSecond / (WalkStrideMetres + (RunStrideMetres - WalkStrideMetres) * runShare);
        }

        public static GaitPose At(double phaseRadians, double speedMetresPerSecond)
        {
            var going = Math.Min(1, speedMetresPerSecond / WalkingPace.WalkMetresPerSecond);
            var run = RunShare(speedMetresPerSecond);
            var swings = new Dictionary<FigurePart, double>
            {
                { FigurePart.Body, 0 }, { FigurePart.Head, 0 },
                { FigurePart.LeftThigh, Stride.Thigh(phaseRadians, run) * going },
                { FigurePart.RightThigh, Stride.Thigh(phaseRadians + HalfACycle, run) * going },
                { FigurePart.LeftShin, Stride.Knee(phaseRadians, run, going) },
                { FigurePart.RightShin, Stride.Knee(phaseRadians + HalfACycle, run, going) },
                { FigurePart.LeftUpperArm, Stride.Arm(phaseRadians + HalfACycle, run) * going },
                { FigurePart.RightUpperArm, Stride.Arm(phaseRadians, run) * going },
                { FigurePart.LeftForearm, Stride.Elbow(run, going) },
                { FigurePart.RightForearm, Stride.Elbow(run, going) }
            };
            return new GaitPose(swings, Stride.Bob(phaseRadians, run) * going, Stride.Lean(run) * going);
        }

        public static GaitPose AtTheTiller()
        {
            var swings = new Dictionary<FigurePart, double>
            {
                { FigurePart.Body, 0 }, { FigurePart.Head, 0 },
                { FigurePart.LeftThigh, 0 }, { FigurePart.RightThigh, 0 },
                { FigurePart.LeftShin, -Stride.StandingKnee }, { FigurePart.RightShin, -Stride.StandingKnee },
                { FigurePart.LeftUpperArm, TillerHold.SlideArm }, { FigurePart.RightUpperArm, TillerHold.SteeringArm },
                { FigurePart.LeftForearm, TillerHold.SlideForearm }, { FigurePart.RightForearm, TillerHold.SteeringForearm }
            };
            return new GaitPose(swings, 0, 0);
        }

        private static double RunShare(double speedMetresPerSecond)
        {
            var share = (speedMetresPerSecond - WalkingPace.WalkMetresPerSecond) / (WalkingPace.RunMetresPerSecond - WalkingPace.WalkMetresPerSecond);
            return Math.Min(Math.Max(share, 0), 1);
        }
    }
}
