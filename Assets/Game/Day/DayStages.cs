namespace ShallowWater.Game.Day
{
    public static class DayStages
    {
        private const double HorizonDegrees = 0;
        private const double FullDaylightDegrees = 12;
        private const double FullDarkDegrees = -10;

        public static StageBlend At(SkyPosition sun)
        {
            var height = sun.HeightDegrees;
            var isAboveTheHorizon = height >= HorizonDegrees;
            if (isAboveTheHorizon) return Daytime(height);
            var towardsTwilight = Shares.Between(height, FullDarkDegrees, HorizonDegrees);
            return new StageBlend(DayStage.Night, DayStage.Twilight, towardsTwilight);
        }

        private static StageBlend Daytime(double height)
        {
            var towardsDay = Shares.Between(height, HorizonDegrees, FullDaylightDegrees);
            return new StageBlend(DayStage.Twilight, DayStage.Day, towardsDay);
        }
    }
}
