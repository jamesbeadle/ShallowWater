using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Fields
{
    public static class HedgeRuns
    {
        private const double SampleMetres = 6;
        private const double ShortestRunMetres = 1;

        public static List<HedgeStretch> Of(Hedgerow hedgerow, Farmland farmland)
        {
            return BetweenTheGaps(hedgerow).SelectMany(stretch => ClearRuns(hedgerow, stretch, farmland)).Where(IsLongEnough).ToList();
        }

        private static IEnumerable<HedgeStretch> BetweenTheGaps(Hedgerow hedgerow)
        {
            var from = 0.0;
            foreach (var gap in hedgerow.Gaps)
            {
                yield return new HedgeStretch(from, gap.FromMetres);
                from = Math.Max(from, gap.ToMetres);
            }
            yield return new HedgeStretch(from, hedgerow.LengthMetres);
        }

        private static IEnumerable<HedgeStretch> ClearRuns(Hedgerow hedgerow, HedgeStretch stretch, Farmland farmland)
        {
            var samples = Samples(stretch).ToList();
            var runStart = double.NaN;
            var lastClear = double.NaN;
            foreach (var along in samples)
            {
                var isClear = farmland.IsHedged(hedgerow.At(along));
                if (isClear && double.IsNaN(runStart)) runStart = along;
                if (isClear) lastClear = along;
                if (isClear || double.IsNaN(runStart)) continue;
                yield return new HedgeStretch(runStart, lastClear);
                runStart = double.NaN;
            }
            if (!double.IsNaN(runStart)) yield return new HedgeStretch(runStart, lastClear);
        }

        private static IEnumerable<double> Samples(HedgeStretch stretch)
        {
            var count = Math.Max(1, (int)Math.Ceiling(stretch.LengthMetres / SampleMetres));
            return Enumerable.Range(0, count + 1).Select(step => stretch.FromMetres + stretch.LengthMetres * step / count);
        }

        private static bool IsLongEnough(HedgeStretch run)
        {
            return run.LengthMetres >= ShortestRunMetres;
        }
    }
}
