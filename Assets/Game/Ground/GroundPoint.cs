using System;

namespace ShallowWater.Game.Ground
{
    public readonly struct GroundPoint
    {
        public static readonly GroundPoint HopwasBridge = new GroundPoint(0, 0);

        public double East { get; }
        public double North { get; }

        public GroundPoint(double east, double north)
        {
            East = east;
            North = north;
        }

        public double Length => Math.Sqrt(East * East + North * North);
        public double Bearing => Math.Atan2(East, North);
        public GroundPoint RightAngleClockwise => new GroundPoint(North, -East);

        public static GroundPoint Facing(double bearing)
        {
            return new GroundPoint(Math.Sin(bearing), Math.Cos(bearing));
        }

        public static GroundPoint operator +(GroundPoint first, GroundPoint second)
        {
            return new GroundPoint(first.East + second.East, first.North + second.North);
        }

        public static GroundPoint operator -(GroundPoint first, GroundPoint second)
        {
            return new GroundPoint(first.East - second.East, first.North - second.North);
        }

        public static GroundPoint operator *(GroundPoint point, double scale)
        {
            return new GroundPoint(point.East * scale, point.North * scale);
        }

        public double Dot(GroundPoint other)
        {
            return East * other.East + North * other.North;
        }

        public double DistanceTo(GroundPoint other)
        {
            var difference = other - this;
            return difference.Length;
        }

        public bool IsSamePlaceAs(GroundPoint other)
        {
            return East == other.East && North == other.North;
        }

        public double DistanceToSegment(GroundPoint from, GroundPoint to)
        {
            var run = to - from;
            var lengthSquared = run.Dot(run);
            var isAPoint = lengthSquared < double.Epsilon;
            if (isAPoint) return DistanceTo(from);
            var fraction = Math.Clamp((this - from).Dot(run) / lengthSquared, 0, 1);
            return DistanceTo(from + run * fraction);
        }
    }
}
