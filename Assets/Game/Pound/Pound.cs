using System;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public sealed class Pound
    {
        private readonly Centreline centreline;

        public Pound(Centreline centreline, double halfWidth)
        {
            this.centreline = centreline;
            HalfWidth = halfWidth;
        }

        public static Pound HuddlesfordToFazeley(GroundLine centreline)
        {
            return new Pound(new Centreline(centreline), PoundLimits.ChannelHalfWidthMetres);
        }

        public double HalfWidth { get; }
        public double LengthMetres => centreline.LengthMetres;
        public GroundLine Line => centreline.Line;

        public bool IsOnTheWater(WaterPosition position)
        {
            var isWithinLength = position.Along >= 0 && position.Along <= LengthMetres;
            var distanceFromCentre = Math.Abs(position.Across);
            var isWithinBanks = distanceFromCentre <= HalfWidth;
            return isWithinLength && isWithinBanks;
        }

        public WaterPosition Nearest(WaterPosition position)
        {
            var along = Math.Clamp(position.Along, 0, LengthMetres);
            var across = Math.Clamp(position.Across, -HalfWidth, HalfWidth);
            return new WaterPosition(along, across);
        }

        public WaterPosition WaterPositionAt(GroundPoint point)
        {
            return centreline.Nearest(point);
        }

        public GroundPoint GroundPointAt(WaterPosition position)
        {
            return centreline.Offset(position.Along, position.Across);
        }

        public double BearingAt(double along)
        {
            return centreline.BearingAt(along);
        }
    }
}
