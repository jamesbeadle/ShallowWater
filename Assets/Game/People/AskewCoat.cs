using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewCoat
    {
        private const double FaceForward = Math.PI / 2;
        private const double CollarOpeningRadians = Math.PI * 0.3;
        private static readonly WorldPoint Hips = new WorldPoint(0, 0, 0);
        private static readonly Offset BodyShape = new Offset(1, 1, AskewForm.BodyDepthShare);

        private static readonly ProfilePoint[] Coat =
        {
            new ProfilePoint(0.25, -0.3), new ProfilePoint(0.228, -0.18), new ProfilePoint(0.205, -0.06), new ProfilePoint(0.19, 0.04),
            new ProfilePoint(0.185, 0.13), new ProfilePoint(0.2, 0.26), new ProfilePoint(0.215, 0.38), new ProfilePoint(0.222, 0.47),
            new ProfilePoint(0.205, 0.53), new ProfilePoint(0.16, 0.558), new ProfilePoint(0.11, 0.575), new ProfilePoint(0.08, 0.585)
        };

        private static readonly ProfilePoint[] Shirt =
        {
            new ProfilePoint(0.062, 0.46), new ProfilePoint(0.06, 0.6)
        };

        private static readonly ProfilePoint[] Collar =
        {
            new ProfilePoint(0.108, 0.565), new ProfilePoint(0.096, 0.59), new ProfilePoint(0.108, 0.62), new ProfilePoint(0.126, 0.645)
        };

        private const double ButtonRadiusMetres = 0.012;
        private const double ButtonRowHalfWidthMetres = 0.055;

        private static readonly WorldPoint[] ButtonRow =
        {
            new WorldPoint(0, 0.4, 0.168), new WorldPoint(0, 0.28, 0.156), new WorldPoint(0, 0.16, 0.143), new WorldPoint(0, 0.04, 0.146)
        };

        public static void Dress(SurfaceShapes torso)
        {
            torso.Add(Surface.Coat, Scaled.Of(Lathe.Turned(Hips, Coat, AskewForm.RoundSides), BodyShape));
            torso.Add(Surface.Shirt, Lathe.Turned(Hips, Shirt, AskewForm.LimbSides));
            torso.Add(Surface.Coat, Lathe.TurnedThrough(Hips, Collar, AskewForm.RoundSides, FaceForward + CollarOpeningRadians, FaceForward + 2 * Math.PI - CollarOpeningRadians));
            Button(torso);
        }

        private static void Button(SurfaceShapes torso)
        {
            foreach (var place in ButtonRow)
            {
                torso.Add(Surface.Buttons, Ball.Of(new WorldPoint(-ButtonRowHalfWidthMetres, place.Height, place.North), ButtonRadiusMetres, AskewForm.FeatureSides));
                torso.Add(Surface.Buttons, Ball.Of(new WorldPoint(ButtonRowHalfWidthMetres, place.Height, place.North), ButtonRadiusMetres, AskewForm.FeatureSides));
            }
        }
    }
}
