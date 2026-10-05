using System;

namespace ShallowWater.Game.Day
{
    public static class SunPath
    {
        private const double TamworthLatitudeDegrees = 52.63;
        private const double MidOctoberDeclinationDegrees = -8.6;
        private const double SolarNoonHours = 12;
        private const double DegreesPerHour = 15;
        private const double RadiansPerDegree = Math.PI / 180;

        public static SkyPosition SunAt(TimeOfDay time)
        {
            var latitude = TamworthLatitudeDegrees * RadiansPerDegree;
            var declination = MidOctoberDeclinationDegrees * RadiansPerDegree;
            var hourAngle = (time.HoursSinceMidnight - SolarNoonHours) * DegreesPerHour * RadiansPerDegree;
            var sineOfHeight = Math.Sin(latitude) * Math.Sin(declination) + Math.Cos(latitude) * Math.Cos(declination) * Math.Cos(hourAngle);
            var height = Math.Asin(sineOfHeight);
            var southward = Math.Cos(hourAngle) * Math.Sin(latitude) - Math.Tan(declination) * Math.Cos(latitude);
            var bearing = Math.Atan2(Math.Sin(hourAngle), southward) + Math.PI;
            return new SkyPosition(height / RadiansPerDegree, bearing / RadiansPerDegree);
        }
    }
}
