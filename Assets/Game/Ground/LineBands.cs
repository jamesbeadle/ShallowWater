using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Ground
{
    public static class LineBands
    {
        public const double RiverWidthMetres = 16;
        public const double RailwayTrackWidthMetres = 3.5;

        private static readonly Dictionary<string, Band> ByKind = new Dictionary<string, Band>
        {
            { LineKinds.River, Band.Flat(Surface.River, RiverWidthMetres, Heights.RiverMetres) },
            { LineKinds.Railway, Band.Flat(Surface.Railway, RailwayTrackWidthMetres, Heights.RailwayMetres) },
        };

        public static Band For(string kind)
        {
            var isKnown = ByKind.TryGetValue(kind, out var band);
            if (isKnown) return band;
            throw new ArgumentException($"No width is known for a line of kind '{kind}'.", nameof(kind));
        }
    }
}
