using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class Timbers
    {
        private const double Half = 0.5;

        public static void AddBox(SurfaceShapes surfaces, GroundPoint centre, GroundPoint along, Measure size, double bottomMetres, double topMetres)
        {
            var lengthways = along * (size.LengthMetres * Half);
            var sideways = along.RightAngleClockwise * (size.WidthMetres * Half);
            var back = centre - lengthways;
            var front = centre + lengthways;
            var footprint = new GroundRing(new[] { back - sideways, front - sideways, front + sideways, back + sideways });
            surfaces.AddSolid(Surface.Timber, footprint, Heights.GroundMetres + bottomMetres, Heights.GroundMetres + topMetres);
        }

        public static void AddPost(SurfaceShapes surfaces, GroundPoint foot, GroundPoint along, double sideMetres, double heightMetres)
        {
            AddBox(surfaces, foot, along, new Measure(sideMetres, sideMetres), 0, heightMetres);
        }

        public static void AddRail(SurfaceShapes surfaces, GroundPoint from, GroundPoint to, Measure section, double heightMetres)
        {
            var run = to - from;
            var middle = from + run * Half;
            var size = new Measure(run.Length, section.WidthMetres);
            AddBox(surfaces, middle, run.Normalised, size, heightMetres - section.LengthMetres * Half, heightMetres + section.LengthMetres * Half);
        }
    }
}
