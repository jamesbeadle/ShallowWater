using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Fields
{
    public sealed class FieldPlan
    {
        private const int Seed = 1938;

        private readonly List<Field> fields = new List<Field>();
        private readonly List<Hedgerow> hedgerows = new List<Hedgerow>();
        private readonly Farmland farmland;
        private readonly Random random = new Random(Seed);

        private FieldPlan(Farmland farmland)
        {
            this.farmland = farmland;
        }

        public IReadOnlyList<Field> Fields => fields;
        public IReadOnlyList<Hedgerow> Hedgerows => hedgerows;

        public static FieldPlan Of(Farmland farmland)
        {
            var plan = new FieldPlan(farmland);
            var farms = FarmBlocks.Across(farmland, plan.random);
            var farmIndexes = new HashSet<int>(farms.Select(farm => farm.Index));
            foreach (var farm in farms) plan.LayOut(farm, farmIndexes);
            HedgeGaps.Open(plan.hedgerows, FootpathLines.Of(farms), plan.random);
            return plan;
        }

        private void LayOut(Farm farm, HashSet<int> farmIndexes)
        {
            var division = FieldDivision.Of(farm, random);
            var outlines = division.Fields.SelectMany(outline => FieldsCutByLines.Cut(outline, farmland.FollowedLines));
            foreach (var outline in outlines) Sow(farm, outline);
            foreach (var chord in division.Chords) hedgerows.Add(Hedgerow.WithinAFarm(chord.From, chord.To));
            HedgeTheBoundary(farm, farmIndexes);
        }

        private void Sow(Farm farm, ConvexOutline outline)
        {
            var centroid = outline.Centroid();
            if (!farmland.IsFarmed(centroid)) return;
            var crop = Cropping.For(farmland.IsWaterMeadow(centroid), random);
            fields.Add(new Field(outline, crop, outline.LongerOf(farm.Grain)));
        }

        private void HedgeTheBoundary(Farm farm, HashSet<int> farmIndexes)
        {
            var outline = farm.Outline;
            for (var edge = 0; edge < outline.EdgeCount; edge++)
            {
                var boundary = outline.Boundaries[edge];
                var isHedgedFromThisSide = !farmIndexes.Contains(boundary.Neighbour) || farm.Index < boundary.Neighbour;
                if (!boundary.HasHedge || !isHedgedFromThisSide) continue;
                hedgerows.Add(Hedgerow.BetweenFarms(outline.EdgeStart(edge), outline.EdgeEnd(edge)));
            }
        }
    }
}
