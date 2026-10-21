using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class LineInHand
    {
        public const double HandOutMetres = 0.33;
        private const double HandBelowTheShoulderMetres = 0.5;
        private const double Level = 0;
        private const double Left = -1;
        private const double Right = 1;
        private const double HandReachMetres = AskewForm.ForearmMetres + Hand.PalmBelowTheWristMetres;

        public static bool IsInTheRightHand(Offset studFromTheFeet)
        {
            return studFromTheFeet.Eastward >= Level;
        }

        public static FigurePart ForearmFor(Offset studFromTheFeet)
        {
            return IsInTheRightHand(studFromTheFeet) ? FigurePart.RightForearm : FigurePart.LeftForearm;
        }

        public static GaitPose Holding(GaitPose stride, Offset studFromTheFeet)
        {
            var side = IsInTheRightHand(studFromTheFeet) ? Right : Left;
            var shoulder = new Offset(side * AskewForm.ShoulderHalfWidthMetres, AskewForm.ShoulderMetres, Level);
            var towardsTheStud = new Offset(studFromTheFeet.Eastward, Level, studFromTheFeet.Northward).Normalised;
            var hand = towardsTheStud * HandOutMetres + Offset.Up * -HandBelowTheShoulderMetres;
            var isAtTheStud = double.IsNaN(hand.Eastward);
            if (isAtTheStud) return stride;
            var angles = ArmReach.Towards(hand, AskewForm.UpperArmMetres, HandReachMetres);
            var upperArm = side == Right ? FigurePart.RightUpperArm : FigurePart.LeftUpperArm;
            return stride.Holding(upperArm, ForearmFor(studFromTheFeet), angles);
        }
    }
}
