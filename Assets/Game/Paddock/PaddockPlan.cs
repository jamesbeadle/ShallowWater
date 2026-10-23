using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Fields;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Paddock
{
    public static class PaddockPlan
    {
        public const double GateAlongMetres = 4;
        public const double GateSwingRadians = -1.45;
        private const double Half = 0.5;
        private const double OnTheFloor = 0;

        private static readonly (double Along, double TowardsTheWater) TroughPlace = (-8, -10);

        private static readonly (double Along, double TowardsTheWater)[] Standards =
        {
            (-18, -Paddock.HalfWidthMetres), (14, -Paddock.HalfWidthMetres), (Paddock.HalfLengthMetres, 9)
        };

        private static readonly (double Along, double TowardsTheWater)[] FeedingGround =
        {
            (-16, -12), (-9, 4), (-3, -18), (2, 12), (6, -4), (11, -14), (15, 6), (19, -8), (-20, 8), (-12, -22), (8, 20), (21, 16)
        };

        public static GroundPoint GateHinge(Paddock paddock)
        {
            return paddock.Place(GateAlongMetres - HedgeGaps.GatewayMetres * Half, Paddock.HalfWidthMetres);
        }

        public static GroundPoint GateLatch(Paddock paddock)
        {
            return paddock.Place(GateAlongMetres + HedgeGaps.GatewayMetres * Half, Paddock.HalfWidthMetres);
        }

        public static HedgeGap Gateway()
        {
            var middle = Paddock.HalfLengthMetres + GateAlongMetres;
            return new HedgeGap(middle - HedgeGaps.GatewayMetres * Half, middle + HedgeGaps.GatewayMetres * Half, GapFurniture.OpenGate);
        }

        public static GroundPoint TroughCentre(Paddock paddock)
        {
            return paddock.Place(TroughPlace.Along, TroughPlace.TowardsTheWater);
        }

        public static IEnumerable<GroundPoint> StandardFeet(Paddock paddock)
        {
            return Standards.Select(standard => paddock.Place(standard.Along, standard.TowardsTheWater));
        }

        public static IEnumerable<WorldPoint> FeedingPlaces(Paddock paddock)
        {
            return FeedingGround.Select(place => paddock.Raised(paddock.Place(place.Along, place.TowardsTheWater), OnTheFloor));
        }
    }
}
