using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Houses;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Ground
{
    public static class Inns
    {
        private const double Half = 0.5;
        private static readonly HouseStyle Inn = new CoachingInn();

        public static SurfaceShapes Raised(IEnumerable<Area> footprints)
        {
            var surfaces = new SurfaceShapes();
            foreach (var run in footprints.Select(Standing))
            {
                RunShapes.Raise(surfaces, run);
                InnSign.Hang(surfaces, run.Plot);
            }
            return surfaces;
        }

        public static IEnumerable<GroundRing> Footings(IEnumerable<Area> footprints)
        {
            return footprints.Select(Standing).SelectMany(RunFootings.Of);
        }

        private static Run Standing(Area footprint)
        {
            var plot = Row.Of(footprint.Outline);
            var random = new Random(RowSeed.Of(plot));
            var frontage = Math.Min(plot.LengthMetres, Inn.FrontageMetres(random));
            var layout = new RunLayout(Inn, 0, new List<double> { frontage }, false);
            var offset = (plot.LengthMetres - frontage) * Half;
            return RunBuilding.Built(plot, new List<RunLayout> { layout }, offset, random).Single();
        }
    }
}
