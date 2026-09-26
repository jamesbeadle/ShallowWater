using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Roads
{
    public sealed class RoadSurroundings
    {
        private const double OnTheCentreline = 0;

        private readonly RoadsideClearings clearings;
        private readonly IReadOnlyList<Area> villages;

        public RoadSurroundings(RoadsideClearings clearings, IReadOnlyList<Area> villages)
        {
            this.clearings = clearings;
            this.villages = villages;
        }

        public Roadside RoadsideAt(Road road, double along, double side)
        {
            var centre = road.At(along, OnTheCentreline);
            var isInAVillage = villages.Any(village => village.IsAround(centre));
            var roadside = isInAVillage ? Roadside.Street : Roadside.Hedgerow;
            var outerEdge = road.At(along, side * RoadSection.ReachOf(road, roadside));
            var isCleared = clearings.IsCleared(outerEdge, road);
            return isCleared ? Roadside.Cleared : roadside;
        }
    }
}
