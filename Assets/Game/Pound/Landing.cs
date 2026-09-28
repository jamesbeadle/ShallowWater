using System;
using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public static class Landing
    {
        private const double ReachMetres = 1.6;
        private const double SlowEnoughMetresPerSecond = 0.6;
        private const double AshoreFromTheEdgeMetres = 0.9;
        private const double BoardingReachMetres = 3.0;
        private const double Midstream = 0;

        public static bool CanStepAshore(BoatMotion boat, Pound pound)
        {
            var isSlowEnough = Math.Abs(boat.SpeedMetresPerSecond) < SlowEnoughMetresPerSecond;
            if (!isSlowEnough) return false;
            var side = SideOf(boat, pound);
            var nearestTheBank = HullOutline.Points.Max(point => pound.WaterPositionAt(boat.PointOf(point)).Across * side);
            return pound.HalfWidth - nearestTheBank < ReachMetres;
        }

        public static GroundPoint AshoreFrom(BoatMotion boat, Pound pound)
        {
            var helm = pound.WaterPositionAt(boat.PointOf(HullOutline.Helm));
            var across = SideOf(boat, pound) * (pound.HalfWidth + AshoreFromTheEdgeMetres);
            return pound.GroundPointAt(new WaterPosition(helm.Along, across));
        }

        public static bool CanStepAboard(BoatMotion boat, GroundPoint walker)
        {
            return walker.DistanceTo(boat.PointOf(HullOutline.Helm)) < BoardingReachMetres;
        }

        private static double SideOf(BoatMotion boat, Pound pound)
        {
            var across = pound.WaterPositionAt(boat.PointOf(HullOutline.Helm)).Across;
            return across == Midstream ? PoundLimits.TowpathSide : Math.Sign(across);
        }
    }
}
