using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FootPath
    {
        private const double ToeOutRadians = 0.08;
        private const double Whole = 1;
        private const double LeavesTheGroundShare = 0.25;
        private const double LandsShare = 0.35;
        private static readonly CycleCurve WalkLift = new CycleCurve((0, 0.012), (0.12, 0), (0.4, 0), (0.6, 0.1), (0.78, 0.11), (0.93, 0.025));
        private static readonly CycleCurve RunLift = new CycleCurve((0, 0.03), (0.08, 0), (0.18, 0.03), (0.3, 0.14), (0.45, 0.3), (0.62, 0.3), (0.85, 0.08));
        private static readonly CycleCurve WalkToesDown = new CycleCurve((0, -0.22), (0.12, 0), (0.42, 0), (0.62, 0.6), (0.72, 0.3), (0.86, -0.1));
        private static readonly CycleCurve RunToesDown = new CycleCurve((0, 0.1), (0.08, 0), (0.18, 0.2), (0.3, 0.85), (0.42, 0.5), (0.6, 0.1), (0.85, 0.05));

        public static FootStep At(double cycleShare, GaitRhythm rhythm, FigureSide side)
        {
            var across = side == FigureSide.Right ? rhythm.StepHalfWidthMetres : -rhythm.StepHalfWidthMetres;
            var lift = rhythm.Between(WalkLift.At(cycleShare) * rhythm.Going, RunLift.At(cycleShare));
            var ankle = new WorldPoint(across, AskewForm.AnkleAboveTheSoleMetres + lift, Ahead(cycleShare, rhythm));
            var toesDown = rhythm.Between(WalkToesDown.At(cycleShare) * rhythm.Going, RunToesDown.At(cycleShare));
            return new FootStep(ankle, toesDown, ToeOutRadians);
        }

        private static double Ahead(double cycleShare, GaitRhythm rhythm)
        {
            var cycle = cycleShare - System.Math.Floor(cycleShare);
            var isInStance = cycle < rhythm.StanceShare;
            var ahead = rhythm.AnkleAheadMetres;
            var behind = rhythm.AnkleBehindMetres;
            if (isInStance) return ahead - (ahead + behind) * cycle / rhythm.StanceShare;
            var swung = (cycle - rhythm.StanceShare) / (Whole - rhythm.StanceShare);
            var groundMoved = rhythm.GroundMovedInTheSwingMetres;
            return Hermite.Between(-behind, ahead, -groundMoved * LeavesTheGroundShare, -groundMoved * LandsShare, swung);
        }
    }
}
