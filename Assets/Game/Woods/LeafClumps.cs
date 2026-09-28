using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Woods
{
    public static class LeafClumps
    {
        private const double BareShare = 0.3;
        private const double SizeJitter = 0.25;
        private const double Unshaded = 0;

        public static IReadOnlyList<LeafClump> Along(IEnumerable<Limb> twigs, Habit habit, Random random)
        {
            var clumps = new List<LeafClump>();
            foreach (var twig in twigs) clumps.AddRange(OnTheTwig(twig, habit, random));
            return CrownShading.Shaded(clumps);
        }

        private static IEnumerable<LeafClump> OnTheTwig(Limb twig, Habit habit, Random random)
        {
            var length = twig.LengthMetres;
            for (var metres = length; metres >= length * BareShare; metres -= habit.ClumpSpacingMetres)
            {
                var point = twig.At(metres / length);
                var size = habit.ClumpSizeMetres * (1 + (random.NextDouble() * 2 - 1) * SizeJitter);
                yield return new LeafClump(point.Place, size, point.Heading, Unshaded, random.NextDouble());
            }
        }
    }
}
