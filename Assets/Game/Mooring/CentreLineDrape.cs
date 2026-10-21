using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class CentreLineDrape
    {
        private const double OffTheStudMetres = 0.05;
        private const double OnThePlankAcross = 0.08;
        private const double PlankRunEndsAlong = -4.1;
        private const double OntoTheRoofAcross = 0.3;
        private const double OntoTheRoofAlong = -4.35;
        private const double RoofLipClearMetres = 0.05;
        private const double AlongTheRoofAcross = 0.62;
        private const double RoofRunStartsAlong = -4.7;
        private const double RoofRunEndsAlong = -8.85;
        private const double CoilAlong = -9.1;
        private const double CoilRadiusMetres = 0.13;
        private const double CoilTurns = 1.75;
        private const int PointsPerTurn = 16;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int RopeSides = 6;
        private const double Lift = HangingLine.RadiusMetres;
        private const double RopeDiameterMetres = 2 * HangingLine.RadiusMetres;
        private static readonly double PlankMetres = Sheeting.PlankTopMetres + Lift;
        private static readonly double RoofMetres = SparrowForm.RoofHeightAt(AlongTheRoofAcross) + Lift;

        private static readonly WorldPoint[] AlongTheBoat =
        {
            new WorldPoint(OffTheStudMetres, CentreStud.TieHeightMetres, OffTheStudMetres),
            new WorldPoint(OnThePlankAcross, PlankMetres, -OffTheStudMetres),
            new WorldPoint(OnThePlankAcross, PlankMetres, PlankRunEndsAlong),
            new WorldPoint(OntoTheRoofAcross, SparrowForm.RoofHeightAt(OntoTheRoofAcross) + RoofLipClearMetres, OntoTheRoofAlong),
            new WorldPoint(AlongTheRoofAcross, RoofMetres, RoofRunStartsAlong),
            new WorldPoint(AlongTheRoofAcross, RoofMetres, RoofRunEndsAlong)
        };

        public static IReadOnlyList<WorldPoint> Path => AlongTheBoat.Concat(CoiledAtTheBack()).ToList();

        public static Shape Laid()
        {
            return Tube.Along(Path, HangingLine.RadiusMetres, RopeSides);
        }

        private static IEnumerable<WorldPoint> CoiledAtTheBack()
        {
            var points = (int)(CoilTurns * PointsPerTurn);
            return Enumerable.Range(1, points).Select(point =>
            {
                var around = FullTurnRadians * point / PointsPerTurn;
                var height = RoofMetres + RopeDiameterMetres * around / FullTurnRadians;
                return new WorldPoint(AlongTheRoofAcross - CoilRadiusMetres * Math.Sin(around), height, CoilAlong + CoilRadiusMetres * Math.Cos(around));
            });
        }
    }
}
