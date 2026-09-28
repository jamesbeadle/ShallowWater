using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Woods
{
    public static class CrownOutlines
    {
        private const double ShortestShare = 0.25;

        private static readonly Dictionary<CrownOutline, double[]> MarginAndSkew = new Dictionary<CrownOutline, double[]>
        {
            { CrownOutline.Dome, new[] { 0.0, 1.0 } },
            { CrownOutline.Ovoid, new[] { 0.05, 0.66 } },
            { CrownOutline.FlatTop, new[] { 0.15, 1.4 } }
        };

        public static double LengthShareAt(CrownOutline outline, double shareUpTheCrown)
        {
            var share = Math.Min(Math.Max(shareUpTheCrown, 0), 1);
            var shape = MarginAndSkew[outline];
            var margin = shape[0];
            var reach = Math.Sin(Math.PI * (margin + (1 - 2 * margin) * Math.Pow(share, shape[1])));
            return ShortestShare + (1 - ShortestShare) * reach;
        }
    }
}
