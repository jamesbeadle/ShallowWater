namespace ShallowWater.Game.Woods
{
    public static class BirchHabits
    {
        public static readonly Habit Mature = new Habit
        {
            Species = TreeSpecies.Birch, HeightMetres = 16, TrunkRadiusMetres = 0.18, TrunkShare = 1, LeanRadians = 0.12, FlareShare = 0.3,
            Trunk = new BranchLevel { Segments = 12, Kink = 0.07, Rise = 0.02, TipShare = 0.12 },
            Outline = CrownOutline.Ovoid,
            Levels = new[]
            {
                new BranchLevel
                {
                    Count = 16, FromShare = 0.32, AngleDegrees = 40, AngleSpreadDegrees = 12, LengthShare = 0.3,
                    RadiusShare = 0.35, Rise = -0.03, Kink = 0.15, Segments = 5
                },
                new BranchLevel
                {
                    Count = 4, FromShare = 0.3, AngleDegrees = 35, AngleSpreadDegrees = 12, LengthShare = 0.5,
                    RadiusShare = 0.4, Rise = -0.12, Kink = 0.2, Segments = 4
                },
                new BranchLevel
                {
                    Count = 3, FromShare = 0.35, AngleDegrees = 25, AngleSpreadDegrees = 12, LengthShare = 0.55,
                    RadiusShare = 0.5, Rise = -0.3, Kink = 0.15, Segments = 3
                }
            },
            ClumpSpacingMetres = 0.7, ClumpSizeMetres = 1.25
        };

        public static readonly Habit Young = new Habit
        {
            Species = TreeSpecies.Birch, IsYoung = true, HeightMetres = 10, TrunkRadiusMetres = 0.09, TrunkShare = 1, LeanRadians = 0.15, FlareShare = 0.25,
            Trunk = new BranchLevel { Segments = 10, Kink = 0.08, Rise = 0.02, TipShare = 0.12 },
            Outline = CrownOutline.Ovoid,
            Levels = new[]
            {
                new BranchLevel
                {
                    Count = 13, FromShare = 0.3, AngleDegrees = 38, AngleSpreadDegrees = 12, LengthShare = 0.3,
                    RadiusShare = 0.35, Rise = -0.03, Kink = 0.15, Segments = 4
                },
                new BranchLevel
                {
                    Count = 3, FromShare = 0.3, AngleDegrees = 35, AngleSpreadDegrees = 12, LengthShare = 0.55,
                    RadiusShare = 0.4, Rise = -0.12, Kink = 0.2, Segments = 3
                },
                new BranchLevel
                {
                    Count = 3, FromShare = 0.35, AngleDegrees = 25, AngleSpreadDegrees = 12, LengthShare = 0.55,
                    RadiusShare = 0.5, Rise = -0.3, Kink = 0.15, Segments = 3
                }
            },
            ClumpSpacingMetres = 0.6, ClumpSizeMetres = 1.05
        };
    }
}
