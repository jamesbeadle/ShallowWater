using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Mooring
{
    public sealed class LeadStations
    {
        private const double StationMetres = 1.5;
        private const double LookAroundMetres = 10;
        private const int Fewest = 1;
        private const double NoRun = 1e-9;

        private readonly double lengthMetres;
        private readonly List<WaterPosition> stations;

        public LeadStations(GroundPoint from, GroundPoint direction, double lengthMetres, SurfaceBeneath beneath)
        {
            this.lengthMetres = lengthMetres;
            var first = beneath.WaterPositionAt(from);
            var nearby = Span.Around(first.Along, lengthMetres + LookAroundMetres);
            var count = Math.Max(Fewest, (int)Math.Ceiling(lengthMetres / StationMetres));
            stations = Enumerable.Range(0, count + 1)
                .Select(station => from + direction * (lengthMetres * station / count))
                .Select(place => beneath.WaterPositionWithin(place, nearby))
                .ToList();
        }

        public WaterPosition At(double along)
        {
            var isOneStation = lengthMetres <= NoRun;
            if (isOneStation) return stations[0];
            var spacing = lengthMetres / (stations.Count - 1);
            var station = Math.Min((int)(along / spacing), stations.Count - 2);
            var share = along / spacing - station;
            return Between(stations[station], stations[station + 1], share);
        }

        private static WaterPosition Between(WaterPosition before, WaterPosition after, double share)
        {
            return new WaterPosition(before.Along + (after.Along - before.Along) * share, before.Across + (after.Across - before.Across) * share);
        }
    }
}
