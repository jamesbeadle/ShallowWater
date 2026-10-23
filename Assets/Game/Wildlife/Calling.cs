using System;

namespace ShallowWater.Game.Wildlife
{
    public sealed class Calling
    {
        private const double Now = 0;
        private const double AlarmWithinSeconds = 1.2;

        private readonly Habit habit;
        private double untilTheNextSeconds;

        public Calling(Habit habit, Random random)
        {
            this.habit = habit;
            untilTheNextSeconds = Next(random);
        }

        public bool IsCalling { get; private set; }

        public void Work(double seconds, Random random)
        {
            IsCalling = false;
            if (!habit.IsVocal) return;
            untilTheNextSeconds -= seconds;
            var isTime = untilTheNextSeconds <= Now;
            if (!isTime) return;
            IsCalling = true;
            untilTheNextSeconds = Next(random);
        }

        public void Alarm(Random random)
        {
            if (!habit.IsVocal) return;
            untilTheNextSeconds = random.NextDouble() * AlarmWithinSeconds;
        }

        private double Next(Random random)
        {
            return habit.ShortestCallSeconds + random.NextDouble() * (habit.LongestCallSeconds - habit.ShortestCallSeconds);
        }
    }
}
