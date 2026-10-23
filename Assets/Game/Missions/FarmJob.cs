using System.Collections.Generic;
using ShallowWater.Game.Wildlife;

namespace ShallowWater.Game.Missions
{
    public sealed class FarmJob
    {
        private const string CullHeading = "Rooks and rats: ";
        private const string AllDown = "All of them. Tell the farmer.";

        private static readonly Dictionary<FarmJobStage, FarmJobStage> WhatASpeechBrings = new Dictionary<FarmJobStage, FarmJobStage>
        {
            { FarmJobStage.NotYetAsked, FarmJobStage.Culling },
            { FarmJobStage.Cleared, FarmJobStage.Paid }
        };

        private readonly Flock flock;

        public FarmJob(Flock flock)
        {
            this.flock = flock;
            Cull = new Cull(flock.Count);
        }

        public FarmJobStage Stage { get; private set; } = FarmJobStage.NotYetAsked;
        public Cull Cull { get; }
        public Lead Lead => PubLeads.HollandAtTheThreeTuns;
        public bool HasTheLead => Stage == FarmJobStage.Paid;
        public bool IsCulling => Stage == FarmJobStage.Culling;
        public bool HasBegun => Stage != FarmJobStage.NotYetAsked;

        public string Words
        {
            get
            {
                if (IsCulling) return CullHeading + Cull.Words;
                if (Stage == FarmJobStage.Cleared) return AllDown;
                return HasTheLead ? Lead.Words : string.Empty;
            }
        }

        public IReadOnlyList<string> LinesNow()
        {
            return FarmerLines.For(Stage);
        }

        public void Spoken()
        {
            var isAStep = WhatASpeechBrings.TryGetValue(Stage, out var next);
            if (isAStep) Stage = next;
            Settle();
        }

        public void Killed(Quarry struck)
        {
            var isOneOfThem = flock.Has(struck);
            if (!isOneOfThem) return;
            Cull.Record();
            Settle();
        }

        private void Settle()
        {
            var isTheLastOne = IsCulling && Cull.IsComplete;
            if (isTheLastOne) Stage = FarmJobStage.Cleared;
        }
    }
}
