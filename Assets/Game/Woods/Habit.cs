namespace ShallowWater.Game.Woods
{
    public sealed class Habit
    {
        private const double UsualFlareShare = 0.45;
        private const double UsualForkRadiusShare = 0.75;

        public TreeSpecies Species { get; set; }
        public bool IsYoung { get; set; }
        public double HeightMetres { get; set; }
        public double TrunkRadiusMetres { get; set; }
        public double TrunkShare { get; set; }
        public double LeanRadians { get; set; }
        public double FlareShare { get; set; } = UsualFlareShare;
        public BranchLevel Trunk { get; set; }
        public int ForkLimbs { get; set; }
        public double ForkAngleDegrees { get; set; }
        public double ForkLengthShare { get; set; }
        public double ForkRadiusShare { get; set; } = UsualForkRadiusShare;
        public CrownOutline Outline { get; set; }
        public BranchLevel[] Levels { get; set; }
        public double ClumpSpacingMetres { get; set; }
        public double ClumpSizeMetres { get; set; }
        public int LeafOrder => Levels.Length;
    }
}
