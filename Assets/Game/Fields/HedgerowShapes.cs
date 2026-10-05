using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Woods;

namespace ShallowWater.Game.Fields
{
    public static class HedgerowShapes
    {
        private const double PointSpacingMetres = 8;

        public static void Add(SurfaceShapes surfaces, List<Tree> trees, Hedgerow hedgerow, Farmland farmland, Random random)
        {
            var bands = FieldHedgeSection.Bands(hedgerow.IsFarmBoundary);
            foreach (var run in HedgeRuns.Of(hedgerow, farmland))
            {
                var line = LineOf(hedgerow, run);
                foreach (var band in bands) surfaces.AddRibbon(line, band);
                FieldHedgeEnds.Close(surfaces, line, bands);
                trees.AddRange(HedgerowTrees.Along(hedgerow, run, random));
            }
            foreach (var gap in hedgerow.Gaps) GapFurnishing.Add(surfaces, hedgerow, gap, farmland, random);
        }

        private static GroundLine LineOf(Hedgerow hedgerow, HedgeStretch run)
        {
            var count = Math.Max(1, (int)Math.Ceiling(run.LengthMetres / PointSpacingMetres));
            var places = Enumerable.Range(0, count + 1).Select(step => hedgerow.At(run.FromMetres + run.LengthMetres * step / count));
            return new GroundLine(places);
        }
    }
}
