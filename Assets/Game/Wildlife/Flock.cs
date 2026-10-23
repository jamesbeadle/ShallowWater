using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public sealed class Flock
    {
        private readonly Random random;
        private readonly List<Quarry> all = new List<Quarry>();

        private Flock(int seed)
        {
            random = new Random(seed);
        }

        public IReadOnlyList<Quarry> All => all;
        public IEnumerable<Quarry> Alive => all.Where(quarry => quarry.IsAlive);
        public int Count => all.Count;

        public static Flock Stocked(IEnumerable<Stocking> stockings, int seed)
        {
            var flock = new Flock(seed);
            foreach (var stocking in stockings) flock.Stock(stocking);
            return flock;
        }

        public bool Has(Quarry quarry)
        {
            return all.Contains(quarry);
        }

        public void Live(double seconds)
        {
            foreach (var quarry in all) quarry.Live(seconds, random);
        }

        public void Startle(WorldPoint shotFrom, double hearingMetres)
        {
            foreach (var quarry in Alive)
            {
                var isWithinHearing = Offset.Between(shotFrom, quarry.Position).Length < hearingMetres;
                if (isWithinHearing) quarry.Startle(random);
            }
        }

        private void Stock(Stocking stocking)
        {
            var habit = QuarryHabits.Of(stocking.Kind);
            for (var each = 0; each < stocking.Count; each++) all.Add(Hatched(stocking, habit));
        }

        private Quarry Hatched(Stocking stocking, Habit habit)
        {
            return new Quarry(stocking.Kind, new Roaming(stocking.Stations, habit, random), new Calling(habit, random));
        }
    }
}
