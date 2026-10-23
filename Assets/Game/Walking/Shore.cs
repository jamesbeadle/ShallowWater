using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using CanalPound = ShallowWater.Game.Pound.Pound;

namespace ShallowWater.Game.Walking
{
    public sealed class Shore
    {
        private const double RiseMetres = Heights.BankTopMetres - Heights.WaterMetres;
        private const double Whole = 1;

        private readonly IReadOnlyList<Stretch> stretches;

        private Shore(IReadOnlyList<Stretch> stretches)
        {
            this.stretches = stretches;
        }

        public static Shore Along(CanalPound pound)
        {
            return new Shore(PoundStretches.Of(pound));
        }

        public bool IsWaterAt(WaterPosition position)
        {
            return Math.Abs(position.Across) < PoundLimits.ChannelHalfWidthMetres;
        }

        public double HeightAt(WaterPosition position)
        {
            if (IsWaterAt(position)) return Heights.WaterMetres;
            var fromTheCentre = Math.Abs(position.Across);
            var upTheEdge = (fromTheCentre - PoundLimits.ChannelHalfWidthMetres) / CanalSection.SoftEdgeWidthMetres;
            var isOnTheSoftEdge = upTheEdge < Whole && !HasHardEdgeAt(position);
            if (isOnTheSoftEdge) return Heights.WaterMetres + RiseMetres * upTheEdge;
            return BankProfile.HeightAt(fromTheCentre);
        }

        private bool HasHardEdgeAt(WaterPosition position)
        {
            var isTheTowpathSide = Math.Sign(position.Across) == PoundLimits.TowpathSide;
            return isTheTowpathSide && stretches.Any(stretch => stretch.IsAround(position.Along) && stretch.HasHardEdge);
        }
    }
}
