using System;

namespace ShallowWater.Game.Shapes
{
    public readonly struct Offset
    {
        public static readonly Offset Up = new Offset(0, 1, 0);
        public static readonly Offset East = new Offset(1, 0, 0);

        public Offset(double east, double up, double north)
        {
            Eastward = east;
            Upward = up;
            Northward = north;
        }

        public double Eastward { get; }
        public double Upward { get; }
        public double Northward { get; }
        public double Length => Math.Sqrt(Dot(this));
        public Offset Normalised => this * (1 / Length);

        public static Offset Between(WorldPoint from, WorldPoint to)
        {
            return new Offset(to.East - from.East, to.Height - from.Height, to.North - from.North);
        }

        public static Offset operator +(Offset first, Offset second)
        {
            return new Offset(first.Eastward + second.Eastward, first.Upward + second.Upward, first.Northward + second.Northward);
        }

        public static Offset operator -(Offset first, Offset second)
        {
            return first + second * -1;
        }

        public static Offset operator *(Offset offset, double scale)
        {
            return new Offset(offset.Eastward * scale, offset.Upward * scale, offset.Northward * scale);
        }

        public double Dot(Offset other)
        {
            return Eastward * other.Eastward + Upward * other.Upward + Northward * other.Northward;
        }

        public Offset Cross(Offset other)
        {
            var east = Upward * other.Northward - Northward * other.Upward;
            var up = Northward * other.Eastward - Eastward * other.Northward;
            var north = Eastward * other.Upward - Upward * other.Eastward;
            return new Offset(east, up, north);
        }

        public WorldPoint From(WorldPoint point)
        {
            return new WorldPoint(point.East + Eastward, point.Height + Upward, point.North + Northward);
        }

        public Offset TurnedAbout(Offset axis, double radians)
        {
            var cosine = Math.Cos(radians);
            var alongTheAxis = axis * (axis.Dot(this) * (1 - cosine));
            return this * cosine + axis.Cross(this) * Math.Sin(radians) + alongTheAxis;
        }

        public Offset AnyRightAngle()
        {
            var isUpright = Math.Abs(Upward) > Math.Abs(Eastward) + Math.Abs(Northward);
            var reference = isUpright ? East : Up;
            return Cross(reference).Normalised;
        }
    }
}
