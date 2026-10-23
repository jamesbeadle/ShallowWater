using System.Collections.Generic;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.Roads
{
    public static class RoadBeds
    {
        private const double PieceMetres = 60;
        private const double OnTheCentreline = 0;

        public static IEnumerable<FloorStrip> Along(IEnumerable<Road> roads, RoadSurroundings surroundings)
        {
            foreach (var road in roads)
            {
                foreach (var span in road.Whole.Pieces(PieceMetres)) yield return new FloorStrip(road.Beside(span, OnTheCentreline), road.HalfWidthMetres, road.HeightMetres);
                foreach (var side in RoadShapes.Sides)
                {
                    foreach (var strip in Margins(road, side, surroundings)) yield return strip;
                }
            }
        }

        private static IEnumerable<FloorStrip> Margins(Road road, double side, RoadSurroundings surroundings)
        {
            foreach (var piece in RoadsideRuns.Along(road, side, surroundings))
            {
                var margin = RoadSection.MarginOf(road, piece.Roadside);
                foreach (var span in piece.Span.Pieces(PieceMetres))
                {
                    yield return new FloorStrip(road.Beside(span, side * margin.MiddleMetres), margin.HalfWidthMetres, margin.HeightMetres);
                }
            }
        }
    }
}
