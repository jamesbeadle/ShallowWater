using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public sealed class TreeFigure
    {
        public TreeFigure(Shape wood, IReadOnlyList<LeafClump> leaves)
        {
            Wood = wood;
            Leaves = leaves;
        }

        public Shape Wood { get; }
        public IReadOnlyList<LeafClump> Leaves { get; }
    }
}
