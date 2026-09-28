using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public static class Understorey
    {
        private const double SpacingMetres = 12;
        private const double HalfACell = 0.5;
        private const double EdgeMetres = 25;
        private const double EdgeShare = 0.85;
        private const double InsideShare = 0.25;
        private const double SmallestScale = 0.75;
        private const double ScaleRange = 0.35;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int Seed = 1066;

        public static IEnumerable<Tree> Within(Wood wood)
        {
            var random = new Random(Seed);
            var places = Places(wood.Outline, random).Where(wood.IsBroadleafAt).ToList();
            return places.Where(place => IsHazelAt(wood, place, random)).Select(place => Hazel(place, random)).ToList();
        }

        private static bool IsHazelAt(Wood wood, GroundPoint place, Random random)
        {
            var isAtTheEdge = wood.EdgeDistanceFrom(place) < EdgeMetres;
            var share = isAtTheEdge ? EdgeShare : InsideShare;
            return random.NextDouble() < share;
        }

        private static Tree Hazel(GroundPoint place, Random random)
        {
            var scale = SmallestScale + random.NextDouble() * ScaleRange;
            return new Tree(place, scale, random.NextDouble() * FullTurnRadians, TreeForms.OneOf(HazelHabits.Coppice, random));
        }

        private static IEnumerable<GroundPoint> Places(GroundRing outline, Random random)
        {
            for (var east = outline.West + SpacingMetres * HalfACell; east < outline.East; east += SpacingMetres)
            {
                for (var north = outline.South + SpacingMetres * HalfACell; north < outline.North; north += SpacingMetres)
                {
                    var wander = new GroundPoint(random.NextDouble() - HalfACell, random.NextDouble() - HalfACell) * SpacingMetres;
                    yield return new GroundPoint(east, north) + wander;
                }
            }
        }
    }
}
