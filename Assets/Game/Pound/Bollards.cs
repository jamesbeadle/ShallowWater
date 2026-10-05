using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Pound
{
    public static class Bollards
    {
        private const double SpacingMetres = 9;
        private const double BackFromTheEdgeMetres = 0.3;
        private const double RadiusMetres = 0.12;
        private const double HeightMetres = 0.6;
        public const double TopMetres = Heights.BankTopMetres + HeightMetres;
        private const int SidesOfEach = 8;
        private const int FirstBollard = 0;
        private const int SpacesBesideTheFirst = 1;
        private const double Half = 0.5;

        public static void StandAlong(SurfaceShapes surfaces, Pound pound, Stretch mooring)
        {
            foreach (var centre in PlacesOn(pound, mooring))
            {
                var footprint = Footprints.Round(centre, RadiusMetres, SidesOfEach);
                surfaces.AddSolid(Surface.Bollard, footprint, Heights.BankTopMetres, TopMetres);
            }
        }

        public static IReadOnlyList<GroundPoint> AlongThe(Pound pound)
        {
            var moorings = PoundStretches.Of(pound).Where(stretch => stretch.IsMooring);
            return moorings.SelectMany(mooring => PlacesOn(pound, mooring)).ToList();
        }

        private static IEnumerable<GroundPoint> PlacesOn(Pound pound, Stretch mooring)
        {
            return PlacesAlong(mooring.Span).Select(along => CentreAt(pound, along));
        }

        private static IEnumerable<double> PlacesAlong(Span span)
        {
            var count = (int)Math.Floor(span.Length / SpacingMetres) + SpacesBesideTheFirst;
            var first = span.Middle - Half * (count - SpacesBesideTheFirst) * SpacingMetres;
            return Enumerable.Range(FirstBollard, count).Select(index => first + index * SpacingMetres);
        }

        private static GroundPoint CentreAt(Pound pound, double along)
        {
            var across = PoundLimits.TowpathSide * (pound.HalfWidth + BackFromTheEdgeMetres);
            return pound.GroundPointAt(new WaterPosition(along, across));
        }
    }
}
