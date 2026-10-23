using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Mooring
{
    public static class LineEnds
    {
        public static LineEnd OnTheBoat(MooringLine line, BoatMotion boat, GroundPoint feet)
        {
            if (line == MooringLine.Centre) return CentreStudOn(boat);
            if (line == MooringLine.Bow) return BowStudOn(boat);
            return DollyOn(boat, feet);
        }

        private static LineEnd CentreStudOn(BoatMotion boat)
        {
            var tie = CentreLine.StudOn(boat);
            return new LineEnd(tie, CentreStud.RadiusMetres);
        }

        private static LineEnd BowStudOn(BoatMotion boat)
        {
            var tie = BowLine.StudOn(boat);
            return new LineEnd(tie, ForeEnd.StudRadiusMetres);
        }

        private static LineEnd DollyOn(BoatMotion boat, GroundPoint feet)
        {
            var dolly = SternLine.DollyNearest(boat, feet);
            var tie = SternLine.TieOn(boat, dolly);
            return new LineEnd(tie, Dollies.RadiusMetres);
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
