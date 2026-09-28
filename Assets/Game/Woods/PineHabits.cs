namespace ShallowWater.Game.Woods
{
    public static class PineHabits
    {
        public static readonly Habit Plantation = new Habit
        {
            Species = TreeSpecies.ScotsPine, HeightMetres = 21, TrunkRadiusMetres = 0.25, TrunkShare = 1, LeanRadians = 0.04, FlareShare = 0.35,
            Trunk = new BranchLevel { Segments = 12, Kink = 0.03, Rise = 0.02, TipShare = 0.15 },
            Outline = CrownOutline.FlatTop,
            Levels = new[]
            {
                new BranchLevel
                {
                    Count = 20, PerWhorl = 4, FromShare = 0.62, AngleDegrees = 72, AngleSpreadDegrees = 12,
                    LengthShare = 0.2, RadiusShare = 0.3, Rise = 0.05, Kink = 0.2, Segments = 5
                },
                new BranchLevel
                {
                    Count = 6, FromShare = 0.25, AngleDegrees = 50, AngleSpreadDegrees = 15, LengthShare = 0.4,
                    RadiusShare = 0.45, Rise = 0.1, Kink = 0.2, Segments = 3
                }
            },
            ClumpSpacingMetres = 0.6, ClumpSizeMetres = 1.35
        };
    }
}
