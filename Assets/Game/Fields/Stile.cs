using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class Stile
    {
        private const double PostSideMetres = 0.12;
        private const double PostHeightMetres = 1.15;
        private const double TopRailMetres = 1.0;
        private const double LowRailMetres = 0.5;
        private const double StepMetres = 0.38;
        private static readonly Measure RailSection = new Measure(0.1, 0.06);
        private static readonly Measure StepSize = new Measure(0.9, 0.22);

        public static void Add(SurfaceShapes surfaces, GroundPoint from, GroundPoint to)
        {
            var along = (to - from).Normalised;
            Timbers.AddPost(surfaces, from, along, PostSideMetres, PostHeightMetres);
            Timbers.AddPost(surfaces, to, along, PostSideMetres, PostHeightMetres);
            Timbers.AddRail(surfaces, from, to, RailSection, TopRailMetres);
            Timbers.AddRail(surfaces, from, to, RailSection, LowRailMetres);
            var middle = from + (to - from) * 0.5;
            Timbers.AddBox(surfaces, middle, along.RightAngleClockwise, StepSize, 0, StepMetres);
        }
    }
}
