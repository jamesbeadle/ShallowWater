using System.Collections.Generic;

namespace ShallowWater.Game.Missions
{
    public sealed class FarmRifleMission
    {
        private const string CullHeading = "Rooks and rats: ";
        private const string AllDown = "All of them. Tell the farmer.";

        private static readonly Dictionary<FarmRifleStage, FarmRifleStage> WhatASpeechBrings = new Dictionary<FarmRifleStage, FarmRifleStage>
        {
            { FarmRifleStage.NotYetAsked, FarmRifleStage.Culling },
            { FarmRifleStage.Cleared, FarmRifleStage.Paid }
        };

        public FarmRifleMission(int quarryCount)
        {
            Cull = new Cull(quarryCount);
        }

        public FarmRifleStage Stage { get; private set; } = FarmRifleStage.NotYetAsked;
        public Cull Cull { get; }
        public Lead Lead => PubLeads.HollandAtTheThreeTuns;
        public bool IsRifleInHand => Stage != FarmRifleStage.NotYetAsked;
        public bool IsRifleKept => Stage == FarmRifleStage.Paid;
        public bool HasTheLead => Stage == FarmRifleStage.Paid;
        public bool IsCulling => Stage == FarmRifleStage.Culling;
        public bool HasBegun => Stage != FarmRifleStage.NotYetAsked;

        public string Words
        {
            get
            {
                if (IsCulling) return CullHeading + Cull.Words;
                if (Stage == FarmRifleStage.Cleared) return AllDown;
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
        }

        public void Killed()
        {
            Cull.Record();
            var isTheLastOne = IsCulling && Cull.IsComplete;
            if (isTheLastOne) Stage = FarmRifleStage.Cleared;
        }
    }
}
