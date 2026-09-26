using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Ground
{
    public static class LineBands
    {
        private const double RiverWidthMetres = 16;
        private const double RailwayTrackWidthMetres = 3.5;
        private const double PrimaryRoadWidthMetres = 8;
        private const double SecondaryRoadWidthMetres = 7;
        private const double TertiaryRoadWidthMetres = 6;
        private const double UnclassifiedRoadWidthMetres = 4.5;

        private static readonly Dictionary<string, Band> ByKind = new Dictionary<string, Band>
        {
            { LineKinds.River, Band.Flat(Surface.River, RiverWidthMetres, Heights.RiverMetres) },
            { LineKinds.Railway, Band.Flat(Surface.Railway, RailwayTrackWidthMetres, Heights.RailwayMetres) },
            { LineKinds.PrimaryRoad, Band.Flat(Surface.MainRoad, PrimaryRoadWidthMetres, Heights.RoadMetres) },
            { LineKinds.SecondaryRoad, Band.Flat(Surface.MainRoad, SecondaryRoadWidthMetres, Heights.RoadMetres) },
            { LineKinds.TertiaryRoad, Band.Flat(Surface.Road, TertiaryRoadWidthMetres, Heights.RoadMetres) },
            { LineKinds.UnclassifiedRoad, Band.Flat(Surface.Lane, UnclassifiedRoadWidthMetres, Heights.RoadMetres) },
        };

        public static Band For(string kind)
        {
            var isKnown = ByKind.TryGetValue(kind, out var band);
            if (isKnown) return band;
            throw new ArgumentException($"No width is known for a line of kind '{kind}'.", nameof(kind));
        }
    }
}
