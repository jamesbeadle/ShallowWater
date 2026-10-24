using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.Mooring
{
    public static class CentreLine
    {
        public const double LengthMetres = 11;
        private const double HeldBackMetres = LengthMetres + LinePull.GiveMetres + LineInHand.HandOutMetres;
        private static readonly double StudTieMetres = Heights.WaterMetres + CentreStud.TieHeightMetres;

        public static WorldPoint StudOn(BoatMotion boat)
        {
            var stud = boat.PointOf(CentreStud.OnTheHull);
            return new WorldPoint(stud.East, StudTieMetres, stud.North);
        }

        public static Tether TetherOn(BoatMotion boat)
        {
            return new Tether(boat.PointOf(CentreStud.OnTheHull), HeldBackMetres);
        }
    }
}
