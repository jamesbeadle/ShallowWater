using System;
using System.Collections.Generic;

namespace ShallowWater.Game.People
{
    public static class TyingPose
    {
        private const double GettingDownShare = 0.25;
        private const double Thigh = 1.3;
        private const double Knee = 2.0;
        private const double Lean = 0.7;
        private const double Arm = 0.3;
        private const double Elbow = 0.35;
        private const double WorkingSwing = 0.22;
        private const double HitchesWhileWorking = 5;
        private const double FullTurnRadians = 2 * Math.PI;
        private const double Upright = 0;
        private const double Whole = 1;
        private const double HeadLift = 0.3;
        private const double SmoothStepRise = 3;
        private const double SmoothStepFall = 2;
        private const double LegLengthMetres = AskewForm.ThighMetres + AskewForm.ShinMetres;

        public static GaitPose At(double share)
        {
            var down = Down(share);
            var lean = Lean * down;
            var thigh = Thigh * down;
            var shin = thigh - Knee * down;
            var work = WorkingSwing * down * Math.Sin(share * HitchesWhileWorking * FullTurnRadians);
            var swings = new Dictionary<FigurePart, double>
            {
                { FigurePart.Body, Upright }, { FigurePart.Head, -HeadLift * down },
                { FigurePart.LeftThigh, thigh + lean }, { FigurePart.RightThigh, thigh + lean },
                { FigurePart.LeftShin, -Knee * down }, { FigurePart.RightShin, -Knee * down },
                { FigurePart.LeftUpperArm, Arm * down + lean + work }, { FigurePart.RightUpperArm, Arm * down + lean - work },
                { FigurePart.LeftForearm, Elbow * down }, { FigurePart.RightForearm, Elbow * down }
            };
            var legHeight = AskewForm.ThighMetres * Math.Cos(thigh) + AskewForm.ShinMetres * Math.Cos(shin);
            return new GaitPose(swings, legHeight - LegLengthMetres, lean);
        }

        private static double Down(double share)
        {
            var gettingDown = Math.Min(Whole, share / GettingDownShare);
            var gettingUp = Math.Min(Whole, (Whole - share) / GettingDownShare);
            var down = Math.Min(gettingDown, gettingUp);
            return down * down * (SmoothStepRise - SmoothStepFall * down);
        }
    }
}
