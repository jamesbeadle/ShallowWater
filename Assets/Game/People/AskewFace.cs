using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewFace
    {
        private const int NoseSides = 8;
        private static readonly WorldPoint AtTheNeck = new WorldPoint(0, 0, 0);
        public static readonly Offset RightEyeFromTheNeck = new Offset(0.031, 0.155, 0.087);
        private static readonly WorldPoint[] Eyes = { Mirrored(RightEyeFromTheNeck).From(AtTheNeck), RightEyeFromTheNeck.From(AtTheNeck) };
        private static readonly Offset EyeShape = new Offset(0.013, 0.007, 0.009);
        private static readonly WorldPoint[] Eyebrows = { new WorldPoint(-0.034, 0.172, 0.104), new WorldPoint(0.034, 0.172, 0.104) };
        private static readonly Offset EyebrowShape = new Offset(0.024, 0.005, 0.009);
        private static readonly WorldPoint[] Nose = { new WorldPoint(0, 0.168, 0.098), new WorldPoint(0.002, 0.142, 0.108), new WorldPoint(0.003, 0.121, 0.112) };
        private static readonly double[] NoseRadiiMetres = { 0.01, 0.013, 0.017 };
        private static readonly WorldPoint Mouth = new WorldPoint(0, 0.085, 0.097);
        private static readonly Offset MouthShape = new Offset(0.026, 0.004, 0.008);
        private static readonly WorldPoint[] Ears = { new WorldPoint(-0.084, 0.145, -0.012), new WorldPoint(0.084, 0.145, -0.012) };
        private static readonly Offset EarShape = new Offset(0.01, 0.028, 0.019);

        public static void Feature(SurfaceShapes head)
        {
            foreach (var eye in Eyes) head.Add(Surface.Eyes, Ball.Shaped(eye, EyeShape, AskewForm.FeatureSides));
            foreach (var eyebrow in Eyebrows) head.Add(Surface.Hair, Ball.Shaped(eyebrow, EyebrowShape, AskewForm.FeatureSides));
            head.Add(Surface.Skin, TaperedTube.Along(Nose, NoseRadiiMetres, NoseSides));
            head.Add(Surface.Stubble, Ball.Shaped(Mouth, MouthShape, AskewForm.FeatureSides));
            foreach (var ear in Ears) head.Add(Surface.Skin, Ball.Shaped(ear, EarShape, AskewForm.FeatureSides));
        }

        private static Offset Mirrored(Offset onTheRight)
        {
            return new Offset(-onTheRight.Eastward, onTheRight.Upward, onTheRight.Northward);
        }
    }
}
