using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class CrownShading
    {
        private const double UpwardLean = 0.3;
        private const double SmallestHalfSizeMetres = 0.5;
        private static readonly Offset BarelyUp = Offset.Up * 1e-6;

        public static IReadOnlyList<LeafClump> Shaded(IReadOnlyList<LeafClump> clumps)
        {
            var hasNoLeaves = !clumps.Any();
            if (hasNoLeaves) return clumps;
            var centres = clumps.Select(clump => clump.Centre).ToList();
            var easts = centres.Select(centre => centre.East).ToList();
            var heights = centres.Select(centre => centre.Height).ToList();
            var norths = centres.Select(centre => centre.North).ToList();
            var middle = new WorldPoint(Middle(easts), Middle(heights), Middle(norths));
            var halfSize = new Offset(HalfSize(easts), HalfSize(heights), HalfSize(norths));
            return clumps.Select(clump => Shaded(clump, middle, halfSize)).ToList();
        }

        private static LeafClump Shaded(LeafClump clump, WorldPoint middle, Offset halfSize)
        {
            var fromTheMiddle = Offset.Between(middle, clump.Centre);
            var withinTheCrown = Relative(fromTheMiddle, halfSize);
            var depth = Math.Max(0, 1 - withinTheCrown.Length);
            var outward = ((withinTheCrown + BarelyUp).Normalised + Offset.Up * UpwardLean).Normalised;
            return new LeafClump(clump.Centre, clump.SizeMetres, outward, depth, clump.Seed);
        }

        private static Offset Relative(Offset fromTheMiddle, Offset halfSize)
        {
            var east = fromTheMiddle.Eastward / halfSize.Eastward;
            var up = fromTheMiddle.Upward / halfSize.Upward;
            return new Offset(east, up, fromTheMiddle.Northward / halfSize.Northward);
        }

        private static double Middle(IReadOnlyList<double> values)
        {
            return (values.Min() + values.Max()) / 2;
        }

        private static double HalfSize(IReadOnlyList<double> values)
        {
            return Math.Max((values.Max() - values.Min()) / 2, SmallestHalfSizeMetres);
        }
    }
}
