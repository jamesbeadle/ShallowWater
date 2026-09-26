using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Roads
{
    public static class RoadShapes
    {
        private const double TelegraphSide = -1;
        private static readonly double[] Sides = { -1, 1 };

        public static SurfaceShapes Of(IEnumerable<Road> roads, RoadSurroundings surroundings)
        {
            var surfaces = new SurfaceShapes();
            foreach (var road in roads) Lay(surfaces, road, surroundings);
            return surfaces;
        }

        private static void Lay(SurfaceShapes surfaces, Road road, RoadSurroundings surroundings)
        {
            surfaces.AddRibbon(road.Between(road.Whole), RoadSection.Carriageway(road));
            if (road.HasCentreLine) CentreLines.Paint(surfaces, road);
            foreach (var side in Sides) Border(surfaces, road, side, surroundings);
        }

        private static void Border(SurfaceShapes surfaces, Road road, double side, RoadSurroundings surroundings)
        {
            foreach (var piece in RoadsideRuns.Along(road, side, surroundings)) Edge(surfaces, road, side, piece);
        }

        private static void Edge(SurfaceShapes surfaces, Road road, double side, RoadsidePiece piece)
        {
            var line = road.Between(piece.Span);
            var bands = RoadSection.Beside(road, side, piece.Roadside);
            foreach (var band in bands) surfaces.AddRibbon(line, band);
            HedgeEnds.Close(surfaces, line, bands, side);
            var hasPoles = road.HasTelegraphPoles && side == TelegraphSide && piece.Roadside == Roadside.Hedgerow;
            if (hasPoles) TelegraphPoles.StandAlong(surfaces, road, piece.Span, side);
        }
    }
}
