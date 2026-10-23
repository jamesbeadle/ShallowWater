using System;

namespace ShallowWater.Game.People
{
    public readonly struct SkullBump
    {
        private const double FaceForward = Math.PI / 2;
        private const double FullTurnRadians = 2 * Math.PI;

        public SkullBump(double metres, double fromTheFrontRadians, double heightMetres, double spreadRadians, double spreadMetres)
        {
            Metres = metres;
            FromTheFrontRadians = fromTheFrontRadians;
            HeightMetres = heightMetres;
            SpreadRadians = spreadRadians;
            SpreadMetres = spreadMetres;
        }

        public double Metres { get; }
        public double FromTheFrontRadians { get; }
        public double HeightMetres { get; }
        public double SpreadRadians { get; }
        public double SpreadMetres { get; }

        public double At(double heightMetres, double around)
        {
            var left = Apart(around, FaceForward + FromTheFrontRadians);
            var right = Apart(around, FaceForward - FromTheFrontRadians);
            var nearest = Math.Min(left, right) / SpreadRadians;
            var rise = (heightMetres - HeightMetres) / SpreadMetres;
            return Metres * Math.Exp(-nearest * nearest - rise * rise);
        }

        private static double Apart(double around, double from)
        {
            var apart = Math.IEEERemainder(around - from, FullTurnRadians);
            return Math.Abs(apart);
        }
    }
}
