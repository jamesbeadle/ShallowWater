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
        private const int SidesOfEach = 8;
        private const int FirstBollard = 0;
        private const int SpacesBesideTheFirst = 1;
        private const double Half = 0.5;
        private const double FullTurnRadians = 2 * Math.PI;

        public static void StandAlong(SurfaceShapes surfaces, Pound pound, Stretch mooring)
        {
            var top = Heights.BankTopMetres + HeightMetres;
            foreach (var along in PlacesAlong(mooring.Span))
            {
                var footprint = FootprintAt(pound, along);
                surfaces.Add(Surface.Bollard, Extrusion.Walls(footprint, Heights.BankTopMetres, top));
                surfaces.Add(Surface.Bollard, Extrusion.Roof(footprint, top));
            }
        }

        private static IEnumerable<double> PlacesAlong(Span span)
        {
            var count = (int)Math.Floor(span.Length / SpacingMetres) + SpacesBesideTheFirst;
            var first = span.Middle - Half * (count - SpacesBesideTheFirst) * SpacingMetres;
            return Enumerable.Range(FirstBollard, count).Select(index => first + index * SpacingMetres);
        }

        private static GroundRing FootprintAt(Pound pound, double along)
        {
            var across = PoundLimits.TowpathSide * (pound.HalfWidth + BackFromTheEdgeMetres);
            var centre = pound.GroundPointAt(new WaterPosition(along, across));
            var corners = Enumerable.Range(FirstBollard, SidesOfEach).Select(corner => CornerAround(centre, corner));
            return new GroundRing(corners);
        }

        private static GroundPoint CornerAround(GroundPoint centre, int corner)
        {
            var bearing = FullTurnRadians * corner / SidesOfEach;
            return centre + GroundPoint.Facing(bearing) * RadiusMetres;
        }
    }
}
