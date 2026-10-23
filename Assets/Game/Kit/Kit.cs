namespace ShallowWater.Game.Kit
{
    public sealed class Kit
    {
        private const int AskewsPenceThisMorning = 210;

        public Cartridges Cartridges { get; } = Cartridges.FullBox();
        public Purse Purse { get; } = new Purse(AskewsPenceThisMorning);
    }
}
