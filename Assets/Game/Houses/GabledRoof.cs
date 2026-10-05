using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class GabledRoof
    {
        private const double Half = 0.5;
        private const double AtTheEaves = 0;

        public static void Lay(SurfaceShapes surfaces, Run run)
        {
            var plot = run.Plot;
            var pitch = Pitch.Of(run);
            var soffit = pitch.Lowered(RoofForm.SoffitBelowTheSlatesMetres);
            var from = -run.LeftVergeMetres;
            var to = run.LengthMetres + run.RightVergeMetres;
            foreach (var side in RoofForm.Sides)
            {
                surfaces.Add(run.Roof, Slope(plot, pitch, side, from, to, true));
                surfaces.Add(Surface.Timber, Slope(plot, soffit, side, from, to, false));
                RoofEdges.AddFascia(surfaces, plot, pitch, side, from, to);
                Gutters.Hang(surfaces, plot, pitch, side, from, to);
            }
            RoofEdges.AddVerge(surfaces, run, from, run.LeftVergeMetres);
            RoofEdges.AddVerge(surfaces, run, to, run.RightVergeMetres);
            var ridgeFrom = plot.Point(from, pitch.HalfSpanMetres, pitch.RidgeMetres);
            var ridgeTo = plot.Point(to, pitch.HalfSpanMetres, pitch.RidgeMetres);
            RidgeTiles.Lay(surfaces, ridgeFrom, ridgeTo, plot.Backwards);
        }

        public static Shape Slope(Plot plot, Pitch pitch, double side, double from, double to, bool isFacingUp)
        {
            var middle = pitch.HalfSpanMetres;
            var eavesLine = pitch.EavesLine(side);
            var eaves = pitch.EavesMetres;
            var ridge = pitch.RidgeMetres;
            var up = pitch.UpTheSlopeMetres;
            var corners = new[]
            {
                plot.Point(from, eavesLine, eaves), plot.Point(to, eavesLine, eaves), plot.Point(to, middle, ridge), plot.Point(from, middle, ridge),
            };
            var places = new[] { new SurfacePlace(from, AtTheEaves), new SurfacePlace(to, AtTheEaves), new SurfacePlace(to, up), new SurfacePlace(from, up) };
            var slope = new Shape();
            Facet.Add(slope, corners, places, Viewer(plot, pitch, side, (from + to) * Half, isFacingUp));
            return slope;
        }

        private static WorldPoint Viewer(Plot plot, Pitch pitch, double side, double along, bool isFacingUp)
        {
            var aboveTheEaves = plot.Point(along, pitch.EavesLine(side), pitch.RidgeMetres + RoofForm.ViewingHeightMetres);
            var belowTheEaves = plot.Point(along, pitch.EavesLine(side), pitch.EavesMetres - RoofForm.ViewingHeightMetres);
            return isFacingUp ? aboveTheEaves : belowTheEaves;
        }
    }
}
