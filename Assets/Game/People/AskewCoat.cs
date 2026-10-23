using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewCoat
    {
        private const double FaceForward = Math.PI / 2;
        private const double CollarOpeningRadians = Math.PI * 0.3;
        private const double NeckRadiusMetres = 0.066;
        private const double ButtonRadiusMetres = 0.012;
        private const double ButtonRowHalfWidthMetres = 0.055;
        private const double ButtonProudMetres = 0.004;
        private static readonly WorldPoint Waist = new WorldPoint(0, 0, 0);
        private static readonly Offset BodyShape = new Offset(1, 1, AskewForm.BodyDepthShare);

        private static readonly ProfilePoint[] Body =
        {
            new ProfilePoint(0.19, 0), new ProfilePoint(0.196, 0.04), new ProfilePoint(0.207, 0.1), new ProfilePoint(0.224, 0.18),
            new ProfilePoint(0.236, 0.25), new ProfilePoint(0.238, 0.29), new ProfilePoint(0.226, 0.32), new ProfilePoint(0.188, 0.345),
            new ProfilePoint(0.14, 0.365), new ProfilePoint(0.098, 0.382), new ProfilePoint(0.074, 0.39)
        };

        private static readonly ProfilePoint[] Neck =
        {
            new ProfilePoint(NeckRadiusMetres, 0.35), new ProfilePoint(NeckRadiusMetres, 0.42)
        };

        private static readonly ProfilePoint[] Shirt =
        {
            new ProfilePoint(0.072, 0.3), new ProfilePoint(0.074, 0.36), new ProfilePoint(0.072, 0.43)
        };

        private static readonly ProfilePoint[] Collar =
        {
            new ProfilePoint(0.1, 0.38), new ProfilePoint(0.095, 0.4), new ProfilePoint(0.108, 0.435), new ProfilePoint(0.128, 0.465)
        };

        private static readonly double[] ButtonHeights = { 0.04, 0.13, 0.22 };

        public static SurfaceShapes Dress(Outfit outfit)
        {
            var torso = new SurfaceShapes();
            torso.Add(outfit.Coat, Scaled.Of(Lathe.Turned(Waist, Body, AskewForm.RoundSides), BodyShape));
            torso.Add(Surface.Skin, Lathe.Turned(Waist, Neck, AskewForm.LimbSides));
            torso.Add(Surface.Shirt, Lathe.Turned(Waist, Shirt, AskewForm.LimbSides));
            torso.Add(outfit.Coat, Lathe.TurnedThrough(Waist, Collar, AskewForm.RoundSides, FaceForward + CollarOpeningRadians, FaceForward + 2 * Math.PI - CollarOpeningRadians));
            if (outfit.HasAScarf) AskewScarf.Wrap(torso);
            Button(torso, outfit);
            return torso;
        }

        private static void Button(SurfaceShapes torso, Outfit outfit)
        {
            var rowHalfWidth = outfit.IsDoubleBreasted ? ButtonRowHalfWidthMetres : 0;
            foreach (var height in ButtonHeights)
            {
                var front = CoatFrontAt(height, rowHalfWidth) + ButtonProudMetres;
                torso.Add(Surface.Buttons, Ball.Of(new WorldPoint(-rowHalfWidth, height, front), ButtonRadiusMetres, AskewForm.FeatureSides));
                if (!outfit.IsDoubleBreasted) continue;
                torso.Add(Surface.Buttons, Ball.Of(new WorldPoint(rowHalfWidth, height, front), ButtonRadiusMetres, AskewForm.FeatureSides));
            }
        }

        private static double CoatFrontAt(double height, double acrossMetres)
        {
            var radius = Profiles.RadiusAt(Body, height);
            return Math.Sqrt(Math.Max(0, radius * radius - acrossMetres * acrossMetres)) * AskewForm.BodyDepthShare;
        }
    }
}
