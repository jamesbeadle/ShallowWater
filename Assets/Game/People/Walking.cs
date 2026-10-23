namespace ShallowWater.Game.People
{
    public static class Walking
    {
        public const double HalfACycle = 0.5;

        public static FigurePose Body(double cycleShare, GaitRhythm rhythm, double leanRadians, FootStep left, FootStep right)
        {
            var hips = GaitHips.At(cycleShare, rhythm, leanRadians, left, right);
            var torso = GaitTorso.On(hips, cycleShare, rhythm);
            return ArmSwing.On(torso, cycleShare, rhythm);
        }
    }
}
