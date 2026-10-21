using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Farmyard
{
    public static class FarmyardPlan
    {
        public const double RickRadiusMetres = 3.2;
        public const double RickApexMetres = 4.6;
        public const double MuckHeapRadiusMetres = 2.6;
        public const double MuckHeapHeightMetres = 1.1;
        private const double RatRunFromTheRickMetres = 0.5;
        private const int RatRunsRoundARick = 6;

        private static readonly (double Along, double Across)[] Ricks = { (-10, -6), (9, -7) };
        private static readonly (double Along, double Across) MuckHeap = (0, -12);

        public static IEnumerable<GroundPoint> RickFeet(Farmyard yard)
        {
            return Ricks.Select(rick => yard.Place(rick.Along, rick.Across));
        }

        public static GroundPoint MuckHeapFoot(Farmyard yard)
        {
            return yard.Place(MuckHeap.Along, MuckHeap.Across);
        }

        public static IEnumerable<GroundPoint> RatRuns(Farmyard yard)
        {
            var runs = RickFeet(yard).SelectMany(RoundTheRick).ToList();
            runs.Add(MuckHeapFoot(yard));
            return runs;
        }

        private static IEnumerable<GroundPoint> RoundTheRick(GroundPoint foot)
        {
            var ring = Footprints.Round(foot, RickRadiusMetres + RatRunFromTheRickMetres, RatRunsRoundARick);
            return ring.Corners;
        }
    }
}
