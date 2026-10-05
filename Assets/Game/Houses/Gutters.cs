using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class Gutters
    {
        private const double InsideTheFasciaMetres = 0.02;
        private const double OutsideTheFasciaMetres = 0.11;
        private const double BelowTheSlatesMetres = 0.02;
        private const double DepthMetres = 0.1;
        private const double Half = 0.5;

        public static void Hang(SurfaceShapes surfaces, Plot plot, Pitch pitch, double side, double from, double to)
        {
            var line = pitch.EavesLine(side);
            var middle = line + side * (OutsideTheFasciaMetres - InsideTheFasciaMetres) * Half;
            var halfDepth = (OutsideTheFasciaMetres + InsideTheFasciaMetres) * Half;
            var gutter = plot.Oblong((from + to) * Half, middle, (to - from) * Half, halfDepth);
            var top = pitch.EavesMetres - BelowTheSlatesMetres;
            surfaces.AddBlock(Surface.Iron, gutter, top - DepthMetres, top);
        }

        public static double BottomOf(Pitch pitch)
        {
            return pitch.EavesMetres - BelowTheSlatesMetres - DepthMetres;
        }

        public static double OutFromTheEavesLine => OutsideTheFasciaMetres * Half;
    }
}
