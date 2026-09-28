using System.Collections.Generic;

namespace ShallowWater.Game.Boat
{
    public static class HullOutline
    {
        private const double ShoulderFromTheBowMetres = 3;
        private const double QuarterFromTheSternMetres = 1;
        private const double HalfBeam = BoatSize.BeamMetres / 2;
        private const double OnTheCentreline = 0;
        private const double Shoulder = BoatSize.HalfLengthMetres - ShoulderFromTheBowMetres;
        private const double Quarter = QuarterFromTheSternMetres - BoatSize.HalfLengthMetres;
        private const double HelmAlongMetres = -9.2;

        public static readonly HullPoint Helm = new HullPoint(HelmAlongMetres, OnTheCentreline);

        public static readonly IReadOnlyList<HullPoint> Points = new[]
        {
            new HullPoint(BoatSize.HalfLengthMetres, OnTheCentreline),
            new HullPoint(Shoulder, HalfBeam),
            new HullPoint(Shoulder, -HalfBeam),
            new HullPoint(Quarter, HalfBeam),
            new HullPoint(Quarter, -HalfBeam),
            new HullPoint(-BoatSize.HalfLengthMetres, OnTheCentreline)
        };
    }
}
