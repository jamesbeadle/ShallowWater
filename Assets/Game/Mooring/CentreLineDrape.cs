using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class CentreLineDrape
    {
        private const double OnThePlankAcross = 0.08;
        private const double PlankRunStartsAlong = -0.3;
        private const double PlankRunEndsAlong = SparrowForm.CabinFrontAlong + 0.08;
        private const double UpTheCabinFrontAcross = 0.2;
        private const double OntoTheRoofAcross = 0.3;
        private const double OntoTheRoofAlong = SparrowForm.CabinFrontAlong - 0.15;
        private const double RoofRunStartsAlong = -4.7;
        private const double RoofRunEndsAlong = -8.85;
        private const double CoilAlong = -9.1;
        private const double CoilOuterRadiusMetres = 0.13;
        private const double CoilTurns = 3;
        private const double CoilTurnGapMetres = 0.004;
        private const double CoilStartBearing = 0;
        private const double BendRadiusMetres = 0.06;
        private const double OverTheEdgeMetres = 0.03;
        private const double Half = 0.5;
        private const double AlongTheRoofAcross = (RoofStowage.CansOuterEdgeMetres + RoofFittings.RailInnerEdgeMetres) * Half;
        private static readonly double PlankMetres = Rope.RestingOn(Sheeting.PlankTopMetres);
        private static readonly LineEnd Stud = new LineEnd(CentreStud.Tie, CentreStud.RadiusMetres);
        private static readonly WorldPoint CoilAxis = new WorldPoint(AlongTheRoofAcross, Sheeting.PlankTopMetres, CoilAlong);

        public static Shape Laid()
        {
            var turns = Turns.UpTheStud(Stud, new GroundPoint(OnThePlankAcross, PlankRunStartsAlong));
            var coil = Coil.Flaked(CoilAxis, CoilOuterRadiusMetres, CoilStartBearing, CoilTurns, Rope.DiameterMetres + CoilTurnGapMetres).Select(OnTheRoof).ToList();
            var rope = new List<WorldPoint>(turns);
            rope.AddRange(RoundedCorners.Of(AlongTheBoat(turns[turns.Count - 1], coil), BendRadiusMetres).Skip(1));
            rope.AddRange(coil.Skip(2));
            return Rope.Laid(rope);
        }

        private static IReadOnlyList<WorldPoint> AlongTheBoat(WorldPoint offTheStud, IReadOnlyList<WorldPoint> coil)
        {
            return new[]
            {
                offTheStud,
                new WorldPoint(OnThePlankAcross, PlankMetres, PlankRunStartsAlong),
                new WorldPoint(OnThePlankAcross, PlankMetres, PlankRunEndsAlong),
                OverTheFrontEdge(),
                OnTheRoof(new WorldPoint(OntoTheRoofAcross, PlankMetres, OntoTheRoofAlong)),
                OnTheRoof(new WorldPoint(AlongTheRoofAcross, PlankMetres, RoofRunStartsAlong)),
                OnTheRoof(new WorldPoint(AlongTheRoofAcross, PlankMetres, RoofRunEndsAlong)),
                coil[0],
                coil[1]
            };
        }

        private static WorldPoint OverTheFrontEdge()
        {
            var edge = OnTheRoof(new WorldPoint(UpTheCabinFrontAcross, PlankMetres, SparrowForm.CabinFrontAlong - Rope.RadiusMetres));
            return new WorldPoint(edge.East, edge.Height + OverTheEdgeMetres, edge.North);
        }

        private static WorldPoint OnTheRoof(WorldPoint point)
        {
            return new WorldPoint(point.East, Rope.RestingOn(SparrowForm.RoofHeightAt(point.East)), point.North);
        }
    }
}
