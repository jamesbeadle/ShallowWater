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
        public const double FloorAboveTheGroundMetres = 0.02;
        private const double SouthOfHopwasBridgeMetres = 1750;
        private const double OffTheWaterMetres = 46;
        private const double OffSide = -PoundLimits.TowpathSide;
        private const double FarmerInsideTheGateMetres = 3;
        private const double HailMetres = 2.6;

        private readonly CanalsidePlot plot;

        public Farmyard(CanalsidePlot plot)
        {
            this.plot = plot;
        }

        public GroundPoint Centre => plot.Centre;
        public GroundPoint Lengthways => plot.Lengthways;
        public GroundPoint TowardsTheWater => plot.TowardsTheWater;
        public GroundRing Outline => plot.Outline;
        public IReadOnlyList<GroundPoint> Corners => plot.Corners;
        public GroundPoint Gate => Place(0, HalfWidthMetres);
        public GroundPoint FarmerStands => Place(0, HalfWidthMetres - FarmerInsideTheGateMetres);
        public double FarmerBearing => TowardsTheWater.Bearing;
        public double FloorMetres => Heights.GroundMetres + FloorAboveTheGroundMetres;

        public static Farmyard DunstallOn(CanalPound pound)
        {
            var along = pound.AlongOf(Landmark.HopwasBridge) + SouthOfHopwasBridgeMetres;
            var anchor = new WaterPosition(along, OffSide * OffTheWaterMetres);
            return new Farmyard(CanalsidePlot.Beside(pound, anchor, HalfLengthMetres, HalfWidthMetres));
        }

        public GroundPoint Place(double alongMetres, double towardsTheWaterMetres)
        {
            return plot.Place(alongMetres, towardsTheWaterMetres);
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
