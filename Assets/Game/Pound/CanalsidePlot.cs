using System.Collections.Generic;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public sealed class CanalsidePlot
    {
        private const double OnTheCentreline = 0;

        private CanalsidePlot(GroundPoint centre, GroundPoint lengthways, GroundPoint towardsTheWater, double halfLengthMetres, double halfWidthMetres)
        {
            Centre = centre;
            Lengthways = lengthways;
            TowardsTheWater = towardsTheWater;
            HalfLengthMetres = halfLengthMetres;
            HalfWidthMetres = halfWidthMetres;
            Outline = Footprints.Oblong(centre, lengthways, halfLengthMetres, halfWidthMetres);
        }

        public GroundPoint Centre { get; }
        public GroundPoint Lengthways { get; }
        public GroundPoint TowardsTheWater { get; }
        public double HalfLengthMetres { get; }
        public double HalfWidthMetres { get; }
        public GroundRing Outline { get; }
        public IReadOnlyList<GroundPoint> Corners => Outline.Corners;

        public static CanalsidePlot Beside(Pound pound, WaterPosition anchor, double halfLengthMetres, double halfWidthMetres)
        {
            var centre = pound.GroundPointAt(anchor);
            var waterside = pound.GroundPointAt(new WaterPosition(anchor.Along, OnTheCentreline));
            var towardsTheWater = (waterside - centre).Normalised;
            return new CanalsidePlot(centre, GroundPoint.Facing(pound.BearingAt(anchor.Along)), towardsTheWater, halfLengthMetres, halfWidthMetres);
        }

        public GroundPoint Place(double alongMetres, double towardsTheWaterMetres)
        {
            return Centre + Lengthways * alongMetres + TowardsTheWater * towardsTheWaterMetres;
        }
    }
}
