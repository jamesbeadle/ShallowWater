using System;

namespace ShallowWater.Game.Day
{
    public static class Shares
    {
        public static double Between(double value, double from, double to)
        {
            var share = (value - from) / (to - from);
            return Math.Max(0, Math.Min(1, share));
        }
    }
}
