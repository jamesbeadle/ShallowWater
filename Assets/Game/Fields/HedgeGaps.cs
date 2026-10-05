using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Fields
{
    public static class HedgeGaps
    {
        private const double ShallowestSine = 0.35;
        private const double Half = 0.5;
        private const double GatewayMetres = 3.6;
        private const double GatewayFromTheEndMetres = 12;
        private const double FieldGatewayChance = 0.85;
        private const double FarmGatewayChance = 0.3;
        private const double ShutGateShare = 0.6;
        private const double OpenGateShare = 0.85;
        private const double BothEnds = 2;
        private const double NoRoom = 0;

        public static void Open(IEnumerable<Hedgerow> hedgerows, IReadOnlyList<FootpathLine> footpaths, Random random)
        {
            foreach (var hedgerow in hedgerows)
            {
                foreach (var footpath in footpaths) OpenForTheFootpath(hedgerow, footpath);
                OpenAGateway(hedgerow, random);
            }
        }

        private static void OpenForTheFootpath(Hedgerow hedgerow, FootpathLine footpath)
        {
            var isCrossed = footpath.IsCrossing(hedgerow, out var alongMetres, out var sine);
            if (!isCrossed) return;
            var halfWidth = FieldStrips.FootpathMetres * Half / Math.Max(sine, ShallowestSine);
            hedgerow.Open(new HedgeGap(alongMetres - halfWidth, alongMetres + halfWidth, GapFurniture.Stile));
        }

        private static void OpenAGateway(Hedgerow hedgerow, Random random)
        {
            var chance = hedgerow.IsFarmBoundary ? FarmGatewayChance : FieldGatewayChance;
            var room = hedgerow.LengthMetres - GatewayFromTheEndMetres * BothEnds - GatewayMetres;
            var isWanted = random.NextDouble() < chance;
            var from = GatewayFromTheEndMetres + random.NextDouble() * room;
            var to = from + GatewayMetres;
            var canFit = room > NoRoom && hedgerow.IsOpenBetween(from, to);
            if (!isWanted || !canFit) return;
            hedgerow.Open(new HedgeGap(from, to, GatewayFurniture(random)));
        }

        private static GapFurniture GatewayFurniture(Random random)
        {
            var roll = random.NextDouble();
            if (roll < ShutGateShare) return GapFurniture.ShutGate;
            if (roll < OpenGateShare) return GapFurniture.OpenGate;
            return GapFurniture.Nothing;
        }
    }
}
