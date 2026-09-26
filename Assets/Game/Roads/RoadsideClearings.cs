using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;

namespace ShallowWater.Game.Roads
{
    public sealed class RoadsideClearings
    {
        private const int NotARoad = -1;
        private const int SegmentsPerChunk = 16;
        private const double JunctionMouthMetres = 3;
        private const double BesideTheWaterMetres = 4.5;
        private const double BesideTheRiverMetres = 3;
        private const double BesideTheRailwayMetres = 3;
        private const double Half = 0.5;

        private readonly List<ClearedChunk> chunks = new List<ClearedChunk>();

        public void ClearAroundThePound(GroundLine pound)
        {
            ClearAround(pound, CanalSection.OuterReachMetres + BesideTheWaterMetres, NotARoad);
        }

        public void ClearAroundCanal(GroundLine canal)
        {
            ClearAround(canal, PoundLimits.ChannelHalfWidthMetres + BesideTheWaterMetres, NotARoad);
        }

        public void ClearAroundTheRiver(GroundLine river)
        {
            ClearAround(river, LineBands.RiverWidthMetres * Half + BesideTheRiverMetres, NotARoad);
        }

        public void ClearAroundTheRailway(GroundLine railway)
        {
            ClearAround(railway, LineBands.RailwayTrackWidthMetres * Half + BesideTheRailwayMetres, NotARoad);
        }

        public void ClearAroundRoad(Road road)
        {
            ClearAround(road.Line, road.HalfWidthMetres + JunctionMouthMetres, road.Index);
        }

        public bool IsCleared(GroundPoint place, Road road)
        {
            var roadIndex = road.Index;
            return chunks.Any(chunk => chunk.Owner != roadIndex && chunk.IsNear(place));
        }

        private void ClearAround(GroundLine line, double reachMetres, int owner)
        {
            var points = line.Points;
            for (var start = 0; start < line.SegmentCount; start += SegmentsPerChunk)
            {
                var count = Math.Min(SegmentsPerChunk + 1, points.Count - start);
                chunks.Add(new ClearedChunk(points.Skip(start).Take(count).ToList(), reachMetres, owner));
            }
        }
    }
}
