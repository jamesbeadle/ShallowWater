using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class RoofEdges
    {
        public const double PlainVergeMetres = 0.1;
        private const double BargeboardMetres = 0.24;
        private const double FinialHalfWidthMetres = 0.045;
        private const double FinialReachMetres = 0.38;
        private const double ViewingDistanceMetres = 1;
        private const double NoVerge = 0;
        private const double Half = 0.5;

        public static void AddFascia(SurfaceShapes surfaces, Plot plot, Pitch pitch, double side, double from, double to)
        {
            var line = pitch.EavesLine(side);
            var top = pitch.EavesMetres;
            var bottom = top - RoofForm.SoffitBelowTheSlatesMetres;
            var corners = new[] { plot.Point(from, line, bottom), plot.Point(to, line, bottom), plot.Point(to, line, top), plot.Point(from, line, top) };
            var viewer = plot.Point((from + to) * Half, line + side * ViewingDistanceMetres, top);
            var fascia = new Shape();
            Facet.Add(fascia, corners, viewer);
            surfaces.Add(Surface.Timber, fascia);
        }

        public static void AddVerge(SurfaceShapes surfaces, Run run, double along, double vergeMetres)
        {
            if (vergeMetres <= NoVerge) return;
            var plot = run.Plot;
            var pitch = Pitch.Of(run);
            var style = run.Style;
            var depth = style.HasBargeboards ? BargeboardMetres : PlainVergeMetres;
            var outward = along < run.LengthMetres * Half ? -ViewingDistanceMetres : ViewingDistanceMetres;
            foreach (var side in RoofForm.Sides) AddVergeBoard(surfaces, plot, pitch, side, along, depth, outward);
            if (!style.HasBargeboards) return;
            var finial = Footprints.Oblong(plot.At(along, pitch.HalfSpanMetres), plot.Lengthways, FinialHalfWidthMetres, FinialHalfWidthMetres);
            surfaces.AddBlock(Surface.Timber, finial, pitch.RidgeMetres - FinialReachMetres, pitch.RidgeMetres + FinialReachMetres);
        }

        public static void AddVergeBoard(SurfaceShapes surfaces, Plot plot, Pitch pitch, double side, double along, double depth, double outward)
        {
            var eavesLine = pitch.EavesLine(side);
            var middle = pitch.HalfSpanMetres;
            var eaves = pitch.EavesMetres;
            var ridge = pitch.RidgeMetres;
            var corners = new[]
            {
                plot.Point(along, eavesLine, eaves), plot.Point(along, middle, ridge),
                plot.Point(along, middle, ridge - depth), plot.Point(along, eavesLine, eaves - depth),
            };
            var viewer = plot.Point(along + outward, middle, eaves);
            var board = new Shape();
            Facet.Add(board, corners, viewer);
            surfaces.Add(Surface.Timber, board);
        }
    }
}
