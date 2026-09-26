using System;
using System.Linq;

namespace ShallowWater.Game.Ground
{
    public static class Footprints
    {
        private const double FullTurnRadians = 2 * Math.PI;
        private const int FirstCorner = 0;

        public static GroundRing Round(GroundPoint centre, double radiusMetres, int sides)
        {
            var corners = Enumerable.Range(FirstCorner, sides).Select(corner => CornerAround(centre, radiusMetres, corner, sides));
            return new GroundRing(corners);
        }

        public static GroundRing Oblong(GroundPoint centre, GroundPoint lengthways, double halfLengthMetres, double halfWidthMetres)
        {
            var along = lengthways * halfLengthMetres;
            var across = lengthways.RightAngleClockwise * halfWidthMetres;
            return new GroundRing(new[] { centre + along + across, centre + along - across, centre - along - across, centre - along + across });
        }

        private static GroundPoint CornerAround(GroundPoint centre, double radiusMetres, int corner, int sides)
        {
            var bearing = FullTurnRadians * corner / sides;
            return centre + GroundPoint.Facing(bearing) * radiusMetres;
        }
    }
}
