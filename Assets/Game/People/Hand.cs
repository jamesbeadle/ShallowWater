using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Hand
    {
        private const double WristRadiusMetres = 0.03;
        private const double FingerRootRadiusMetres = 0.0095;
        private const double FingerTipRadiusMetres = 0.0075;
        private const double ThumbRootRadiusMetres = 0.012;
        private const double ThumbTipRadiusMetres = 0.0095;
        private const int FingerSides = 6;
        private const double TowardsTheRight = 1;
        private const double TowardsTheLeft = -1;
        private static readonly WorldPoint Wrist = new WorldPoint(0, 0, 0);
        private static readonly WorldPoint Palm = new WorldPoint(0, -0.058, 0.004);
        private static readonly Offset PalmShape = new Offset(0.024, 0.075, 0.046);
        private static readonly double[] FingersAcross = { -0.022, -0.008, 0.007, 0.021 };
        private static readonly double[] FingerCurl = { 0.009, 0.028 };
        private static readonly double[] FingerReach = { 0.028, 0.052 };
        private const double FingerRootBelowTheWristMetres = 0.105;
        private const double FingerDroopMetres = 0.008;

        public static SurfaceShapes Relaxed(FigureSide side)
        {
            var inward = side == FigureSide.Right ? TowardsTheLeft : TowardsTheRight;
            var hand = new SurfaceShapes();
            hand.Add(Surface.Skin, Ball.Of(Wrist, WristRadiusMetres, AskewForm.FeatureSides));
            hand.Add(Surface.Skin, Ball.Shaped(Palm, PalmShape, AskewForm.FeatureSides));
            foreach (var across in FingersAcross) hand.Add(Surface.Skin, Finger(across, inward));
            hand.Add(Surface.Skin, Thumb(inward));
            return hand;
        }

        private static Shape Finger(double across, double inward)
        {
            var root = new WorldPoint(0, -FingerRootBelowTheWristMetres, across);
            var knuckle = new WorldPoint(inward * FingerCurl[0], root.Height - FingerReach[0], across - FingerDroopMetres);
            var tip = new WorldPoint(inward * FingerCurl[1], root.Height - FingerReach[1], across - FingerDroopMetres * 2);
            var radii = new[] { FingerRootRadiusMetres, FingerRootRadiusMetres, FingerTipRadiusMetres };
            return TaperedTube.Along(new[] { root, knuckle, tip }, radii, FingerSides);
        }

        private static Shape Thumb(double inward)
        {
            var root = new WorldPoint(inward * -0.004, -0.045, 0.024);
            var tip = new WorldPoint(inward * 0.012, -0.085, 0.05);
            return TaperedTube.Along(new[] { root, tip }, new[] { ThumbRootRadiusMetres, ThumbTipRadiusMetres }, FingerSides);
        }
    }
}
