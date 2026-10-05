using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Fields
{
    public static class FieldsCutByLines
    {
        private const double SmallestPieceSquareMetres = 500;

        public static IEnumerable<ConvexOutline> Cut(ConvexOutline field, IEnumerable<FollowedLine> lines)
        {
            var pieces = new List<ConvexOutline> { field };
            foreach (var line in lines) pieces = pieces.SelectMany(piece => CutBy(piece, line)).ToList();
            return pieces.Where(piece => piece.Area >= SmallestPieceSquareMetres);
        }

        private static List<ConvexOutline> CutBy(ConvexOutline field, FollowedLine line)
        {
            var pieces = new List<ConvexOutline> { field };
            foreach (var pass in LinePass.Through(field, line))
            {
                if (!pass.IsStraightWithin(line.CoverMetres)) continue;
                pieces = pieces.SelectMany(piece => CutWhereItPasses(piece, pass)).ToList();
            }
            return pieces;
        }

        private static IEnumerable<ConvexOutline> CutWhereItPasses(ConvexOutline piece, LinePass pass)
        {
            if (!piece.IsAround(pass.Middle)) return new[] { piece };
            var cutting = pass.CuttingLine;
            var halves = new[] { OutlineCut.KeepRight(piece, cutting, Boundary.Open), OutlineCut.KeepRight(piece, cutting.Reversed, Boundary.Open) };
            return halves.Where(half => half.IsDrawable);
        }
    }
}
