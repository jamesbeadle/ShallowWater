using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Woods;

namespace ShallowWater.Game.Fields
{
    public static class HedgerowTrees
    {
        private const double FarmHedgeSpacingMetres = 30;
        private const double FieldHedgeSpacingMetres = 55;
        private const double SpacingSpread = 1.4;
        private const double ClearOfTheEndsMetres = 4;
        private const double OakShare = 0.82;
        private const double SmallestScale = 0.85;
        private const double ScaleRange = 0.35;
        private const double FullTurnRadians = 2 * Math.PI;

        public static IEnumerable<Tree> Along(Hedgerow hedgerow, HedgeStretch run, Random random)
        {
            var spacing = hedgerow.IsFarmBoundary ? FarmHedgeSpacingMetres : FieldHedgeSpacingMetres;
            var along = run.FromMetres + random.NextDouble() * spacing;
            for (; along < run.ToMetres - ClearOfTheEndsMetres; along += spacing * (1 + random.NextDouble() * SpacingSpread))
            {
                if (along < run.FromMetres + ClearOfTheEndsMetres) continue;
                yield return Standard(hedgerow.At(along), random);
            }
        }

        private static Tree Standard(GroundPoint place, Random random)
        {
            var habit = random.NextDouble() < OakShare ? OakHabits.OpenGrown : AshHabits.Woodland;
            var scale = SmallestScale + random.NextDouble() * ScaleRange;
            return new Tree(place, scale, random.NextDouble() * FullTurnRadians, TreeForms.OneOf(habit, random));
        }
    }
}
