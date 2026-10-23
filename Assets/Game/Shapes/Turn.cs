using System;

namespace ShallowWater.Game.Shapes
{
    public readonly struct Turn
    {
        private const double FullTurnRadians = 2 * Math.PI;
        private const int ClosedRing = 0;
        private const int OpenRing = 1;

        private Turn(double fromRadians, double sweepRadians, int sides, int extraPoints)
        {
            FromRadians = fromRadians;
            SweepRadians = sweepRadians;
            Sides = sides;
            PointsPerRing = sides + extraPoints;
        }

        public double FromRadians { get; }
        public double SweepRadians { get; }
        public int Sides { get; }
        public int PointsPerRing { get; }

        public static Turn Whole(int sides)
        {
            return new Turn(0, FullTurnRadians, sides, ClosedRing);
        }

        public static Turn Through(double fromRadians, double toRadians, int sides)
        {
            return new Turn(fromRadians, toRadians - fromRadians, sides, OpenRing);
        }

        public double AngleAt(int side)
        {
            return FromRadians + SweepRadians * side / Sides;
        }
    }
}
