using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public static class WingShapes
    {
        public const double LeftSide = -1;
        public const double RightSide = 1;
        private const double ThicknessMetres = 0.012;
        private const double Half = 0.5;

        private static readonly GroundPoint[] Planform =
        {
            new GroundPoint(0, 0.07), new GroundPoint(0.16, 0.085), new GroundPoint(0.3, 0.055), new GroundPoint(0.38, 0),
            new GroundPoint(0.34, -0.08), new GroundPoint(0.18, -0.125), new GroundPoint(0, -0.11)
        };

        public static SurfaceShapes Of(double side)
        {
            var wing = new SurfaceShapes();
            var outline = new GroundRing(Planform.Select(corner => new GroundPoint(corner.East * side, corner.North)));
            wing.AddBlock(Surface.Feathers, outline, -ThicknessMetres * Half, ThicknessMetres * Half);
            return wing;
        }
    }
}
