using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FigureHips
    {
        private const double BackOfTheWaist = Math.PI;
        private const double RoundTheBack = Math.PI;
        private const double BeltRadiusMetres = 0.198;
        private const double BeltButtonRadiusMetres = 0.012;
        private const double BeltButtonsHalfWidthMetres = 0.1;
        private static readonly WorldPoint Hips = new WorldPoint(0, 0, 0);
        private static readonly Offset BodyShape = new Offset(1, 1, AskewForm.BodyDepthShare);

        private static readonly ProfilePoint[] SeatProfile =
        {
            new ProfilePoint(0.15, -0.12), new ProfilePoint(0.165, 0), new ProfilePoint(0.17, 0.1), new ProfilePoint(0.165, AskewForm.WaistAboveTheHipsMetres)
        };

        private static readonly ProfilePoint[] HalfBelt =
        {
            new ProfilePoint(BeltRadiusMetres, AskewForm.WaistAboveTheHipsMetres - 0.045), new ProfilePoint(BeltRadiusMetres, AskewForm.WaistAboveTheHipsMetres - 0.005)
        };

        public static SurfaceShapes Seat(Outfit outfit)
        {
            var hips = new SurfaceShapes();
            hips.Add(outfit.Trousers, Scaled.Of(Lathe.Turned(Hips, SeatProfile, AskewForm.RoundSides), BodyShape));
            if (!outfit.IsDoubleBreasted) return hips;
            hips.Add(outfit.Coat, Scaled.Of(Lathe.TurnedThrough(Hips, HalfBelt, AskewForm.RoundSides, BackOfTheWaist, BackOfTheWaist + RoundTheBack), BodyShape));
            BeltButtons(hips);
            return hips;
        }

        private static void BeltButtons(SurfaceShapes hips)
        {
            var height = AskewForm.WaistAboveTheHipsMetres - 0.025;
            var back = -Math.Sqrt(BeltRadiusMetres * BeltRadiusMetres - BeltButtonsHalfWidthMetres * BeltButtonsHalfWidthMetres) * AskewForm.BodyDepthShare;
            hips.Add(Surface.Buttons, Ball.Of(new WorldPoint(-BeltButtonsHalfWidthMetres, height, back), BeltButtonRadiusMetres, AskewForm.FeatureSides));
            hips.Add(Surface.Buttons, Ball.Of(new WorldPoint(BeltButtonsHalfWidthMetres, height, back), BeltButtonRadiusMetres, AskewForm.FeatureSides));
        }
    }
}
