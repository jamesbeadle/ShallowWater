namespace ShallowWater.Game.Woods
{
    public static class HazelHabits
    {
        public static readonly Habit Coppice = new Habit
        {
            Species = TreeSpecies.Hazel, HeightMetres = 5.5, TrunkRadiusMetres = 0.2, TrunkShare = 0.05, LeanRadians = 0.05, FlareShare = 0.2,
            Trunk = new BranchLevel { Segments = 2, Kink = 0.02, Rise = 0, TipShare = 0.9 },
            ForkLimbs = 7, ForkAngleDegrees = 20, ForkLengthShare = 0.85, ForkRadiusShare = 0.22, Outline = CrownOutline.Dome,
            Levels = new[]
            {
                new BranchLevel
                {
                    Count = 4, FromShare = 0.35, AngleDegrees = 45, AngleSpreadDegrees = 12,
                    LengthShare = 0.35, RadiusShare = 0.5, Rise = 0.04, Kink = 0.2, Segments = 5, TipShare = 0.35
                },
                new BranchLevel
                {
                    Count = 3, FromShare = 0.3, AngleDegrees = 40, AngleSpreadDegrees = 15,
                    LengthShare = 0.5, RadiusShare = 0.5, Rise = 0.02, Kink = 0.25, Segments = 3
                }
            },
            ClumpSpacingMetres = 0.55, ClumpSizeMetres = 1.25
        };
    }
}
