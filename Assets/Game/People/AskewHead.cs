using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewHead
    {
        private const double FaceForward = Math.PI / 2;
        private const double StubbleSpreadRadians = Math.PI * 0.6;
        private const double HairSpreadRadians = Math.PI * 0.62;
        private static readonly WorldPoint Neck = new WorldPoint(0, 0, 0);
        private static readonly Offset SkullShape = new Offset(AskewForm.HeadWidthShare, 1, 1);
        private static readonly WorldPoint Nose = new WorldPoint(0, 0.122, 0.092);
        private static readonly Offset NoseShape = new Offset(0.014, 0.03, 0.028);
        private static readonly WorldPoint Brow = new WorldPoint(0, 0.172, 0.07);
        private static readonly Offset BrowShape = new Offset(0.062, 0.011, 0.026);
        private static readonly WorldPoint[] Eyes = { new WorldPoint(-0.03, 0.15, 0.081), new WorldPoint(0.03, 0.15, 0.081) };
        private const double EyeRadiusMetres = 0.0085;
        private static readonly WorldPoint[] Ears = { new WorldPoint(-0.08, 0.13, -0.01), new WorldPoint(0.08, 0.13, -0.01) };
        private static readonly Offset EarShape = new Offset(0.01, 0.026, 0.018);

        private static readonly ProfilePoint[] Skull =
        {
            new ProfilePoint(0.055, -0.03), new ProfilePoint(0.057, 0.03), new ProfilePoint(0.062, 0.045), new ProfilePoint(0.076, 0.07),
            new ProfilePoint(0.088, 0.11), new ProfilePoint(0.095, 0.16), new ProfilePoint(0.096, 0.2), new ProfilePoint(0.088, 0.235),
            new ProfilePoint(0.06, 0.262), new ProfilePoint(0, 0.272)
        };

        private static readonly ProfilePoint[] Stubble =
        {
            new ProfilePoint(0.06, 0.036), new ProfilePoint(0.068, 0.05), new ProfilePoint(0.081, 0.072), new ProfilePoint(0.092, 0.105)
        };

        private static readonly ProfilePoint[] Hair =
        {
            new ProfilePoint(0.07, 0.055), new ProfilePoint(0.08, 0.075), new ProfilePoint(0.092, 0.11), new ProfilePoint(0.099, 0.16),
            new ProfilePoint(0.1, 0.2), new ProfilePoint(0.092, 0.235)
        };

        public static void Shape(SurfaceShapes head)
        {
            head.Add(Surface.Skin, Scaled.Of(Lathe.Turned(Neck, Skull, AskewForm.RoundSides), SkullShape));
            head.Add(Surface.Skin, Ball.Shaped(Nose, NoseShape, AskewForm.FeatureSides));
            head.Add(Surface.Skin, Ball.Shaped(Brow, BrowShape, AskewForm.FeatureSides));
            foreach (var eye in Eyes) head.Add(Surface.Eyes, Ball.Of(eye, EyeRadiusMetres, AskewForm.FeatureSides));
            foreach (var ear in Ears) head.Add(Surface.Skin, Ball.Shaped(ear, EarShape, AskewForm.FeatureSides));
            head.Add(Surface.Stubble, Scaled.Of(Lathe.TurnedThrough(Neck, Stubble, AskewForm.RoundSides, FaceForward - StubbleSpreadRadians, FaceForward + StubbleSpreadRadians), SkullShape));
            head.Add(Surface.Hair, Scaled.Of(Lathe.TurnedThrough(Neck, Hair, AskewForm.RoundSides, FaceForward + HairSpreadRadians, FaceForward + 2 * Math.PI - HairSpreadRadians), SkullShape));
        }
    }
}
