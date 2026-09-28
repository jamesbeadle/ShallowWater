namespace ShallowWater.Game.Woods
{
    public static class AshHabits
    {
        public static readonly Habit Woodland = new Habit
        {
            Species = TreeSpecies.Ash, HeightMetres = 22, TrunkRadiusMetres = 0.32, TrunkShare = 0.55, LeanRadians = 0.06,
            Trunk = new BranchLevel { Segments = 10, Kink = 0.03, Rise = 0.02, TipShare = 0.65 },
            ForkLimbs = 3, ForkAngleDegrees = 24, ForkLengthShare = 0.45, Outline = CrownOutline.Dome,
            Levels = new[]
            {
                new BranchLevel
                {
                    Count = 3, FromShare = 0.6, AngleDegrees = 45, AngleSpreadDegrees = 10, LengthShare = 0.8,
                    RadiusShare = 0.45, Rise = 0.08, Kink = 0.15, Segments = 6
                },
                new BranchLevel
                {
                    Count = 5, FromShare = 0.3, AngleDegrees = 42, AngleSpreadDegrees = 12, LengthShare = 0.45,
                    RadiusShare = 0.45, Rise = 0.12, Kink = 0.2, Segments = 4
                },
                new BranchLevel
                {
                    Count = 3, FromShare = 0.35, AngleDegrees = 40, AngleSpreadDegrees = 15, LengthShare = 0.45,
                    RadiusShare = 0.5, Rise = 0.12, Kink = 0.2, Segments = 2
                }
            },
            ClumpSpacingMetres = 1.2, ClumpSizeMetres = 1.8
        };

        public static readonly Habit Young = new Habit
        {
            Species = TreeSpecies.Ash, IsYoung = true, HeightMetres = 13, TrunkRadiusMetres = 0.13, TrunkShare = 0.8, LeanRadians = 0.08,
            Trunk = new BranchLevel { Segments = 10, Kink = 0.04, Rise = 0.02, TipShare = 0.4 },
            ForkLimbs = 2, ForkAngleDegrees = 18, ForkLengthShare = 0.25, Outline = CrownOutline.Ovoid,
            Levels = new[]
            {
                new BranchLevel
                {
                    Count = 7, FromShare = 0.45, AngleDegrees = 42, AngleSpreadDegrees = 10, LengthShare = 0.35,
                    RadiusShare = 0.4, Rise = 0.1, Kink = 0.15, Segments = 5
                },
                new BranchLevel
                {
                    Count = 4, FromShare = 0.3, AngleDegrees = 40, AngleSpreadDegrees = 12, LengthShare = 0.5,
                    RadiusShare = 0.45, Rise = 0.12, Kink = 0.2, Segments = 3
                },
                new BranchLevel
                {
                    Count = 3, FromShare = 0.35, AngleDegrees = 40, AngleSpreadDegrees = 15, LengthShare = 0.5,
                    RadiusShare = 0.5, Rise = 0.12, Kink = 0.2, Segments = 2
                }
            },
            ClumpSpacingMetres = 1.0, ClumpSizeMetres = 1.4
        };
    }
}
