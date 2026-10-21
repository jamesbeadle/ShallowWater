using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public static class RatShapes
    {
        private const int Sides = 8;
        private const double EarRadiusMetres = 0.013;
        private const double EyeRadiusMetres = 0.005;
        private static readonly WorldPoint BodyCentre = new WorldPoint(0, 0.05, 0);
        private static readonly Offset BodyShape = new Offset(0.045, 0.045, 0.11);
        private static readonly WorldPoint HeadCentre = new WorldPoint(0, 0.055, 0.12);
        private static readonly Offset HeadShape = new Offset(0.03, 0.03, 0.055);
        private static readonly WorldPoint[] Ears = { new WorldPoint(-0.022, 0.085, 0.1), new WorldPoint(0.022, 0.085, 0.1) };
        private static readonly WorldPoint[] Eyes = { new WorldPoint(-0.018, 0.065, 0.15), new WorldPoint(0.018, 0.065, 0.15) };
        private static readonly WorldPoint[] Tail = { new WorldPoint(0, 0.035, -0.1), new WorldPoint(0, 0.02, -0.3) };
        private static readonly double[] TailRadiiMetres = { 0.008, 0.002 };

        public static SurfaceShapes Rat()
        {
            var rat = new SurfaceShapes();
            rat.Add(Surface.Fur, Ball.Shaped(BodyCentre, BodyShape, Sides));
            rat.Add(Surface.Fur, Ball.Shaped(HeadCentre, HeadShape, Sides));
            foreach (var ear in Ears) rat.Add(Surface.Fur, Ball.Of(ear, EarRadiusMetres, Sides));
            foreach (var eye in Eyes) rat.Add(Surface.Eyes, Ball.Of(eye, EyeRadiusMetres, Sides));
            rat.Add(Surface.Skin, TaperedTube.Along(Tail, TailRadiiMetres, Sides));
            return rat;
        }
    }
}
