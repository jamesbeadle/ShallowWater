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
        private const double BowTaperMetres = 4.6;
        private const double BowFullness = 1.1;
        private const double SternTaperMetres = 1.6;
        private const double SternNarrowing = 0.4;
        private const double BowRiseMetres = 0.9;
        private const double BowRiseRunMetres = 6;
        private const double SternRiseMetres = 0.3;
        private const double SternRiseRunMetres = 5;
        private const double StemRakeMetres = 0.3;
        private const double CounterRakeMetres = 0.2;
        private const double StationSpacingMetres = 0.35;
        private const double DeckMetres = 0.62;
        private const double ForeDeckRiseMetres = 0.25;
        private const double Whole = 1;
        private const double None = 0;

        public static double HalfBeamAt(double along)
        {
            var intoTheBow = IntoTheBow(along, BowTaperMetres);
            var intoTheStern = IntoTheStern(along, SternTaperMetres);
            var bowTaper = Math.Pow(Whole - intoTheBow * intoTheBow, BowFullness);
            var sternTaper = Whole - SternNarrowing * intoTheStern * intoTheStern;
            return HalfBeamMetres * bowTaper * sternTaper;
        }

        public static double GunwaleAt(double along)
        {
            var intoTheBow = IntoTheBow(along, BowRiseRunMetres);
            var intoTheStern = IntoTheStern(along, SternRiseRunMetres);
            return FreeboardMetres + BowRiseMetres * intoTheBow * intoTheBow + SternRiseMetres * intoTheStern * intoTheStern;
        }

        public static double DeckAt(double along)
        {
            var intoTheBow = IntoTheBow(along, BowTaperMetres);
            var foreDeck = DeckMetres + ForeDeckRiseMetres * intoTheBow * intoTheBow;
            return Math.Min(GunwaleAt(along), foreDeck);
        }

        public static double RakeAt(double along, double heightMetres)
        {
            var upTheSide = (heightMetres - KeelMetres) / (GunwaleAt(along) - KeelMetres);
            return RakeAt(along) * upTheSide;
        }

        public static double RakeAt(double along)
        {
            var intoTheBow = IntoTheBow(along, BowTaperMetres);
            var intoTheStern = IntoTheStern(along, SternTaperMetres);
            return StemRakeMetres * intoTheBow * intoTheBow - CounterRakeMetres * intoTheStern * intoTheStern;
        }

        public static List<double> Stations(double from, double to)
        {
            return Stations(from, to, StationSpacingMetres);
        }

        public static List<double> Stations(double from, double to, double spacingMetres)
        {
            var count = (int)Math.Ceiling((to - from) / spacingMetres);
            var stations = new List<double>();
            for (var station = 0; station <= count; station++) stations.Add(from + (to - from) * station / count);
            return stations;
        }

        private static double IntoTheBow(double along, double runMetres)
        {
            return Fraction(along - (StemAlong - runMetres), runMetres);
        }

        private static double IntoTheStern(double along, double runMetres)
        {
            return Fraction(SternAlong + runMetres - along, runMetres);
        }

        private static double Fraction(double distance, double run)
        {
            return Math.Clamp(distance / run, None, Whole);
        }
    }
}
