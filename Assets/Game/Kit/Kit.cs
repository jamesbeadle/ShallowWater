using ShallowWater.Game.Guns;

namespace ShallowWater.Game.Kit
{
    public sealed class Kit
    {
        private const int AskewsPenceThisMorning = 210;

        public Rifles Rifles { get; } = new Rifles();
        public Purse Purse { get; } = new Purse(AskewsPenceThisMorning);
    }
}
