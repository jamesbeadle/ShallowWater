using System;

namespace ShallowWater.Game.Boat
{
    public static class HullDrag
    {
        private const double SkinNewtonsPerMetrePerSecond = 300;
        private const double WaveNewtonsPerSpeedSquared = 70;
        private const double ChannelLimitMetresPerSecond = 2.8;
        private const double TightestSqueeze = 0.12;
        private const double SternFirstBluffness = 2.2;
        private const double Stopped = 0;

        public static double At(double aheadMetresPerSecond)
        {
            var speed = Math.Abs(aheadMetresPerSecond);
            var limitShare = speed / ChannelLimitMetresPerSecond;
            var squeeze = Math.Max(TightestSqueeze, 1 - limitShare * limitShare);
            var drag = (SkinNewtonsPerMetrePerSecond * speed + WaveNewtonsPerSpeedSquared * speed * speed) / squeeze;
            var isSternFirst = aheadMetresPerSecond < Stopped;
            var bluffness = isSternFirst ? SternFirstBluffness : 1;
            return Math.Sign(aheadMetresPerSecond) * drag * bluffness;
        }
    }
}
