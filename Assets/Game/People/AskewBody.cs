using System;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewBody
    {
        private const double PeakForwardMetres = 0.075;
        private const double PeakHalfWidthMetres = 0.1;
        private const double PeakMetres = 0.205;
        private const double PeakRootForwardMetres = 0.085;
        private const double PeakThicknessMetres = 0.012;
        private const int PeakCorners = 7;
        private static readonly WorldPoint Pivot = new WorldPoint(0, 0, 0);
        private static readonly WorldPoint Nose = new WorldPoint(0, 0.12, 0.088);
        private const double NoseRadiusMetres = 0.018;
        private static readonly WorldPoint[] Ears = { new WorldPoint(-0.09, 0.14, -0.005), new WorldPoint(0.09, 0.14, -0.005) };
        private const double EarRadiusMetres = 0.02;
        private const int FeatureSides = 8;

        private static readonly Offset BodyShape = new Offset(1, 1, AskewForm.BodyDepthShare);

        private static readonly ProfilePoint[] Jacket =
        {
            new ProfilePoint(0.2, -0.24), new ProfilePoint(0.19, -0.12), new ProfilePoint(0.175, 0.0), new ProfilePoint(0.168, 0.12),
            new ProfilePoint(0.18, 0.26), new ProfilePoint(0.195, 0.38), new ProfilePoint(0.19, 0.45), new ProfilePoint(0.165, 0.51),
            new ProfilePoint(0.11, 0.55), new ProfilePoint(0.065, 0.575), new ProfilePoint(0, 0.58)
        };

        private static readonly ProfilePoint[] Head =
        {
            new ProfilePoint(0.052, -0.02), new ProfilePoint(0.056, 0.07), new ProfilePoint(0.08, 0.1), new ProfilePoint(0.094, 0.15),
            new ProfilePoint(0.092, 0.2), new ProfilePoint(0.07, 0.25), new ProfilePoint(0, 0.27)
        };

        private static readonly ProfilePoint[] Cap =
        {
            new ProfilePoint(0.104, 0.195), new ProfilePoint(0.118, 0.225), new ProfilePoint(0.113, 0.25), new ProfilePoint(0, 0.262)
        };

        public static SurfaceShapes Torso()
        {
            var torso = new SurfaceShapes();
            torso.Add(Surface.Jacket, Scaled.Of(Lathe.Turned(Pivot, Jacket, AskewForm.RoundSides), BodyShape));
            return torso;
        }

        public static SurfaceShapes HeadAndCap()
        {
            var head = new SurfaceShapes();
            head.Add(Surface.Skin, Lathe.Turned(Pivot, Head, AskewForm.RoundSides));
            head.Add(Surface.Skin, Ball.Of(Nose, NoseRadiusMetres, FeatureSides));
            foreach (var ear in Ears) head.Add(Surface.Skin, Ball.Of(ear, EarRadiusMetres, FeatureSides));
            head.Add(Surface.Cap, Lathe.Turned(Pivot, Cap, AskewForm.RoundSides));
            head.AddSolid(Surface.Cap, Peak(), PeakMetres, PeakMetres + PeakThicknessMetres);
            return head;
        }

        private static GroundRing Peak()
        {
            var corners = Enumerable.Range(0, PeakCorners).Select(corner =>
            {
                var around = Math.PI * corner / (PeakCorners - 1) - Math.PI / 2;
                return new GroundPoint(PeakHalfWidthMetres * Math.Sin(around), PeakRootForwardMetres + PeakForwardMetres * Math.Cos(around));
            });
            return new GroundRing(corners);
        }
    }
}
