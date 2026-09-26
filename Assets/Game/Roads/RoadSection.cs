using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Roads
{
    public static class RoadSection
    {
        private const double VergeRiseMetres = 0.03;
        private const double HedgeFaceMetres = 0.2;
        private const double HedgeTopMetres = 0.8;
        private const double HedgeHeightMetres = 1.8;
        private const double KerbFaceMetres = 0;
        private const double KerbHeightMetres = 0.12;
        private const double PavementWidthMetres = 1.8;
        private const double FrontGardenWidthMetres = 2.4;
        private const double GardenHedgeFaceMetres = 0.1;
        private const double GardenHedgeTopMetres = 0.4;
        private const double GardenHedgeHeightMetres = 1;
        private const double HedgeRoughnessMetres = 0.35;
        private const double GardenHedgeRoughnessMetres = 0.1;
        private const double MeasuringSide = 1;

        public const int HedgeBandCount = 3;

        public static Band Carriageway(Road road)
        {
            return Band.Flat(road.Surface, road.WidthMetres, road.HeightMetres);
        }

        public static IReadOnlyList<Band> Beside(Road road, double side, Roadside roadside)
        {
            var walk = WalkOut(road, side, roadside);
            return walk.Bands;
        }

        public static double ReachOf(Road road, Roadside roadside)
        {
            var walk = WalkOut(road, MeasuringSide, roadside);
            return walk.DistanceFromCentre;
        }

        public static double VergeHeightOf(Road road)
        {
            return road.HeightMetres + VergeRiseMetres;
        }

        private static SectionWalk WalkOut(Road road, double side, Roadside roadside)
        {
            var walk = new SectionWalk(side, road.HalfWidthMetres, road.HeightMetres);
            if (roadside == Roadside.Street) Street(walk);
            if (roadside == Roadside.Hedgerow) Hedgerow(walk, road);
            return walk;
        }

        private static void Hedgerow(SectionWalk walk, Road road)
        {
            var verge = VergeHeightOf(road);
            walk.Step(Surface.Grass, road.VergeWidthMetres, verge);
            var hedge = new HedgeShape(HedgeFaceMetres, HedgeTopMetres, verge + HedgeHeightMetres, HedgeRoughnessMetres);
            Hedge(walk, Surface.Hedge, hedge);
        }

        private static void Street(SectionWalk walk)
        {
            var kerb = walk.Height + KerbHeightMetres;
            walk.Step(Surface.Kerb, KerbFaceMetres, kerb);
            walk.Step(Surface.Pavement, PavementWidthMetres, kerb);
            walk.Step(Surface.Grass, FrontGardenWidthMetres, kerb);
            var gardenHedge = new HedgeShape(GardenHedgeFaceMetres, GardenHedgeTopMetres, kerb + GardenHedgeHeightMetres, GardenHedgeRoughnessMetres);
            Hedge(walk, Surface.GardenHedge, gardenHedge);
        }

        private static void Hedge(SectionWalk walk, Surface surface, HedgeShape hedge)
        {
            walk.StepToRough(surface, hedge.FaceMetres, hedge.TopHeightMetres, hedge.RoughnessMetres);
            walk.StepToRough(surface, hedge.TopMetres, hedge.TopHeightMetres, hedge.RoughnessMetres);
            walk.Step(surface, hedge.FaceMetres, Heights.GroundMetres);
        }
    }
}
