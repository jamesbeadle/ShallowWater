using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Woods;

namespace ShallowWater.Game.Fields
{
    public sealed class CountrysideShapes
    {
        private const int Seed = 1939;

        private readonly List<Tree> hedgerowTrees = new List<Tree>();
        private readonly HedgerowTiles hedgerows = new HedgerowTiles();

        private CountrysideShapes()
        {
        }

        public SurfaceShapes Fields { get; } = new SurfaceShapes();
        public IEnumerable<SurfaceShapes> HedgerowTiles => hedgerows.All;
        public IReadOnlyList<Tree> HedgerowTrees => hedgerowTrees;

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
            HedgerowShapes.Add(tile, hedgerowTrees, hedgerow, farmland, random);
        }
    }
}
