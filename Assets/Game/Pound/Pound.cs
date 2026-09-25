using System;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public sealed class Pound
    {
        private const double BearingSouthRadians = Math.PI;

        public double NorthEndAlong { get; }
        public double SouthEndAlong { get; }
        public double HalfWidth { get; }

        public Pound(double northEndAlong, double southEndAlong, double halfWidth)
        {
            NorthEndAlong = northEndAlong;
            SouthEndAlong = southEndAlong;
            HalfWidth = halfWidth;
        }

        public static Pound HuddlesfordToFazeley()
        {
            return new Pound(
                PoundLimits.HuddlesfordPlanksAlong,
                PoundLimits.FazeleyChainAlong,
                PoundLimits.ChannelHalfWidthMetres);
        }

        public bool Holds(WaterPosition position)
        {
            var isWithinLength = position.Along >= NorthEndAlong && position.Along <= SouthEndAlong;
            var distanceFromCentre = Math.Abs(position.Across);
            var isWithinBanks = distanceFromCentre <= HalfWidth;
            return isWithinLength && isWithinBanks;
        }

        public WaterPosition Nearest(WaterPosition position)
        {
            var along = Math.Clamp(position.Along, NorthEndAlong, SouthEndAlong);
            var across = Math.Clamp(position.Across, -HalfWidth, HalfWidth);
            return new WaterPosition(along, across);
        }

        public GroundPoint GroundPointAt(WaterPosition position)
        {
            var southOfHopwasBridge = position.Along - PoundLimits.HopwasMooringAlong;
            return new GroundPoint(-position.Across, -southOfHopwasBridge);
        }

        public double BearingAt(double along)
        {
            return BearingSouthRadians;
        }
    }
}
