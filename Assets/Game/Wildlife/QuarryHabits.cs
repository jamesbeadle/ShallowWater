using System.Collections.Generic;

namespace ShallowWater.Game.Wildlife
{
    public static class QuarryHabits
    {
        private static readonly Habit Corvid = new Habit(0.22, 6, 3, 12, 1.5, 15, 60);
        private static readonly Habit Rat = new Habit(0.14, 2.5, 1, 5, 0, Habit.Silent, Habit.Silent);

        private static readonly Dictionary<QuarryKind, Habit> ByKind = new Dictionary<QuarryKind, Habit>
        {
            { QuarryKind.Crow, Corvid }, { QuarryKind.Rook, Corvid }, { QuarryKind.Magpie, Corvid }, { QuarryKind.Rat, Rat }
        };

        public static Habit Of(QuarryKind kind)
        {
            return ByKind[kind];
        }

        public static bool IsACorvid(QuarryKind kind)
        {
            return kind != QuarryKind.Rat;
        }
    }
}
