namespace ShallowWater.Game.Day
{
    public readonly struct TimeOfDay
    {
        private const int MinutesInAnHour = 60;
        private const int HoursInADay = 24;
        private const double MinutesInADay = MinutesInAnHour * HoursInADay;

        public double MinutesSinceMidnight { get; }

        public TimeOfDay(double minutesSinceMidnight)
        {
            var wrapped = minutesSinceMidnight % MinutesInADay;
            MinutesSinceMidnight = (wrapped + MinutesInADay) % MinutesInADay;
        }

        public static TimeOfDay At(int hour, int minute)
        {
            return new TimeOfDay(hour * MinutesInAnHour + minute);
        }

        public int Hour => (int)(MinutesSinceMidnight / MinutesInAnHour);
        public int Minute => (int)(MinutesSinceMidnight % MinutesInAnHour);
        public double HoursSinceMidnight => MinutesSinceMidnight / MinutesInAnHour;

        public TimeOfDay Later(double minutes)
        {
            return new TimeOfDay(MinutesSinceMidnight + minutes);
        }

        public override string ToString()
        {
            return $"{Hour:00}:{Minute:00}";
        }
    }
}
