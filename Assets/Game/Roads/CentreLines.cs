using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Roads
{
    public static class CentreLines
    {
        private const double DashMetres = 2.7;
        private const double GapMetres = 6.4;
        private const double WidthMetres = 0.1;
        private const double AboveTheRoadMetres = 0.01;

        public static void Paint(SurfaceShapes surfaces, Road road)
        {
            var dash = Band.Flat(Surface.RoadLine, WidthMetres, road.HeightMetres + AboveTheRoadMetres);
            for (var from = GapMetres; from + DashMetres <= road.LengthMetres; from += DashMetres + GapMetres)
            {
                surfaces.AddRibbon(road.Between(new Span(from, from + DashMetres)), dash);
            }
        }
    }
}
