using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class TreeSurfaces
    {
        private static readonly Dictionary<TreeSpecies, Surface> Barks = new Dictionary<TreeSpecies, Surface>
        {
            { TreeSpecies.Oak, Surface.OakBark }, { TreeSpecies.Ash, Surface.AshBark },
            { TreeSpecies.Birch, Surface.BirchBark }, { TreeSpecies.ScotsPine, Surface.PineBark },
            { TreeSpecies.Hazel, Surface.HazelBark }
        };

        private static readonly Dictionary<TreeSpecies, Surface> Foliage = new Dictionary<TreeSpecies, Surface>
        {
            { TreeSpecies.Oak, Surface.OakLeaves }, { TreeSpecies.Ash, Surface.AshLeaves },
            { TreeSpecies.Birch, Surface.BirchLeaves }, { TreeSpecies.ScotsPine, Surface.PineNeedles },
            { TreeSpecies.Hazel, Surface.HazelLeaves }
        };

        public static Surface BarkOf(TreeSpecies species)
        {
            return Barks[species];
        }

        public static Surface LeavesOf(TreeSpecies species)
        {
            return Foliage[species];
        }
    }
}
