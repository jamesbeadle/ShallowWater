using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Boat
{
    public static class Hull
    {
        public const double KeelMetres = -0.65;
        public const double SternAlong = -BoatSize.HalfLengthMetres;
        public const double StemAlong = BoatSize.HalfLengthMetres;
        private const double HalfBeamMetres = BoatSize.BeamMetres / 2;
        private const double FreeboardMetres = 0.45;
        private const double BowTaperMetres = 3.4;
        private const double SternTaperMetres = 1.6;
        private const double SternNarrowing = 0.4;
        private const double BowRiseMetres = 0.8;
        private const double BowRiseRunMetres = 6;
        private const double SternRiseMetres = 0.3;
        private const double SternRiseRunMetres = 5;
        private const double StationSpacingMetres = 0.35;
        private const double Whole = 1;
        private const double None = 0;

        public static double HalfBeamAt(double along)
        {
            var intoTheBow = Fraction(along - (StemAlong - BowTaperMetres), BowTaperMetres);
            var intoTheStern = Fraction(SternAlong + SternTaperMetres - along, SternTaperMetres);
            var bowTaper = Whole - intoTheBow * intoTheBow;
            var sternTaper = Whole - SternNarrowing * intoTheStern * intoTheStern;
            return HalfBeamMetres * bowTaper * sternTaper;
        }

        public static double GunwaleAt(double along)
        {
            var intoTheBow = Fraction(along - (StemAlong - BowRiseRunMetres), BowRiseRunMetres);
            var intoTheStern = Fraction(SternAlong + SternRiseRunMetres - along, SternRiseRunMetres);
            return FreeboardMetres + BowRiseMetres * intoTheBow * intoTheBow + SternRiseMetres * intoTheStern * intoTheStern;
        }

        public static List<double> Stations(double from, double to)
        {
            var count = (int)Math.Ceiling((to - from) / StationSpacingMetres);
            var stations = new List<double>();
            for (var station = 0; station <= count; station++) stations.Add(from + (to - from) * station / count);
            return stations;
        }

        private static double Fraction(double distance, double run)
        {
            return Math.Clamp(distance / run, None, Whole);
        }
    }
}
