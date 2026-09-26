using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Pound
{
    public static class CanalSection
    {
        private const double LipWidthMetres = 0.5;
        private const double TowpathWidthMetres = 3;
        private const double OffsideBankWidthMetres = 1.5;
        private const double OuterSlopeWidthMetres = 1;

        public static IReadOnlyList<Band> OfThePound()
        {
            var water = PoundLimits.ChannelHalfWidthMetres;
            var lip = water + LipWidthMetres;
            var towpath = lip + TowpathWidthMetres;
            var offsideBank = lip + OffsideBankWidthMetres;
            return new[]
            {
                Rising(Surface.Bank, -towpath - OuterSlopeWidthMetres, Heights.GroundMetres, -towpath, Heights.BankTopMetres),
                Rising(Surface.Towpath, -towpath, Heights.BankTopMetres, -lip, Heights.BankTopMetres),
                Rising(Surface.Bank, -lip, Heights.BankTopMetres, -water, Heights.WaterMetres),
                Rising(Surface.Water, -water, Heights.WaterMetres, water, Heights.WaterMetres),
                Rising(Surface.Bank, water, Heights.WaterMetres, lip, Heights.BankTopMetres),
                Rising(Surface.Bank, lip, Heights.BankTopMetres, offsideBank, Heights.BankTopMetres),
                Rising(Surface.Bank, offsideBank, Heights.BankTopMetres, offsideBank + OuterSlopeWidthMetres, Heights.GroundMetres),
            };
        }

        public static Band BeyondThePound()
        {
            var width = PoundLimits.ChannelHalfWidthMetres * 2;
            return Band.Flat(Surface.Water, width, Heights.CanalsBeyondThePoundMetres);
        }

        private static Band Rising(Surface surface, double fromOffset, double fromHeight, double toOffset, double toHeight)
        {
            return new Band(surface, new BandEdge(fromOffset, fromHeight), new BandEdge(toOffset, toHeight));
        }
    }
}
