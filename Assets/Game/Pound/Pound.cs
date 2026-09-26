using System;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public sealed class Pound
    {
        private const double HuddlesfordAlong = 0;

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
        public double PlanksAlong => PoundLimits.PlanksFromHuddlesfordMetres;
        public double LockGatesAlong => LengthMetres - PoundLimits.LockGatesFromGlascoteMetres;
        private double BoatAlongNearestHuddlesford => PlanksAlong + BoatSize.HalfLengthMetres;
        private double BoatAlongNearestGlascote => LockGatesAlong - BoatSize.HalfLengthMetres;

        public bool IsOnTheWater(WaterPosition position)
        {
            var isBetweenPlanksAndLock = position.Along >= BoatAlongNearestHuddlesford && position.Along <= BoatAlongNearestGlascote;
            var distanceFromCentre = Math.Abs(position.Across);
            var isWithinBanks = distanceFromCentre <= HalfWidth;
            return isBetweenPlanksAndLock && isWithinBanks;
        }

        public WaterPosition Nearest(WaterPosition position)
        {
            var along = Math.Clamp(position.Along, BoatAlongNearestHuddlesford, BoatAlongNearestGlascote);
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

        public double AlongOf(Landmark landmark)
        {
            if (landmark == Landmark.HuddlesfordJunction) return HuddlesfordAlong;
            if (landmark == Landmark.GlascoteLocks) return LengthMetres;
            var place = LandmarkPlaces.Of(landmark);
            return WaterPositionAt(place).Along;
        }

        public GroundLine LineAlong(Span span)
        {
            return CentrelinePiece.Between(centreline, span);
        }
    }
}
