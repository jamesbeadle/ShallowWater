using System;

namespace ShallowWater.Game.Saving
{
    public static class SaveAge
    {
        private const string JustNow = "just now";
        private const int MinutesInAnHour = 60;
        private const int HoursInADay = 24;

        public static string Words(DateTime savedAt, DateTime now)
        {
            var age = now - savedAt;
            var minutes = (int)age.TotalMinutes;
            if (minutes < 1) return JustNow;
            if (minutes < MinutesInAnHour) return Ago(minutes, "minute");
            var hours = (int)age.TotalHours;
            if (hours < HoursInADay) return Ago(hours, "hour");
            return Ago((int)age.TotalDays, "day");
        }

        private static string Ago(int count, string unit)
        {
            var isOne = count == 1;
            var plural = isOne ? unit : unit + "s";
            return $"{count} {plural} ago";
        }
    }
}
