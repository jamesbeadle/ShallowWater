using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Roads;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.Fields
{
    public sealed class KeptClear
    {
        private const double CellMetres = 50;
        private const double BesideTheWaterMetres = 3;
        private const double BesideTheRiverMetres = 2.5;
        private const double BesideTheRailwayMetres = 4;
        private const double BesideTheRoadHedgeMetres = 1;
        private const double Half = 0.5;

        private readonly PlaceGrid<ClearSegment> segments = new PlaceGrid<ClearSegment>(CellMetres);
        private readonly List<FollowedLine> lines = new List<FollowedLine>();

        public IReadOnlyList<FollowedLine> Lines => lines;

        public void AroundThePound(GroundLine pound)
        {
            Around(pound, CanalSection.OuterReachMetres + BesideTheWaterMetres);
        }

        public void AroundCanal(GroundLine canal)
        {
            Around(canal, PoundLimits.ChannelHalfWidthMetres + BesideTheWaterMetres);
        }

        public void AroundTheRiver(GroundLine river)
        {
            Around(river, LineBands.RiverWidthMetres * Half + BesideTheRiverMetres);
        }

        public void AroundTheRailway(GroundLine railway)
        {
            Around(railway, LineBands.RailwayTrackWidthMetres * Half + BesideTheRailwayMetres);
        }

        public void AroundRoad(Road road)
        {
            Around(road.Line, RoadSection.ReachOf(road, Roadside.Hedgerow) + BesideTheRoadHedgeMetres);
        }

        public bool IsNear(GroundPoint place)
        {
            return segments.HasAnyNear(place, segment => segment.IsNear(place));
        }

        private void Around(GroundLine line, double reachMetres)
        {
            lines.Add(new FollowedLine(line, reachMetres));
            for (var segment = 0; segment < line.SegmentCount; segment++)
            {
                var clear = new ClearSegment(line[segment], line[segment + 1], reachMetres);
                segments.Add(clear, clear.SouthWest, clear.NorthEast);
            }
        }
    }
}
