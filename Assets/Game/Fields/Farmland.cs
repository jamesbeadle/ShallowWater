using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class Farmland
    {
        public const double ReachMetres = 1500;
        private const double HedgesBeyondTheFieldsMetres = 80;
        private const double WaterMeadowMetres = 260;

        private readonly DistanceGrid fromThePound;
        private readonly DistanceGrid fromTheRiver;
        private readonly KeptClear keptClear;
        private readonly IReadOnlyList<Area> unfarmed;

        public Farmland(GroundLine pound, IEnumerable<GroundLine> rivers, KeptClear keptClear, IEnumerable<Area> unfarmed)
        {
            Bounds = Bounds.Around(pound.Points, ReachMetres + HedgesBeyondTheFieldsMetres);
            fromThePound = new DistanceGrid(Bounds, new[] { pound });
            fromTheRiver = new DistanceGrid(Bounds, rivers);
            this.keptClear = keptClear;
            this.unfarmed = unfarmed.ToList();
        }

        public Bounds Bounds { get; }
        public IReadOnlyList<FollowedLine> FollowedLines => keptClear.Lines;

        public bool IsFarmed(GroundPoint place)
        {
            var isInReach = fromThePound.At(place) <= ReachMetres;
            return isInReach && !IsUnfarmed(place);
        }

        public bool IsHedged(GroundPoint place)
        {
            var isInReach = fromThePound.At(place) <= ReachMetres + HedgesBeyondTheFieldsMetres;
            return isInReach && !IsUnfarmed(place) && !keptClear.IsNear(place);
        }

        public bool IsWaterMeadow(GroundPoint place)
        {
            return fromTheRiver.At(place) <= WaterMeadowMetres;
        }

        public bool IsNearTheFarmland(GroundPoint place, double reachMetres)
        {
            return fromThePound.At(place) <= ReachMetres + reachMetres;
        }

        private bool IsUnfarmed(GroundPoint place)
        {
            return unfarmed.Any(area => area.IsAround(place));
        }
    }
}
