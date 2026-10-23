using System;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class HedgerowShapes
    {
        private const double PointSpacingMetres = 8;

        public static GroundLine AddRun(SurfaceShapes surfaces, Hedgerow hedgerow, HedgeStretch run)
        {
            var bands = FieldHedgeSection.Bands(hedgerow.IsFarmBoundary);
            var line = LineOf(hedgerow, run);
            foreach (var band in bands) surfaces.AddRibbon(line, band);
            FieldHedgeEnds.Close(surfaces, line, bands);
            return line;
        }

        private static GroundLine LineOf(Hedgerow hedgerow, HedgeStretch run)
        {
            var count = Math.Max(1, (int)Math.Ceiling(run.LengthMetres / PointSpacingMetres));
            var places = Enumerable.Range(0, count + 1).Select(step => hedgerow.At(run.FromMetres + run.LengthMetres * step / count));
            return new GroundLine(places);
        }
    }
}
