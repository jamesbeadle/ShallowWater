using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Roads
{
    public static class TelegraphPoles
    {
        private const double SpacingMetres = 55;
        private const double InFromTheHedgeMetres = 0.5;
        private const double HeightMetres = 7.5;
        private const double RadiusMetres = 0.11;
        private const int SidesOfEach = 6;
        private const double ArmHalfLengthMetres = 0.9;
        private const double ArmHalfThicknessMetres = 0.05;
        private const double ArmDepthMetres = 0.1;
        private static readonly double[] ArmsBelowTheTopMetres = { 0.25, 0.75 };

        public static void StandAlong(SurfaceShapes surfaces, Road road, Span span, double side)
        {
            var across = side * (road.HalfWidthMetres + road.VergeWidthMetres - InFromTheHedgeMetres);
            var foot = RoadSection.VergeHeightOf(road);
            foreach (var along in PlacesWithin(span)) Pole(surfaces, road, along, across, foot);
        }

        private static IEnumerable<double> PlacesWithin(Span span)
        {
            var first = Math.Ceiling(span.From / SpacingMetres) * SpacingMetres;
            for (var along = first; along <= span.To; along += SpacingMetres) yield return along;
        }

        private static void Pole(SurfaceShapes surfaces, Road road, double along, double across, double foot)
        {
            var centre = road.At(along, across);
            var top = foot + HeightMetres;
            surfaces.AddSolid(Surface.TelegraphPole, Footprints.Round(centre, RadiusMetres, SidesOfEach), foot, top);
            var crossways = GroundPoint.Facing(road.BearingAt(along)).RightAngleClockwise;
            var arm = Footprints.Oblong(centre, crossways, ArmHalfLengthMetres, ArmHalfThicknessMetres);
            foreach (var below in ArmsBelowTheTopMetres) surfaces.AddSolid(Surface.TelegraphPole, arm, top - below - ArmDepthMetres, top - below);
        }
    }
}
