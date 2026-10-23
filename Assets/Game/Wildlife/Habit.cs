namespace ShallowWater.Game.Wildlife
{
    public readonly struct Habit
    {
        public const double Silent = 0;

        public Habit(double bodyRadiusMetres, double speedMetresPerSecond, double shortestPauseSeconds, double longestPauseSeconds, double hopHeightMetres, double shortestCallSeconds, double longestCallSeconds)
        {
            BodyRadiusMetres = bodyRadiusMetres;
            SpeedMetresPerSecond = speedMetresPerSecond;
            ShortestPauseSeconds = shortestPauseSeconds;
            LongestPauseSeconds = longestPauseSeconds;
            HopHeightMetres = hopHeightMetres;
            ShortestCallSeconds = shortestCallSeconds;
            LongestCallSeconds = longestCallSeconds;
        }

        public double BodyRadiusMetres { get; }
        public double SpeedMetresPerSecond { get; }
        public double ShortestPauseSeconds { get; }
        public double LongestPauseSeconds { get; }
        public double HopHeightMetres { get; }
        public double ShortestCallSeconds { get; }
        public double LongestCallSeconds { get; }
        public bool IsVocal => LongestCallSeconds > Silent;
    }
}
