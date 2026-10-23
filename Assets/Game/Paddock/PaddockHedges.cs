using System.Collections.Generic;
using ShallowWater.Game.Fields;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.Paddock
{
    public static class PaddockHedges
    {
        private const double HalfLength = Paddock.HalfLengthMetres;
        private const double HalfWidth = Paddock.HalfWidthMetres;

        public static List<Hedge> Grow(SurfaceShapes shapes, Paddock paddock)
        {
            var hedges = new List<Hedge>();
            var waterside = Hedgerow.WithinAFarm(paddock.Place(-HalfLength, HalfWidth), paddock.Place(HalfLength, HalfWidth));
            waterside.Open(PaddockPlan.Gateway());
            Grow(shapes, hedges, waterside);
            Grow(shapes, hedges, Hedgerow.WithinAFarm(paddock.Place(-HalfLength, -HalfWidth), paddock.Place(HalfLength, -HalfWidth)));
            Grow(shapes, hedges, Hedgerow.WithinAFarm(paddock.Place(-HalfLength, -HalfWidth), paddock.Place(-HalfLength, HalfWidth)));
            Grow(shapes, hedges, Hedgerow.WithinAFarm(paddock.Place(HalfLength, -HalfWidth), paddock.Place(HalfLength, HalfWidth)));
            return hedges;
        }

        private static void Grow(SurfaceShapes shapes, List<Hedge> hedges, Hedgerow hedgerow)
        {
            foreach (var run in HedgeRuns.Of(hedgerow)) hedges.Add(new Hedge(HedgerowShapes.AddRun(shapes, hedgerow, run), FieldHedgeSection.HalfWidthMetres));
        }
    }
}
