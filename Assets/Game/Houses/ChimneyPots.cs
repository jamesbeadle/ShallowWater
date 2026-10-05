using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class ChimneyPots
    {
        private const double SpacingMetres = 0.26;
        private const double ShortestMetres = 0.42;
        private const double HeightSpreadMetres = 0.4;
        private const double FootRadiusMetres = 0.14;
        private const double NeckRadiusMetres = 0.115;
        private const double RimRadiusMetres = 0.135;
        private const double RimDepthMetres = 0.07;
        private const double ShareRolledRims = 0.4;
        private const double RolledRimRadiusMetres = 0.155;
        private const double Half = 0.5;
        private const int Sides = 10;
        private const int AlongToTheSeed = 1000;

        public static void Set(SurfaceShapes surfaces, Plot plot, StackPlace stack, double middle, double top)
        {
            var random = new Random(RowSeed.Of(plot) + (int)(stack.AlongMetres * AlongToTheSeed));
            var first = -(stack.Pots - 1) * SpacingMetres * Half;
            for (var pot = 0; pot < stack.Pots; pot++)
            {
                var foot = plot.Point(stack.AlongMetres, middle + first + pot * SpacingMetres, top);
                surfaces.Add(Surface.ChimneyPot, Lathe.Turned(foot, Profile(random), Sides));
            }
        }

        private static IReadOnlyList<ProfilePoint> Profile(Random random)
        {
            var height = ShortestMetres + random.NextDouble() * HeightSpreadMetres;
            var isRolled = random.NextDouble() < ShareRolledRims;
            var rim = isRolled ? RolledRimRadiusMetres : RimRadiusMetres;
            return new List<ProfilePoint>
            {
                new ProfilePoint(FootRadiusMetres, 0),
                new ProfilePoint(NeckRadiusMetres, height - RimDepthMetres),
                new ProfilePoint(rim, height - RimDepthMetres * Half),
                new ProfilePoint(NeckRadiusMetres, height),
            };
        }
    }
}
