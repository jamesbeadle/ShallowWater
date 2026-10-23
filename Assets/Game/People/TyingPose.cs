using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class TyingPose
    {
        public const double Finished = 1;
        private const double GettingDownShare = 0.25;
        private const double HitchesWhileWorking = 5;
        private const double HandCircleMetres = 0.05;
        private const double HandsApartMetres = 0.07;
        private const double HandsAheadMetres = 0.45;
        private const double FullTurnRadians = 2 * Math.PI;
        private const double Whole = 1;
        private const double Opposite = -1;
        private const double Level = 0;
        private static readonly Offset RightElbowOut = new Offset(0.6, -1, 0);
        private static readonly Offset LeftElbowOut = new Offset(-0.6, -1, 0);

        public static FigurePose At(double share, double tieAboveTheFeetMetres)
        {
            var down = Down(share);
            var body = Stance.Body(Level).Blend(Kneeling.Body, down);
            var left = Stance.LeftFoot.Blend(Kneeling.LeftFoot, down);
            var kneeling = Legs.Planted(body, left, Kneeling.RightFoot(down));
            var turn = share * HitchesWhileWorking * FullTurnRadians;
            var circling = new Offset(Math.Cos(turn), Level, Math.Sin(turn)) * (HandCircleMetres * down);
            var rightOnThePost = circling.From(new WorldPoint(HandsApartMetres, tieAboveTheFeetMetres, HandsAheadMetres));
            var leftOnThePost = (circling * Opposite).From(new WorldPoint(-HandsApartMetres, tieAboveTheFeetMetres, HandsAheadMetres));
            var right = Arms.Reaching(kneeling, FigurePart.RightHand, Reaching(kneeling, FigurePart.RightHand, rightOnThePost, down), RightElbowOut);
            var both = Arms.Reaching(right, FigurePart.LeftHand, Reaching(kneeling, FigurePart.LeftHand, leftOnThePost, down), LeftElbowOut);
            return Skirts.Following(both);
        }

        private static WorldPoint Reaching(FigurePose body, FigurePart hand, WorldPoint onThePost, double down)
        {
            var hanging = Arms.PalmOf(body, hand);
            var towardsThePost = Offset.Between(hanging, onThePost) * down;
            return towardsThePost.From(hanging);
        }

        private static double Down(double share)
        {
            var gettingDown = Math.Min(Whole, share / GettingDownShare);
            var gettingUp = Math.Min(Whole, (Whole - share) / GettingDownShare);
            return Smoothing.Step(Math.Min(gettingDown, gettingUp));
        }
    }
}
