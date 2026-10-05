using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Houses
{
    public static class HipFaces
    {
        private const double Half = 0.5;
        private const double Forwards = 1;
        private const double Backwards = -1;
        private const double Level = 0;

        public static IReadOnlyList<HipFace> Of(Plot plot, Pitch pitch)
        {
            var reach = RoofForm.EavesOverhangMetres;
            var length = plot.LengthMetres;
            var depth = plot.DepthMetres;
            var middle = pitch.HalfSpanMetres;
            var ridgeStart = Math.Min(middle, length * Half);
            var frontLeft = (-reach, -reach);
            var frontRight = (length + reach, -reach);
            var backRight = (length + reach, depth + reach);
            var backLeft = (-reach, depth + reach);
            var ridgeLeft = (ridgeStart, middle);
            var ridgeRight = (length - ridgeStart, middle);
            return new List<HipFace>
            {
                new HipFace(new[] { frontLeft, frontRight, ridgeRight, ridgeLeft }, (Forwards, Level)),
                new HipFace(new[] { backRight, backLeft, ridgeLeft, ridgeRight }, (Backwards, Level)),
                new HipFace(new[] { backLeft, frontLeft, ridgeLeft }, (Level, Backwards)),
                new HipFace(new[] { frontRight, backRight, ridgeRight }, (Level, Forwards)),
            };
        }
    }
}
