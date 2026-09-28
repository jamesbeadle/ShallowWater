using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public static class TreeScatter
    {
        private const double SpacingMetres = 10;
        private const double Wander = 0.8;
        private const double SmallestScale = 0.75;
        private const double ScaleRange = 0.5;
        private const int Seed = 1938;
        private const int ShapingSeed = 1939;
        private const double Middle = 0.5;
        private const double FullTurnRadians = 2 * Math.PI;
        private const double StoutestCrown = 0.8;
        private const double SlendernessRange = 0.45;

        public static IEnumerable<Tree> Within(Area area)
        {
            var random = new Random(Seed);
            var shaping = new Random(ShapingSeed);
            foreach (var corner in CellCornersUnder(area.Outline))
            {
                var tree = WanderedTree(corner, random, shaping);
                if (area.IsAround(tree.Position)) yield return tree;
            }
        }

        private static IEnumerable<GroundPoint> CellCornersUnder(GroundRing outline)
        {
            var firstColumn = (int)Math.Floor(outline.West / SpacingMetres);
            var lastColumn = (int)Math.Ceiling(outline.East / SpacingMetres);
            var firstRow = (int)Math.Floor(outline.South / SpacingMetres);
            var lastRow = (int)Math.Ceiling(outline.North / SpacingMetres);
            for (var column = firstColumn; column <= lastColumn; column++)
            {
                for (var row = firstRow; row <= lastRow; row++) yield return new GroundPoint(column * SpacingMetres, row * SpacingMetres);
            }
        }

        private static Tree WanderedTree(GroundPoint corner, Random random, Random shaping)
        {
            var east = (random.NextDouble() - Middle) * Wander * SpacingMetres;
            var north = (random.NextDouble() - Middle) * Wander * SpacingMetres;
            var scale = SmallestScale + random.NextDouble() * ScaleRange;
            var turn = shaping.NextDouble() * FullTurnRadians;
            var slenderness = StoutestCrown + shaping.NextDouble() * SlendernessRange;
            return new Tree(corner + new GroundPoint(east, north), scale, turn, slenderness);
        }
    }
}
