using System;

namespace ShallowWater.Game.Guns
{
    public sealed class RifleSight
    {
        private const double Unaided = 1;
        private const double Half = 0.5;
        private const double Whole = 2;
        private const double RadiansPerDegree = Math.PI / 180;

        public static readonly RifleSight Open = new RifleSight(Unaided);

        private RifleSight(double magnification)
        {
            Magnification = magnification;
        }

        public double Magnification { get; }
        public bool IsATelescope => Magnification > Unaided;

        public static RifleSight Telescope(double magnification)
        {
            return new RifleSight(magnification);
        }

        public double FieldOfViewDegrees(double unaidedDegrees)
        {
            var halfAngle = unaidedDegrees * Half * RadiansPerDegree;
            var magnifiedHalfAngle = Math.Atan(Math.Tan(halfAngle) / Magnification);
            return magnifiedHalfAngle * Whole / RadiansPerDegree;
        }
    }
}
