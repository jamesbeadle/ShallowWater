using System;

namespace ShallowWater.Game.Mooring
{
    public sealed class TyingUp
    {
        private const double JobSeconds = 3.6;
        private const double LineMadeShare = 0.55;
        private const double Done = 1;
        private const double Started = 0;

        public MooringStage Stage { get; private set; } = MooringStage.Loose;
        public double Share { get; private set; }
        public bool IsTying => Stage == MooringStage.Tying;
        public bool IsCastingOff => Stage == MooringStage.CastingOff;
        public bool IsBusy => IsTying || IsCastingOff;
        public bool IsFast => Stage == MooringStage.Fast;
        public bool IsLoose => Stage == MooringStage.Loose;
        public bool IsLineMade => IsFast || (IsTying && Share >= LineMadeShare) || (IsCastingOff && Share < LineMadeShare);

        public void Begin()
        {
            if (IsBusy) return;
            Stage = IsLoose ? MooringStage.Tying : MooringStage.CastingOff;
            Share = Started;
        }

        public void Work(double seconds)
        {
            if (!IsBusy) return;
            Share = Math.Min(Done, Share + seconds / JobSeconds);
            var isDone = Share >= Done;
            if (!isDone) return;
            Stage = IsTying ? MooringStage.Fast : MooringStage.Loose;
            Share = Started;
        }
    }
}
