using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class TreeFigures
    {
        private const double ThinnestNearLimbMetres = 0.012;
        private const double ThinnestMiddleLimbMetres = 0.06;
        private const int EveryRing = 1;
        private const int EverySecondRing = 2;
        private const int EveryFourthRing = 4;
        private const double MiddleCellClumps = 1.5;
        private const double FarCellClumps = 3.5;
        private static readonly LimbDetail NearWood = new LimbDetail(new[] { 10, 6, 4 }, ThinnestNearLimbMetres, EveryRing);
        private static readonly LimbDetail MiddleWood = new LimbDetail(new[] { 7, 4 }, ThinnestMiddleLimbMetres, EverySecondRing);
        private static readonly LimbDetail FarWood = new LimbDetail(new[] { 5 }, ThinnestMiddleLimbMetres, EveryFourthRing);

        public static IReadOnlyDictionary<DetailLevel, TreeFigure> Of(TreeForm form)
        {
            var habit = form.Habit;
            var limbs = TreeGrowth.Grown(habit, form.Seed);
            var wood = limbs.Where(limb => limb.Order < habit.LeafOrder).ToList();
            var twigs = limbs.Where(limb => limb.Order == habit.LeafOrder);
            var leaves = LeafClumps.Along(twigs, habit, new Random(form.Seed));
            var clumpSize = habit.ClumpSizeMetres;
            return new Dictionary<DetailLevel, TreeFigure>
            {
                { DetailLevel.Near, new TreeFigure(Wood(wood, NearWood), leaves) },
                { DetailLevel.Middle, new TreeFigure(Wood(wood, MiddleWood), ClumpMerging.Into(leaves, clumpSize * MiddleCellClumps)) },
                { DetailLevel.Far, new TreeFigure(Wood(wood, FarWood), ClumpMerging.Into(leaves, clumpSize * FarCellClumps)) }
            };
        }

        private static Shape Wood(IEnumerable<Limb> limbs, LimbDetail detail)
        {
            var wood = new Shape();
            foreach (var limb in limbs.Where(detail.IsShown).Select(detail.Coarsened))
            {
                wood.Append(TaperedTube.Along(limb.Path, limb.RadiiMetres, detail.SidesOf(limb)));
            }
            return wood;
        }
    }
}
