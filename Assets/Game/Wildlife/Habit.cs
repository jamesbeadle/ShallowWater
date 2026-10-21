namespace ShallowWater.Game.Wildlife
{
    public readonly struct Habit
    {
        public Habit(double bodyRadiusMetres, double speedMetresPerSecond, double shortestPauseSeconds, double longestPauseSeconds, double hopHeightMetres)
        {
            BodyRadiusMetres = bodyRadiusMetres;
            SpeedMetresPerSecond = speedMetresPerSecond;
            ShortestPauseSeconds = shortestPauseSeconds;
            LongestPauseSeconds = longestPauseSeconds;
            HopHeightMetres = hopHeightMetres;
        }

        public double BodyRadiusMetres { get; }
        public double SpeedMetresPerSecond { get; }
        public double ShortestPauseSeconds { get; }
        public double LongestPauseSeconds { get; }
        public double HopHeightMetres { get; }
    }
}
