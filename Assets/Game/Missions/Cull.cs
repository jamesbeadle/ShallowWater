using System;

namespace ShallowWater.Game.Missions
{
    public sealed class Cull
    {
        public Cull(int quarryCount)
        {
            QuarryCount = quarryCount;
        }

        public int QuarryCount { get; }
        public int Killed { get; private set; }
        public bool IsComplete => Killed >= QuarryCount;
        public string Words => $"{Killed} of {QuarryCount}";

        public void Record()
        {
            Killed = Math.Min(QuarryCount, Killed + 1);
        }
    }
}
