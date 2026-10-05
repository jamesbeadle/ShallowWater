using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class Hedgerow
    {
        private readonly List<HedgeGap> gaps = new List<HedgeGap>();

        private Hedgerow(GroundPoint from, GroundPoint to, bool isFarmBoundary)
        {
            From = from;
            To = to;
            IsFarmBoundary = isFarmBoundary;
        }

        public GroundPoint From { get; }
        public GroundPoint To { get; }
        public bool IsFarmBoundary { get; }
        public IReadOnlyList<HedgeGap> Gaps => gaps;
        public double LengthMetres => From.DistanceTo(To);
        public GroundPoint Direction => (To - From).Normalised;

        public static Hedgerow WithinAFarm(GroundPoint from, GroundPoint to)
        {
            return new Hedgerow(from, to, isFarmBoundary: false);
        }

        public static Hedgerow BetweenFarms(GroundPoint from, GroundPoint to)
        {
            return new Hedgerow(from, to, isFarmBoundary: true);
        }

        public GroundPoint At(double alongMetres)
        {
            return From + Direction * alongMetres;
        }

        public bool IsOpenBetween(double fromMetres, double toMetres)
        {
            return !gaps.Any(gap => gap.IsOverlapping(fromMetres, toMetres));
        }

        public void Open(HedgeGap gap)
        {
            gaps.Add(gap);
            gaps.Sort((first, second) => first.FromMetres.CompareTo(second.FromMetres));
        }
    }
}
