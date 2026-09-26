using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Pound
{
    public static class CanalSection
    {
        private const double SoftEdgeWidthMetres = 0.6;
        private const double StoneFaceWidthMetres = 0;
        private const double CopingWidthMetres = 0.5;
        private const double DirtPathFromCentreMetres = 7.4;
        private const double DirtPathWidthMetres = 1.8;
        private const double PavedPathWidthMetres = 2.8;
        private const double GrassFromCentreMetres = 13;
        private const double OuterSlopeWidthMetres = 1.5;
        private const double BothSides = 2;

        public const double OuterReachMetres = GrassFromCentreMetres + OuterSlopeWidthMetres;

        public static IReadOnlyList<Band> For(Stretch stretch)
        {
            var water = Band.Flat(Surface.Water, PoundLimits.ChannelHalfWidthMetres * BothSides, Heights.WaterMetres);
            var bands = new List<Band>(TowpathSide(stretch)) { water };
            if (stretch.IsAtAnArmMouth) return bands;
            bands.AddRange(Offside());
            return bands;
        }

        public static Band BeyondThePound()
        {
            var width = PoundLimits.ChannelHalfWidthMetres * BothSides;
            return Band.Flat(Surface.Water, width, Heights.CanalsBeyondThePoundMetres);
        }

        private static SectionWalk FromTheWater(double side)
        {
            return new SectionWalk(side, PoundLimits.ChannelHalfWidthMetres, Heights.WaterMetres);
        }

        private static IReadOnlyList<Band> TowpathSide(Stretch stretch)
        {
            var walk = FromTheWater(PoundLimits.TowpathSide);
            WaterEdge(walk, stretch.HasHardEdge);
            Path(walk, stretch.IsVillage);
            GrassVerge(walk);
            return walk.Bands;
        }

        private static IReadOnlyList<Band> Offside()
        {
            var walk = FromTheWater(-PoundLimits.TowpathSide);
            walk.Step(Surface.Bank, SoftEdgeWidthMetres, Heights.BankTopMetres);
            GrassVerge(walk);
            return walk.Bands;
        }

        private static void WaterEdge(SectionWalk walk, bool isHard)
        {
            if (!isHard)
            {
                walk.Step(Surface.Bank, SoftEdgeWidthMetres, Heights.BankTopMetres);
                return;
            }
            walk.Step(Surface.Coping, StoneFaceWidthMetres, Heights.BankTopMetres);
            walk.Step(Surface.Coping, CopingWidthMetres, Heights.BankTopMetres);
        }

        private static void Path(SectionWalk walk, bool isPaved)
        {
            if (isPaved)
            {
                walk.Step(Surface.PavedTowpath, PavedPathWidthMetres, Heights.BankTopMetres);
                return;
            }
            walk.StepOutTo(Surface.Grass, DirtPathFromCentreMetres, Heights.BankTopMetres);
            walk.Step(Surface.Towpath, DirtPathWidthMetres, Heights.BankTopMetres);
        }

        private static void GrassVerge(SectionWalk walk)
        {
            walk.StepOutTo(Surface.Grass, GrassFromCentreMetres, Heights.BankTopMetres);
            walk.Step(Surface.Bank, OuterSlopeWidthMetres, Heights.GroundMetres);
        }
    }
}
