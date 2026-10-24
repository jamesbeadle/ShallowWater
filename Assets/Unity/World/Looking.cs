namespace ShallowWater.Unity.World
{
    public readonly struct Looking
    {
        public static readonly Looking Round = new Looking(-5f, 70f, 1f);
        public static readonly Looking DownTheSights = new Looking(-60f, 60f, 0.3f);

        public Looking(float lowestPitchDegrees, float highestPitchDegrees, float turnShare)
        {
            LowestPitchDegrees = lowestPitchDegrees;
            HighestPitchDegrees = highestPitchDegrees;
            TurnShare = turnShare;
        }

        public float LowestPitchDegrees { get; }
        public float HighestPitchDegrees { get; }
        public float TurnShare { get; }
    }
}
