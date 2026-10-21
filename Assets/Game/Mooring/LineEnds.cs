using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class LineEnds
    {
        public static WorldPoint OnTheBoat(MooringLine line, BoatMotion boat, GroundPoint feet)
        {
            if (line == MooringLine.Centre) return CentreLine.StudOn(boat);
            if (line == MooringLine.Bow) return BowLine.StudOn(boat);
            return SternLine.TieOn(boat, SternLine.DollyNearest(boat, feet));
        }

        public static HullPoint OnTheHull(MooringLine line, BoatMotion boat, GroundPoint feet)
        {
            if (line == MooringLine.Centre) return CentreStud.OnTheHull;
            if (line == MooringLine.Bow) return BowLine.Stud;
            return SternLine.DollyNearest(boat, feet);
        }

        public static bool CanReach(MooringLine line, BoatMotion boat, GroundPoint feet)
        {
            if (line == MooringLine.Centre) return true;
            if (line == MooringLine.Bow) return BowLine.CanReach(boat, feet);
            return SternLine.CanReach(boat, feet);
        }
    }
}
