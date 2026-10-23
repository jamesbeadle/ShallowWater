using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class Sheeting
    {
        public const double PlankTopMetres = SparrowForm.TopPlankMetres + SparrowForm.TopPlankThicknessMetres;
        private const double Half = 0.5;
        private const int Ridge = 3;
        private const int StarboardSlope = 4;
        private const int StarboardEdge = 5;
        private const double Whole = 1;
        private const double None = 0;

        public static double SpanMetres => (SparrowForm.HoldFrontAlong - SparrowForm.CabinFrontAlong) / SparrowForm.ClothSpans;

        public static IReadOnlyList<WorldPoint> Sagging(double along)
        {
            var sag = SagAt(along);
            var gunwale = Hull.GunwaleAt(along);
            var halfBeam = Hull.HalfBeamAt(along);
            var sideTop = gunwale + SparrowForm.SideClothMetres;
            var ridge = new WorldPoint(BoatSides.Amidships, SparrowForm.TopPlankMetres, along);
            return new[]
            {
                new WorldPoint(-halfBeam, gunwale, along), new WorldPoint(-halfBeam, sideTop, along), MidSlope(-halfBeam, sideTop, along, sag), ridge,
                MidSlope(halfBeam, sideTop, along, sag), new WorldPoint(halfBeam, sideTop, along), new WorldPoint(halfBeam, gunwale, along)
            };
        }

        public static double HeightAt(double along, double acrossMetres)
        {
            var across = Math.Abs(acrossMetres);
            var isOnThePlank = across <= SparrowForm.TopPlankHalfWidthMetres;
            if (isOnThePlank) return PlankTopMetres;
            var profile = Sagging(along);
            var slope = profile[StarboardSlope];
            var isAboveTheSlope = across <= slope.East;
            return isAboveTheSlope ? HeightBetween(profile[Ridge], slope, across) : HeightBetween(slope, profile[StarboardEdge], across);
        }

        private static double HeightBetween(WorldPoint inner, WorldPoint outer, double across)
        {
            var share = (across - inner.East) / (outer.East - inner.East);
            return inner.Height + (outer.Height - inner.Height) * Math.Clamp(share, None, Whole);
        }

        private static WorldPoint MidSlope(double across, double sideTop, double along, double sag)
        {
            var run = Math.Abs(across);
            var rise = SparrowForm.TopPlankMetres - sideTop;
            var outward = new Offset(Math.Sign(across) * rise, run, 0).Normalised;
            var middle = new WorldPoint(across * Half, (sideTop + SparrowForm.TopPlankMetres) * Half, along);
            return (outward * -sag).From(middle);
        }

        private static double SagAt(double along)
        {
            var intoTheSpan = (along - SparrowForm.CabinFrontAlong) / SpanMetres;
            var between = Math.Sin(Math.PI * (intoTheSpan - Math.Floor(intoTheSpan)));
            return SparrowForm.ClothSagMetres * between * between;
        }
    }
}
