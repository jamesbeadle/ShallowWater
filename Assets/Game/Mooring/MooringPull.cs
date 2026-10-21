using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Mooring
{
    public static class MooringPull
    {
        private static readonly GroundPoint StillPost = new GroundPoint(0, 0);

        public static BoatForces On(Mooring mooring, BoatMotion boat)
        {
            var pull = BoatForces.None;
            var hand = mooring.Hand;
            if (mooring.IsLineInHand) pull += LinePull.On(boat, CentreStud.OnTheHull, hand.Place, hand.Velocity, CentreLine.LengthMetres);
            foreach (var line in mooring.LinesLaid) pull += OnTheLaidLine(mooring, boat, line);
            return pull;
        }

        private static BoatForces OnTheLaidLine(Mooring mooring, BoatMotion boat, MooringLine line)
        {
            var laid = mooring.LaidAt(line);
            var fixedEnd = LineEnds.OnTheHull(line, boat, laid.Anchor);
            return LinePull.On(boat, fixedEnd, laid.Anchor, StillPost, laid.LengthMetres);
        }
    }
}
