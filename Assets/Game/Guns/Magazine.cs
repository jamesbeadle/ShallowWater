using System;

namespace ShallowWater.Game.Guns
{
    public sealed class Magazine : Feed
    {
        private const double Ready = 0;
        private const int Empty = 0;
        private const int OneRound = 1;
        private const string InTheMagazine = " in the magazine";
        private const string Changing = "loading";
        private const string MagazineEmpty = "magazine empty";

        private readonly int capacity;
        private readonly double boltCycleSeconds;
        private readonly double changeSeconds;
        private double untilReadySeconds = Ready;

        public Magazine(Cartridges cartridges, int capacity, double boltCycleSeconds, double changeSeconds) : base(cartridges)
        {
            this.capacity = capacity;
            this.boltCycleSeconds = boltCycleSeconds;
            this.changeSeconds = changeSeconds;
            Rounds = cartridges.Take(capacity);
        }

        public int Rounds { get; private set; }
        public override bool IsLoaded => untilReadySeconds <= Ready && HasRounds;
        private bool HasRounds => Rounds > Empty;
        private bool IsChanging => !HasRounds && untilReadySeconds > Ready;

        public override string Words
        {
            get
            {
                if (IsChanging) return Changing;
                return HasRounds ? Rounds + InTheMagazine : MagazineEmpty;
            }
        }

        public override void Work(double seconds)
        {
            untilReadySeconds = Math.Max(Ready, untilReadySeconds - seconds);
            var isFreshMagazineIn = !HasRounds && untilReadySeconds <= Ready;
            if (isFreshMagazineIn) Rounds = Cartridges.Take(capacity);
        }

        public override void Fire()
        {
            Rounds -= OneRound;
            untilReadySeconds = HasRounds ? boltCycleSeconds : changeSeconds;
        }
    }
}
