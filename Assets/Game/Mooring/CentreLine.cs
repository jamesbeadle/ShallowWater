using System;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class CentreLine
    {
        public const double LengthMetres = 11;
        private const double HeldBackMetres = LengthMetres + LinePull.GiveMetres + LineInHand.HandOutMetres;
        private static readonly double StudTieMetres = Heights.WaterMetres + CentreStud.TieHeightMetres;
        private static readonly double SideClothTopMetres = Hull.GunwaleAt(CentreStud.Along) + SparrowForm.SideClothMetres;
        private static readonly double ClothEdgeMetres = Heights.WaterMetres + SideClothTopMetres + HangingLine.RadiusMetres;
        private static readonly double HalfBeamAtTheStud = Hull.HalfBeamAt(CentreStud.Along);

        public static WorldPoint StudOn(BoatMotion boat)
        {
            var stud = boat.PointOf(CentreStud.OnTheHull);
            return new WorldPoint(stud.East, StudTieMetres, stud.North);
        }

        public static WorldPoint OverTheSideTowards(BoatMotion boat, GroundPoint hand)
        {
            var stud = boat.PointOf(CentreStud.OnTheHull);
            var side = Math.Sign((hand - stud).Dot(boat.Starboard));
            var isAbeam = side != BoatSides.Amidships;
            var towardsTheHand = isAbeam ? side : BoatSides.Port;
            var edge = boat.PointOf(new HullPoint(CentreStud.Along, towardsTheHand * HalfBeamAtTheStud));
            return new WorldPoint(edge.East, ClothEdgeMetres, edge.North);
        }

        public static double SlackBeyond(WorldPoint stud, WorldPoint edge, WorldPoint hand)
        {
            var onTheBoat = Offset.Between(stud, edge).Length;
            var overTheWater = Offset.Between(edge, hand).Length;
            return LengthMetres - onTheBoat - overTheWater;
        }

        public static GroundPoint HeldBack(GroundPoint feet, BoatMotion boat)
        {
            var stud = boat.PointOf(CentreStud.OnTheHull);
            var away = feet - stud;
            var isWithinTheLine = away.Length <= HeldBackMetres;
            if (isWithinTheLine) return feet;
            return stud + away.Normalised * HeldBackMetres;
        }
    }
}
