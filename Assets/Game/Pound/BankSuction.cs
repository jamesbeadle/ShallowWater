using System;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public static class BankSuction
    {
        private const double SternPullPerSpeedSquared = 65;
        private const double BowCushionPerSpeedSquared = -35;
        private const double NarrowestGapMetres = 0.25;
        private const double OneMetre = 1;
        private const double HalfBeam = BoatSize.BeamMetres / 2;

        public static BoatForces On(BoatMotion boat, Pound pound)
        {
            var speed = boat.SpeedMetresPerSecond;
            var sternPull = SternPullPerSpeedSquared * speed * speed;
            var bowCushion = BowCushionPerSpeedSquared * speed * speed;
            var stern = Towards(boat, pound, HullOutline.SternQuarter, sternPull);
            return stern + Towards(boat, pound, HullOutline.BowShoulder, bowCushion);
        }

        private static BoatForces Towards(BoatMotion boat, Pound pound, HullPoint point, double strength)
        {
            var place = boat.PointOf(point);
            var water = pound.WaterPositionAt(place);
            var leftBank = Gap(pound.HalfWidth + water.Across);
            var rightBank = Gap(pound.HalfWidth - water.Across);
            var pull = strength * (OneMetre / rightBank - OneMetre / leftBank);
            var rightwards = GroundPoint.Facing(pound.BearingAt(water.Along)).RightAngleClockwise;
            var toStarboard = rightwards.Dot(boat.Starboard);
            return BoatForces.Aside(point.Ahead, pull * toStarboard);
        }

        private static double Gap(double fromTheCentrelineMetres)
        {
            return Math.Max(NarrowestGapMetres, fromTheCentrelineMetres - HalfBeam);
        }
    }
}
