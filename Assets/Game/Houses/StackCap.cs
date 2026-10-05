using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class StackCap
    {
        public const double DepthMetres = 0.3;
        private static readonly (double belowTheTop, double upToBelowTheTop, double outward)[] Courses =
        {
            (0.3, 0.225, 0.04),
            (0.225, 0.15, 0.085),
            (0.15, 0.0, 0.045),
        };

        public static void Add(SurfaceShapes surfaces, Plot plot, StackPlace stack, double middle, double top)
        {
            foreach (var course in Courses)
            {
                var outline = plot.Oblong(stack.AlongMetres, middle, stack.HalfAlongMetres + course.outward, stack.HalfAcrossMetres + course.outward);
                surfaces.AddBlock(Surface.Brickwork, outline, top - course.belowTheTop, top - course.upToBelowTheTop);
            }
        }
    }
}
