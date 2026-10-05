using System;

namespace ShallowWater.Game.Fields
{
    public static class FootpathRule
    {
        private const double FootpathShare = 0.6;
        private const double HeldByTheLowerShare = FootpathShare / 2;
        private const int Salt = 1938;

        public static bool IsHeldBy(int farm, int neighbour)
        {
            var lower = Math.Min(farm, neighbour);
            var higher = Math.Max(farm, neighbour);
            var pairing = new Random(lower * Salt + higher);
            var roll = pairing.NextDouble();
            var hasFootpath = roll < FootpathShare;
            var isHeldByTheLower = roll < HeldByTheLowerShare;
            var isThisSide = (farm == lower) == isHeldByTheLower;
            return hasFootpath && isThisSide;
        }
    }
}
