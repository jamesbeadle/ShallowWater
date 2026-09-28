using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Cottages
{
    public sealed class Cottage
    {
        private const int CornersToAverage = 4;
        private const double Half = 0.5;

        public Cottage(GroundRing footprint)
        {
            var corners = footprint.Corners;
            var firstSide = corners[1] - corners[0];
            var secondSide = corners[2] - corners[1];
            var isFirstSideLonger = firstSide.Length >= secondSide.Length;
            var lengthways = isFirstSideLonger ? firstSide : secondSide;
            var crossways = isFirstSideLonger ? secondSide : firstSide;
            Centre = (corners[0] + corners[1] + corners[2] + corners[3]) * (1.0 / CornersToAverage);
            Lengthways = lengthways * (1 / lengthways.Length);
            HalfLengthMetres = lengthways.Length * Half;
            HalfWidthMetres = crossways.Length * Half;
        }

        public GroundPoint Centre { get; }
        public GroundPoint Lengthways { get; }
        public GroundPoint Crossways => Lengthways.RightAngleClockwise;
        public double HalfLengthMetres { get; }
        public double HalfWidthMetres { get; }
        public double EavesMetres => CottageForm.FootMetres + CottageForm.EavesAboveTheFootMetres;
        public double RidgeMetres => EavesMetres + HalfWidthMetres * CottageForm.RoofRisePerMetre;

        public GroundPoint At(double along, double across)
        {
            return Centre + Lengthways * along + Crossways * across;
        }
    }
}
