using System.Collections.Generic;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Shapes;
using CanalPound = ShallowWater.Game.Pound.Pound;

namespace ShallowWater.Game.Paddock
{
    public sealed class Paddock
    {
        public const double HalfLengthMetres = 27;
        public const double HalfWidthMetres = 29;
        private const double SouthOfHopwasBridgeMetres = 51;
        private const double OffTheWaterMetres = 47;

        private readonly CanalsidePlot plot;

        public Paddock(CanalsidePlot plot)
        {
            this.plot = plot;
        }

        public GroundPoint Centre => plot.Centre;
        public GroundPoint Lengthways => plot.Lengthways;
        public GroundRing Outline => plot.Outline;
        public IReadOnlyList<GroundPoint> Corners => plot.Corners;
        public double FloorMetres => Heights.GroundMetres;

        public static Paddock HopwasOn(CanalPound pound)
        {
            var along = pound.AlongOf(Landmark.HopwasBridge) + SouthOfHopwasBridgeMetres;
            var anchor = new WaterPosition(along, PoundLimits.TowpathSide * OffTheWaterMetres);
            return new Paddock(CanalsidePlot.Beside(pound, anchor, HalfLengthMetres, HalfWidthMetres));
        }

        public GroundPoint Place(double alongMetres, double towardsTheWaterMetres)
        {
            return plot.Place(alongMetres, towardsTheWaterMetres);
        }

        public WorldPoint Raised(GroundPoint place, double aboveTheFloorMetres)
        {
            return new WorldPoint(place.East, FloorMetres + aboveTheFloorMetres, place.North);
        }
    }
}
