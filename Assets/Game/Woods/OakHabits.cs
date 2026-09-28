namespace ShallowWater.Game.Woods
{
    public static class OakHabits
    {
        private static readonly BranchLevel OakTwigs = new BranchLevel
        {
            Count = 5, FromShare = 0.25, AngleDegrees = 45, AngleSpreadDegrees = 20,
            LengthShare = 0.5, RadiusShare = 0.5, Rise = 0.05, Kink = 0.3, Segments = 2
        };

        public static readonly Habit Woodland = new Habit
        {
            Species = TreeSpecies.Oak, HeightMetres = 19, TrunkRadiusMetres = 0.42, TrunkShare = 0.34, LeanRadians = 0.08,
            Trunk = new BranchLevel { Segments = 10, Kink = 0.05, Rise = 0.01, TipShare = 0.8 },
            ForkLimbs = 4, ForkAngleDegrees = 36, ForkLengthShare = 0.5, Outline = CrownOutline.Dome,
            Levels = new[]
            {
                new BranchLevel
                {
                    Count = 2, FromShare = 0.55, AngleDegrees = 62, AngleSpreadDegrees = 10, LengthShare = 1.1,
                    RadiusShare = 0.5, Rise = 0.05, Kink = 0.3, Segments = 7
                },
                new BranchLevel
                {
                    Count = 7, FromShare = 0.2, AngleDegrees = 50, AngleSpreadDegrees = 15, LengthShare = 0.5,
                    RadiusShare = 0.45, Rise = 0.04, Kink = 0.35, Segments = 4
                },
                OakTwigs
            },
            ClumpSpacingMetres = 0.9, ClumpSizeMetres = 1.9
        };

        public static readonly Habit Young = new Habit
        {
            Species = TreeSpecies.Oak, IsYoung = true, HeightMetres = 12, TrunkRadiusMetres = 0.17, TrunkShare = 0.7, LeanRadians = 0.1,
            Trunk = new BranchLevel { Segments = 9, Kink = 0.06, Rise = 0.02, TipShare = 0.45 },
            ForkLimbs = 2, ForkAngleDegrees = 25, ForkLengthShare = 0.35, Outline = CrownOutline.Ovoid,
            Levels = new[]
            {
                new BranchLevel
                {
                    Count = 7, FromShare = 0.4, AngleDegrees = 55, AngleSpreadDegrees = 12, LengthShare = 0.45,
                    RadiusShare = 0.45, Rise = 0.05, Kink = 0.3, Segments = 5
                },
                new BranchLevel
                {
                    Count = 5, FromShare = 0.25, AngleDegrees = 48, AngleSpreadDegrees = 15, LengthShare = 0.5,
                    RadiusShare = 0.5, Rise = 0.04, Kink = 0.35, Segments = 3
                },
                new BranchLevel
                {
                    Count = 3, FromShare = 0.3, AngleDegrees = 45, AngleSpreadDegrees = 20, LengthShare = 0.55,
                    RadiusShare = 0.5, Rise = 0.05, Kink = 0.3, Segments = 2
                }
            },
            ClumpSpacingMetres = 0.8, ClumpSizeMetres = 1.5
        };

        public static readonly Habit OpenGrown = new Habit
        {
            Species = TreeSpecies.Oak, HeightMetres = 17, TrunkRadiusMetres = 0.55, TrunkShare = 0.24, LeanRadians = 0.06,
            Trunk = new BranchLevel { Segments = 9, Kink = 0.05, Rise = 0.01, TipShare = 0.8 },
            ForkLimbs = 5, ForkAngleDegrees = 50, ForkLengthShare = 0.6, Outline = CrownOutline.Dome,
            Levels = new[]
            {
                new BranchLevel
                {
                    Count = 3, FromShare = 0.45, AngleDegrees = 74, AngleSpreadDegrees = 8, LengthShare = 2.0,
                    RadiusShare = 0.5, Rise = 0.07, Kink = 0.3, Segments = 7
                },
                new BranchLevel
                {
                    Count = 8, FromShare = 0.15, AngleDegrees = 52, AngleSpreadDegrees = 15, LengthShare = 0.5,
                    RadiusShare = 0.45, Rise = 0.04, Kink = 0.35, Segments = 4
                },
                OakTwigs
            },
            ClumpSpacingMetres = 0.9, ClumpSizeMetres = 1.9
        };
    }
}
