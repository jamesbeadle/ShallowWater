using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Houses
{
    public static class RowPlan
    {
        private const double ShareJoinedToTheNext = 0.65;
        private const double NarrowestGapMetres = 1.2;
        private const double GapSpreadMetres = 1.8;
        private const double Half = 0.5;

        public static IReadOnlyList<Run> RunsOn(Plot row)
        {
            var random = new Random(RowSeed.Of(row));
            var street = Street.Chosen(random);
            var layouts = LaidOut(row, street, random);
            var offset = (row.LengthMetres - EndOf(layouts)) * Half;
            return RunBuilding.Built(row, layouts, offset, random);
        }

        private static List<RunLayout> LaidOut(Plot row, Street street, Random random)
        {
            var layouts = new List<RunLayout>();
            while (true)
            {
                var style = street.Pick(random, row.DepthMetres);
                var isJoined = layouts.Any() && IsJoinedTo(layouts.Last(), style, random);
                var isFirst = !layouts.Any();
                var along = isJoined || isFirst ? EndOf(layouts) : EndOf(layouts) + GapMetres(random);
                var frontages = Frontages(style, random, row.LengthMetres - along);
                var isFull = !frontages.Any();
                if (isFull) return layouts;
                layouts.Add(new RunLayout(style, along, frontages, isJoined));
            }
        }

        private static bool IsJoinedTo(RunLayout previous, HouseStyle style, Random random)
        {
            var standsAlone = style.StandsAlone || previous.StandsAlone;
            return !standsAlone && random.NextDouble() < ShareJoinedToTheNext;
        }

        private static double EndOf(List<RunLayout> layouts)
        {
            var isEmpty = !layouts.Any();
            if (isEmpty) return 0;
            var last = layouts.Last();
            return last.EndMetres;
        }

        private static double GapMetres(Random random)
        {
            return NarrowestGapMetres + random.NextDouble() * GapSpreadMetres;
        }

        private static List<double> Frontages(HouseStyle style, Random random, double roomMetres)
        {
            var frontages = new List<double>();
            var houses = style.HousesInARun(random);
            for (var house = 0; house < houses; house++)
            {
                var frontage = style.FrontageMetres(random);
                var isTooLong = frontages.Sum() + frontage > roomMetres;
                if (isTooLong) return frontages;
                frontages.Add(frontage);
            }
            return frontages;
        }
    }
}
