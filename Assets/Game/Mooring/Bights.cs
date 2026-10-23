using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class Bights
    {
        private const double WavelengthMetres = 2.4;
        private const double WidestMetres = 0.5;
        private const double FullTurnRadians = 2 * Math.PI;
        private const double HalfTurnRadians = Math.PI;
        private const double NoRun = 1e-9;

        public static List<WorldPoint> Thrown(Lead lead, Hang hang, double slackMetres)
        {
            var heights = hang.Heights;
            var points = Enumerable.Range(0, lead.Count).Select(index => lead.At(index, heights[index])).ToList();
            var runs = RestingRun.In(lead, hang).Where(run => run.IsLevel).ToList();
            var lyingMetres = runs.Sum(run => run.LengthMetres);
            var isLyingAnywhere = lyingMetres > NoRun;
            if (!isLyingAnywhere) return points;
            foreach (var run in runs) Throw(lead, run, points, slackMetres * run.LengthMetres / lyingMetres);
            return points;
        }

        private static void Throw(Lead lead, RestingRun run, List<WorldPoint> points, double slackMetres)
        {
            var isTooShortToBight = run.LengthMetres < NoRun;
            if (isTooShortToBight) return;
            var amplitude = Math.Min(WidestMetres, WavelengthMetres / Math.PI * Math.Sqrt(slackMetres / run.LengthMetres));
            for (var index = run.First; index <= run.Last; index++)
            {
                var along = lead[index].Along - run.StartMetres;
                var wave = Math.Sin(FullTurnRadians * along / WavelengthMetres) * Math.Sin(HalfTurnRadians * along / run.LengthMetres);
                points[index] = lead.RestingBeside(index, amplitude * wave);
            }
        }
    }
}
