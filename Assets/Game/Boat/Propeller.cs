using System;

namespace ShallowWater.Game.Boat
{
    public static class Propeller
    {
        public const double FullThrustNewtons = 9200;
        public const double AheadMetres = -BoatSize.HalfLengthMetres;
        private const double NoThrustMetresPerSecond = 7.0;
        private const double AsternEfficiency = 0.6;
        private const double HullWakeShare = 0.3;
        private const double DiscSquareMetres = 0.24;
        private const double WashOnTheRudderShare = 0.75;
        private const double AsternWalkShare = 0.2;
        private const double AheadWalkShare = 0.02;
        private const double WalkFadingMetresPerSecond = 1.0;
        private const double Stopped = 0;

        public static double ThrustAt(double turns, double aheadMetresPerSecond)
        {
            var inflow = aheadMetresPerSecond * (1 - HullWakeShare);
            var bite = Math.Abs(turns);
            var thrust = FullThrustNewtons * (turns * bite - bite * inflow / NoThrustMetresPerSecond);
            var isAstern = turns < Stopped;
            return isAstern ? thrust * AsternEfficiency : thrust;
        }

        public static double FlowPastTheRudder(double thrust, double aheadMetresPerSecond)
        {
            var inflow = aheadMetresPerSecond * (1 - HullWakeShare);
            var jetSquared = 2 * Math.Abs(thrust) / (FreshWater.KilogramsPerCubicMetre * DiscSquareMetres);
            var jet = Math.Sqrt(inflow * inflow + jetSquared) - Math.Abs(inflow);
            var flow = inflow + Math.Sign(thrust) * WashOnTheRudderShare * jet;
            return Math.Max(Stopped, flow);
        }

        public static BoatForces Walk(double thrust, double aheadMetresPerSecond)
        {
            var isAstern = thrust < Stopped;
            var share = isAstern ? AsternWalkShare : AheadWalkShare;
            var fading = 1 / (1 + Math.Abs(aheadMetresPerSecond) / WalkFadingMetresPerSecond);
            return BoatForces.Aside(AheadMetres, thrust * share * fading);
        }
    }
}
