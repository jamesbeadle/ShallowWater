namespace ShallowWater.Game.Day
{
    public readonly struct StageBlend
    {
        private const double Whole = 1;

        public DayStage Earlier { get; }
        public DayStage Later { get; }
        public double Share { get; }

        public StageBlend(DayStage earlier, DayStage later, double share)
        {
            Earlier = earlier;
            Later = later;
            Share = share;
        }

        public bool IsAfterDark => Earlier == DayStage.Night;
        public double NightShare => IsAfterDark ? Whole - Share : 0;
    }
}
