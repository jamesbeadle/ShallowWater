using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Walking;
using ShallowWater.Game.Woods;

namespace ShallowWater.Game.Fields
{
    public sealed class CountrysideShapes
    {
        private const int Seed = 1939;

        private readonly List<Tree> standards = new List<Tree>();
        private readonly List<Hedge> hedges = new List<Hedge>();
        private readonly HedgerowTiles hedgerows = new HedgerowTiles();

        private CountrysideShapes()
        {
        }

        public SurfaceShapes Fields { get; } = new SurfaceShapes();
        public IEnumerable<SurfaceShapes> HedgerowTiles => hedgerows.All;
        public IReadOnlyList<Tree> Standards => standards;
        public IReadOnlyList<Hedge> Hedges => hedges;

        public static CountrysideShapes Of(FieldPlan plan, Farmland farmland)
        {
            var countryside = new CountrysideShapes();
            foreach (var field in plan.Fields) FieldShapes.Add(countryside.Fields, field);
            var random = new Random(Seed);
            foreach (var hedgerow in plan.Hedgerows) countryside.Plant(hedgerow, farmland, random);
            return countryside;
        }

        private void Plant(Hedgerow hedgerow, Farmland farmland, Random random)
        {
            var tile = hedgerows.TileOf(hedgerow);
            foreach (var run in HedgeRuns.Of(hedgerow, farmland)) Grow(tile, hedgerow, run, random);
            foreach (var gap in hedgerow.Gaps) GapFurnishing.Add(tile, hedgerow, gap, farmland, random);
        }

        private void Grow(SurfaceShapes tile, Hedgerow hedgerow, HedgeStretch run, Random random)
        {
            var line = HedgerowShapes.AddRun(tile, hedgerow, run);
            hedges.Add(new Hedge(line, FieldHedgeSection.HalfWidthMetres));
            standards.AddRange(HedgerowTrees.Along(hedgerow, run, random));
        }
    }
}
