using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Woods
{
    public static class TreeForms
    {
        public static readonly IReadOnlyList<TreeForm> All = new[]
        {
            new TreeForm(OakHabits.Woodland, 1101), new TreeForm(OakHabits.Woodland, 1102), new TreeForm(OakHabits.Young, 1103),
            new TreeForm(OakHabits.OpenGrown, 1104), new TreeForm(AshHabits.Woodland, 2101), new TreeForm(AshHabits.Young, 2102),
            new TreeForm(BirchHabits.Mature, 3101), new TreeForm(BirchHabits.Young, 3102),
            new TreeForm(PineHabits.Plantation, 4101), new TreeForm(PineHabits.Plantation, 4102),
            new TreeForm(HazelHabits.Coppice, 5101), new TreeForm(HazelHabits.Coppice, 5102)
        };

        public static int OneOf(Habit habit, Random random)
        {
            var matching = Enumerable.Range(0, All.Count).Where(form => All[form].Habit == habit).ToList();
            return matching[random.Next(matching.Count)];
        }

        public static int StandInFor(int form)
        {
            var species = All[form].Species;
            return Enumerable.Range(0, All.Count).First(candidate => All[candidate].Species == species);
        }
    }
}
