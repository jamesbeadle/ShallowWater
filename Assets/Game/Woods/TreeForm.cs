namespace ShallowWater.Game.Woods
{
    public sealed class TreeForm
    {
        public TreeForm(Habit habit, int seed)
        {
            Habit = habit;
            Seed = seed;
        }

        public Habit Habit { get; }
        public int Seed { get; }
        public TreeSpecies Species => Habit.Species;
    }
}
