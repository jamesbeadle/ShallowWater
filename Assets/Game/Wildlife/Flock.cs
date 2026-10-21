using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public sealed class Flock
    {
        private const int Seed = 1938;

        private static readonly QuarryKind[] OnTheFarm =
        {
            QuarryKind.Rook, QuarryKind.Rook, QuarryKind.Rook, QuarryKind.Crow, QuarryKind.Crow, QuarryKind.Magpie,
            QuarryKind.Rat, QuarryKind.Rat, QuarryKind.Rat, QuarryKind.Rat
        };

        private readonly Random random = new Random(Seed);
        private readonly List<Quarry> all = new List<Quarry>();

        public IReadOnlyList<Quarry> All => all;
        public IEnumerable<Quarry> Alive => all.Where(quarry => quarry.IsAlive);
        public int Count => all.Count;

        public static Flock Stocked(IReadOnlyList<WorldPoint> perches, IReadOnlyList<WorldPoint> ratRuns)
        {
            var flock = new Flock();
            foreach (var kind in OnTheFarm)
            {
                var stations = QuarryHabits.IsACorvid(kind) ? perches : ratRuns;
                flock.all.Add(new Quarry(kind, new Roaming(stations, QuarryHabits.Of(kind), flock.random)));
            }
            return flock;
        }

        public void Live(double seconds)
        {
            foreach (var quarry in all) quarry.Live(seconds, random);
        }
    }
}
