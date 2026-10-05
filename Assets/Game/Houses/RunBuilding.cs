using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Houses
{
    public static class RunBuilding
    {
        private const double ShareHipped = 0.6;
        private const int EveryOther = 2;
        private const int Even = 0;
        private const int OneHouse = 1;

        public static IReadOnlyList<Run> Built(Plot row, IReadOnlyList<RunLayout> layouts, double offsetMetres, Random random)
        {
            var runs = new List<Run>();
            var setBack = 0.0;
            for (var index = 0; index < layouts.Count; index++)
            {
                var layout = layouts[index];
                var isJoinedOnTheRight = index + 1 < layouts.Count && layouts[index + 1].IsJoinedToThePrevious;
                var neighbours = new Neighbours(layout.IsJoinedToThePrevious, isJoinedOnTheRight);
                var style = layout.Style;
                setBack = layout.IsJoinedToThePrevious ? setBack : SetBack(row, style, random);
                var plot = row.Part(offsetMetres + layout.AlongMetres, setBack, layout.LengthMetres, style.DepthMetres);
                runs.Add(new Run(plot, style, HousesOf(layout, random), FinishFor(layout, neighbours, random), neighbours));
            }
            return runs;
        }

        private static double SetBack(Plot row, HouseStyle style, Random random)
        {
            var room = row.DepthMetres - style.TotalDepthMetres;
            return Math.Max(0, Math.Min(room, style.FrontSetBackMetres(random)));
        }

        private static Finish FinishFor(RunLayout layout, Neighbours neighbours, Random random)
        {
            var style = layout.Style;
            var isSingle = layout.HouseCount == OneHouse;
            var isHipped = style.CanBeHipped && isSingle && neighbours.IsDetached && random.NextDouble() < ShareHipped;
            return new Finish(style.EavesMetres(random), style.Walls(random), style.Roof(random), isHipped);
        }

        private static List<House> HousesOf(RunLayout layout, Random random)
        {
            var style = layout.Style;
            var houses = new List<House>();
            var along = 0.0;
            foreach (var frontage in layout.Frontages)
            {
                var isDoorOnTheRight = houses.Count % EveryOther == Even;
                houses.Add(new House(along, frontage, isDoorOnTheRight, Paintwork.Chosen(random), style.ExtrasFor(random)));
                along += frontage;
            }
            return houses;
        }
    }
}
