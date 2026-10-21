using System.Collections.Generic;

namespace ShallowWater.Game.People
{
    public static class AimingPose
    {
        private const double RightArmForward = 1.15;
        private const double RightReachOut = 0.5;
        private const double RightElbow = 1.5;
        private const double LeftArmForward = 1.35;
        private const double LeftReachAcross = -0.35;
        private const double LeftElbow = 0.55;
        private const double CheekToTheStock = 0.18;
        private const double LeanIn = 0.06;
        private const double Upright = 0;
        private const double Still = 0;

        public static GaitPose At(double muzzleDownRadians)
        {
            var lift = -muzzleDownRadians;
            var swings = new Dictionary<FigurePart, double>
            {
                { FigurePart.Body, Upright }, { FigurePart.Head, CheekToTheStock },
                { FigurePart.LeftThigh, Upright }, { FigurePart.RightThigh, Upright },
                { FigurePart.LeftShin, -Stride.StandingKnee }, { FigurePart.RightShin, -Stride.StandingKnee },
                { FigurePart.LeftUpperArm, LeftArmForward + lift }, { FigurePart.RightUpperArm, RightArmForward + lift },
                { FigurePart.LeftForearm, LeftElbow }, { FigurePart.RightForearm, RightElbow }
            };
            var reaches = new Dictionary<FigurePart, double>
            {
                { FigurePart.LeftUpperArm, LeftReachAcross }, { FigurePart.RightUpperArm, RightReachOut }
            };
            return new GaitPose(swings, Still, LeanIn, reaches, Upright, Still);
        }
    }
}
