using System;

namespace ShallowWater.Game.Guns
{
    public sealed class RookRifle
    {
        public const double RangeMetres = 90;
        public const double BalanceFromTheButtMetres = 0.45;
        private const double ReloadSeconds = 1.6;
        private const double Loaded = 0;

        private double untilReloadedSeconds = Loaded;

        public bool IsLoaded => untilReloadedSeconds <= Loaded;
        public bool IsShouldered { get; private set; }

        public void Shoulder(bool isShouldered)
        {
            IsShouldered = isShouldered;
        }

        public void Work(double seconds)
        {
            untilReloadedSeconds = Math.Max(Loaded, untilReloadedSeconds - seconds);
        }

        public void Fire()
        {
            if (!IsLoaded) return;
            untilReloadedSeconds = ReloadSeconds;
        }
    }
}
