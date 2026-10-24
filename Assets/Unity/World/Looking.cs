namespace ShallowWater.Unity.World
{
    public readonly struct Looking
    {
        public static readonly Looking Round = new Looking(-5f, 70f, 1f, isOnTheMouseAlone: false);
        public static readonly Looking DownTheSights = new Looking(-60f, 60f, 0.3f, isOnTheMouseAlone: true);

        public Looking(float lowestPitchDegrees, float highestPitchDegrees, float turnShare, bool isOnTheMouseAlone)
        {
            LowestPitchDegrees = lowestPitchDegrees;
            HighestPitchDegrees = highestPitchDegrees;
            TurnShare = turnShare;
            IsOnTheMouseAlone = isOnTheMouseAlone;
        }

        public float LowestPitchDegrees { get; }
        public float HighestPitchDegrees { get; }
        public float TurnShare { get; }
        public bool IsOnTheMouseAlone { get; }
    }
}
