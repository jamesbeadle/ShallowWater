using System;

namespace ShallowWater.Game.Guns
{
    public sealed class SingleShot : Feed
    {
        private const double Reloaded = 0;
        private const string Loaded = "loaded";
        private const string BoltOpen = "bolt open";
        private const string NothingLeftToLoad = "nothing left to load";

        private readonly double reloadSeconds;
        private double untilReloadedSeconds = Reloaded;

        public SingleShot(Cartridges cartridges, double reloadSeconds) : base(cartridges)
        {
            this.reloadSeconds = reloadSeconds;
        }

        public override bool IsLoaded => untilReloadedSeconds <= Reloaded && Cartridges.HasAny;

        public override string Words
        {
            get
            {
                if (IsLoaded) return Loaded;
                return Cartridges.HasAny ? BoltOpen : NothingLeftToLoad;
            }
        }

        public override void Work(double seconds)
        {
            untilReloadedSeconds = Math.Max(Reloaded, untilReloadedSeconds - seconds);
        }

        public override void Fire()
        {
            Cartridges.Spend();
            untilReloadedSeconds = reloadSeconds;
        }
    }
}
