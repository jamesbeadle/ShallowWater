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

        public static WorldPoint StudOn(BoatMotion boat)
        {
            var stud = boat.PointOf(CentreStud.OnTheHull);
            return new WorldPoint(stud.East, StudTieMetres, stud.North);
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
