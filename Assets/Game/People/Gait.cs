namespace ShallowWater.Game.People
{
    public static class Gait
    {
        public static double CyclesPerSecond(double speedMetresPerSecond)
        {
            var rhythm = GaitRhythm.At(speedMetresPerSecond);
            return rhythm.CyclesPerSecond;
        }

        public static FigurePose At(double cycleShare, double speedMetresPerSecond, double turnRadiansPerSecond, double seconds)
        {
            var rhythm = GaitRhythm.At(speedMetresPerSecond);
            var lean = Turning.LeanRadians(turnRadiansPerSecond, speedMetresPerSecond);
            var left = Stance.LeftFoot.Blend(FootPath.At(cycleShare, rhythm, FigureSide.Left), rhythm.Going);
            var right = Stance.RightFoot.Blend(FootPath.At(cycleShare + Walking.HalfACycle, rhythm, FigureSide.Right), rhythm.Going);
            var walking = Walking.Body(cycleShare, rhythm, lean, left, right);
            var body = Stance.Body(seconds).Blend(walking, rhythm.Going);
            return Skirts.Following(Legs.Planted(body, left, right));
        }
    }
}
