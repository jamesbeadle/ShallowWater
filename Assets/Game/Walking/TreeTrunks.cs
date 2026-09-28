using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Woods;

namespace ShallowWater.Game.Walking
{
    public static class TreeTrunks
    {
        public static IEnumerable<Trunk> Standing(IEnumerable<Tree> trees)
        {
            return trees.Where(HasATrunk).Select(TrunkOf);
        }

        private static bool HasATrunk(Tree tree)
        {
            var form = TreeForms.All[tree.Form];
            return form.Species != TreeSpecies.Hazel;
        }

        private static Trunk TrunkOf(Tree tree)
        {
            var form = TreeForms.All[tree.Form];
            return new Trunk(tree.Position, form.TrunkRadiusMetres * tree.Scale);
        }
    }
}
