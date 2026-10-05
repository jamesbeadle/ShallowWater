using System;

namespace ShallowWater.Game.Boat
{
    public static class RudderForce
    {
        public const double HardOverRadians = 0.7;
        private const double AbaftTheSternMetres = 0.4;
        private const double AheadMetres = -BoatSize.HalfLengthMetres - AbaftTheSternMetres;
        private const double BladeSquareMetres = 0.65;
        private const double MostLift = 1.1;
        private const double MostDrag = 0.8;
        private const double SlackWaterMetresPerSecond = 0.01;
        private const double SquareOn = Math.PI / 2;
        private const double Doubled = 2;

        public static BoatForces On(double rudder, double flowAft, BoatWay way)
        {
            var isWaterRunningAft = flowAft > SlackWaterMetresPerSecond;
            if (!isWaterRunningAft) return BoatForces.None;
            var sternSway = way.AsideAt(AheadMetres);
            var attack = Math.Clamp(rudder * HardOverRadians + Math.Atan2(sternSway, flowAft), -SquareOn, SquareOn);
            var pressure = FreshWater.PressureAt(flowAft) * BladeSquareMetres;
            var lift = pressure * MostLift * Math.Sin(Doubled * attack);
            var drag = pressure * MostDrag * Math.Sin(attack) * Math.Sin(attack);
            return new BoatForces(-drag, 0, 0) + BoatForces.Aside(AheadMetres, -lift);
        }
    }
}
