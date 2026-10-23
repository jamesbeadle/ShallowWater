using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class DrapedLine
    {
        private const double Taut = 0;
        private const double SteepestCurvature = 50;
        private const int Bisections = 20;
        private const double Half = 0.5;

        public static List<WorldPoint> Over(Lead lead, double fromHeight, double toHeight, double lengthMetres, double leastSlackMetres)
        {
            var tight = Hang.Of(lead, fromHeight, toHeight, Taut);
            var wanted = Math.Max(lengthMetres, tight.LengthMetres + leastSlackMetres);
            var isStretched = tight.LengthMetres >= wanted;
            if (isStretched) return Bights.Thrown(lead, tight, Taut);
            var hang = Slackened(lead, fromHeight, toHeight, wanted);
            return Bights.Thrown(lead, hang, Math.Max(Taut, wanted - hang.LengthMetres));
        }

        private static Hang Slackened(Lead lead, double fromHeight, double toHeight, double wantedMetres)
        {
            var slackest = Hang.Of(lead, fromHeight, toHeight, SteepestCurvature);
            var isAllTheHangCanTake = slackest.LengthMetres <= wantedMetres;
            if (isAllTheHangCanTake) return slackest;
            var tauter = Taut;
            var slacker = SteepestCurvature;
            var hang = slackest;
            for (var bisection = 0; bisection < Bisections; bisection++)
            {
                var curvature = (tauter + slacker) * Half;
                hang = Hang.Of(lead, fromHeight, toHeight, curvature);
                var isTooLong = hang.LengthMetres > wantedMetres;
                slacker = isTooLong ? curvature : slacker;
                tauter = isTooLong ? tauter : curvature;
            }
            return hang;
        }
    }
}
