using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.Roads
{
    public static class RoadHedges
    {
        private const double PieceMetres = 60;

        public static IEnumerable<Hedge> Along(IEnumerable<Road> roads, RoadSurroundings surroundings)
        {
            return roads.SelectMany(road => RoadShapes.Sides.SelectMany(side => Beside(road, side, surroundings)));
        }

        private static IEnumerable<Hedge> Beside(Road road, double side, RoadSurroundings surroundings)
        {
            foreach (var piece in RoadsideRuns.Along(road, side, surroundings))
            {
                var margin = RoadSection.MarginOf(road, piece.Roadside);
                foreach (var span in piece.Span.Pieces(PieceMetres))
                {
                    yield return new Hedge(road.Beside(span, side * margin.HedgeCentreMetres), margin.HedgeHalfWidthMetres);
                }
            }
        }
    }
}
