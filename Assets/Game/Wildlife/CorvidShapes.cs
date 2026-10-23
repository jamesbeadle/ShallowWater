using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public static class CorvidShapes
    {
        private const int Sides = 8;
        private const double LegRadiusMetres = 0.004;
        private const double HeadRadiusMetres = 0.045;
        private const double FaceRadiusMetres = 0.022;
        private static readonly WorldPoint BodyCentre = new WorldPoint(0, 0.14, 0);
        private static readonly Offset BodyShape = new Offset(0.07, 0.075, 0.13);
        private static readonly WorldPoint BellyCentre = new WorldPoint(0, 0.115, 0.02);
        private static readonly Offset BellyShape = new Offset(0.058, 0.05, 0.1);
        private static readonly WorldPoint Head = new WorldPoint(0, 0.24, 0.1);
        private static readonly WorldPoint Face = new WorldPoint(0, 0.235, 0.13);
        private static readonly WorldPoint[] Beak = { new WorldPoint(0, 0.238, 0.14), new WorldPoint(0, 0.232, 0.21) };
        private static readonly double[] BeakRadiiMetres = { 0.014, 0.002 };
        private static readonly WorldPoint[] Tail = { new WorldPoint(0, 0.15, -0.12), new WorldPoint(0, 0.17, -0.26) };
        private static readonly WorldPoint[] MagpieTail = { new WorldPoint(0, 0.15, -0.12), new WorldPoint(0, 0.18, -0.36) };
        private static readonly double[] TailRadiiMetres = { 0.03, 0.04 };
        private static readonly double[] LegFeet = { -0.02, 0.02 };
        private static readonly WorldPoint RightShoulder = new WorldPoint(0.045, 0.19, 0.04);

        public static SurfaceShapes Of(QuarryKind kind)
        {
            var bird = new SurfaceShapes();
            bird.Add(Surface.Feathers, Ball.Shaped(BodyCentre, BodyShape, Sides));
            bird.Add(Surface.Feathers, Ball.Of(Head, HeadRadiusMetres, Sides));
            bird.Add(Surface.Beak, TaperedTube.Along(Beak, BeakRadiiMetres, Sides));
            var isAMagpie = kind == QuarryKind.Magpie;
            bird.Add(Surface.Feathers, TaperedTube.Along(isAMagpie ? MagpieTail : Tail, TailRadiiMetres, Sides));
            foreach (var across in LegFeet) Leg(bird, across);
            Markings(bird, kind);
            return bird;
        }

        public static WorldPoint ShoulderOf(double side)
        {
            return new WorldPoint(RightShoulder.East * side, RightShoulder.Height, RightShoulder.North);
        }

        private static void Leg(SurfaceShapes bird, double across)
        {
            var leg = new[] { new WorldPoint(across, 0, 0), new WorldPoint(across, 0.08, 0.01) };
            bird.Add(Surface.Beak, Tube.Along(leg, LegRadiusMetres, Sides));
        }

        private static void Markings(SurfaceShapes bird, QuarryKind kind)
        {
            var isAMagpie = kind == QuarryKind.Magpie;
            var isARook = kind == QuarryKind.Rook;
            if (isAMagpie) WhiteBelly(bird);
            if (isARook) GreyFace(bird);
        }

        private static void WhiteBelly(SurfaceShapes bird)
        {
            bird.Add(Surface.WhiteFeathers, Ball.Shaped(BellyCentre, BellyShape, Sides));
        }

        private static void GreyFace(SurfaceShapes bird)
        {
            bird.Add(Surface.WhiteFeathers, Ball.Of(Face, FaceRadiusMetres, Sides));
        }
    }
}
