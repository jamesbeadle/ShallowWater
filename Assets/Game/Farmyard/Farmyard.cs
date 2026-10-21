using System.Collections.Generic;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Shapes;
using CanalPound = ShallowWater.Game.Pound.Pound;

namespace ShallowWater.Game.Farmyard
{
    public sealed class Farmyard
    {
        public const double HalfLengthMetres = 22;
        public const double HalfWidthMetres = 16;
        private const double SouthOfHopwasBridgeMetres = 1750;
        private const double OffTheWaterMetres = 46;
        private const double OffSide = -PoundLimits.TowpathSide;
        private const double FarmerInsideTheGateMetres = 3;
        private const double HailMetres = 2.6;

        public Farmyard(GroundPoint centre, GroundPoint lengthways, GroundPoint towardsTheWater)
        {
            Centre = centre;
            Lengthways = lengthways;
            TowardsTheWater = towardsTheWater;
            Outline = Footprints.Oblong(centre, lengthways, HalfLengthMetres, HalfWidthMetres);
        }

        public GroundPoint Centre { get; }
        public GroundPoint Lengthways { get; }
        public GroundPoint TowardsTheWater { get; }
        public GroundRing Outline { get; }
        public IReadOnlyList<GroundPoint> Corners => Outline.Corners;
        public Area Ground => new Area(Outline, new GroundRing[0]);
        public GroundPoint Gate => Place(0, HalfWidthMetres);
        public GroundPoint FarmerStands => Place(0, HalfWidthMetres - FarmerInsideTheGateMetres);
        public double FarmerBearing => TowardsTheWater.Bearing;
        public double FloorMetres => Heights.GroundMetres;

        public static Farmyard DunstallOn(CanalPound pound)
        {
            var along = pound.AlongOf(Landmark.HopwasBridge) + SouthOfHopwasBridgeMetres;
            var centre = pound.GroundPointAt(new WaterPosition(along, OffSide * OffTheWaterMetres));
            var waterside = pound.GroundPointAt(new WaterPosition(along, 0));
            var towardsTheWater = (waterside - centre).Normalised;
            return new Farmyard(centre, GroundPoint.Facing(pound.BearingAt(along)), towardsTheWater);
        }

        public GroundPoint Place(double alongMetres, double towardsTheWaterMetres)
        {
            return Centre + Lengthways * alongMetres + TowardsTheWater * towardsTheWaterMetres;
        }

        public WorldPoint Raised(GroundPoint place, double aboveTheFloorMetres)
        {
            return new WorldPoint(place.East, FloorMetres + aboveTheFloorMetres, place.North);
        }

        public bool IsWithinHail(GroundPoint feet)
        {
            return feet.DistanceTo(FarmerStands) < HailMetres;
        }
    }
}
