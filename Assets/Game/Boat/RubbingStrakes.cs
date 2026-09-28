using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class RubbingStrakes
    {
        private const double FromAlong = -9.9;
        private const double ToAlong = 10.2;
        private const double HeightMetres = 0.06;
        private const double ProudMetres = 0.03;
        private static readonly double[] BelowTheGunwaleMetres = { 0.16, 0.42 };
        private static readonly double[] Sides = { BoatSides.Port, BoatSides.Starboard };

        public static void Build(SurfaceShapes surfaces)
        {
            var stations = Hull.Stations(FromAlong, ToAlong);
            foreach (var below in BelowTheGunwaleMetres)
            {
                foreach (var side in Sides)
                {
                    var sections = stations.Select(along => Raked(along, Beading.Across(along, side, Hull.HalfBeamAt(along), Band(along, below), ProudMetres)));
                    surfaces.Add(Surface.RubbingStrake, Sweep.Lengthways(sections.ToList()));
                }
            }
        }

        private static IReadOnlyList<WorldPoint> Raked(double along, IReadOnlyList<WorldPoint> section)
        {
            return section.Select(point => new WorldPoint(point.East, point.Height, along + Hull.RakeAt(along, point.Height))).ToList();
        }

        private static Rise Band(double along, double below)
        {
            var top = Hull.GunwaleAt(along) - below;
            return new Rise(top - HeightMetres, top);
        }
    }
}
