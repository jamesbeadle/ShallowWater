using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class BowLine
    {
        public const double LengthMetres = 9;
        public static readonly HullPoint Stud = new HullPoint(ForeEnd.StudAlong, BoatSides.Amidships);
        private static readonly double TieMetres = Hull.DeckAt(ForeEnd.StudAlong) + ForeEnd.StudPinHeightMetres;

        public static WorldPoint StudOn(BoatMotion boat)
        {
            var stud = boat.PointOf(Stud);
            return new WorldPoint(stud.East, Heights.WaterMetres + TieMetres, stud.North);
        }

        public static bool CanReach(BoatMotion boat, GroundPoint feet)
        {
            return boat.PointOf(Stud).DistanceTo(feet) < LengthMetres;
        }
    }
}
