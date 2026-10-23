using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Kneeling
    {
        private const double HipsMetres = 0.5;
        private const double HipsForwardMetres = 0.02;
        private const double ChestOverTheWorkRadians = 0.55;
        private const double HeadDownRadians = 0.1;
        private const double RightAnkleAcrossMetres = 0.1;
        private const double RightAnkleBehindMetres = 0.51;
        private const double FootFlippedBackRadians = Math.PI;
        private const double Level = 0;

        public static readonly FootStep LeftFoot = FootStep.Flat(-0.12, 0.42, 0.1);
        public static readonly FigurePose Body = FigurePose.Upright.WithHips(new Offset(Level, HipsMetres - AskewForm.HipMetres, HipsForwardMetres))
            .With(FigurePart.Chest, JointAngles.Swung(ChestOverTheWorkRadians))
            .With(FigurePart.Head, JointAngles.Swung(HeadDownRadians));

        public static FootStep RightFoot(double down)
        {
            var pitch = FootFlippedBackRadians * down * down;
            var ankle = new WorldPoint(RightAnkleAcrossMetres, AnkleHeightFor(pitch, down), -RightAnkleBehindMetres * down);
            return new FootStep(ankle, pitch, Level);
        }

        private static double AnkleHeightFor(double pitch, double down)
        {
            var soleToeOnTheGround = AskewForm.AnkleAboveTheSoleMetres * Math.Cos(pitch) + AskewForm.ToeAheadOfTheAnkleMetres * Math.Sin(pitch);
            var toeCapOnTheGround = AskewForm.ToeAheadOfTheAnkleMetres * Math.Sin(pitch);
            var shinOnTheGround = AskewForm.KneeRadiusMetres * down;
            return Math.Max(Math.Max(soleToeOnTheGround, toeCapOnTheGround), shinOnTheGround);
        }
    }
}
