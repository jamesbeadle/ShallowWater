using System;
using ShallowWater.Game.Boat;

namespace ShallowWater.Game.Pound
{
    public sealed class Pound
    {
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
            var isWithinBanks = Math.Abs(position.Across) <= HalfWidth;
            return isWithinLength && isWithinBanks;
        }

        public WaterPosition Nearest(WaterPosition position)
        {
            var along = Math.Clamp(position.Along, NorthEndAlong, SouthEndAlong);
            var across = Math.Clamp(position.Across, -HalfWidth, HalfWidth);
            return new WaterPosition(along, across);
        }
    }
}
