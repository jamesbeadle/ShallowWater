using System;

namespace ShallowWater.Game.Mooring
{
    public sealed class LineWork
    {
        private const double JobSeconds = 3.6;
        private const double LineMadeShare = 0.55;
        private const double Done = 1;
        private const double Started = 0;

        private LineWork(MooringLine line, bool isMakingFast)
        {
            Line = line;
            IsMakingFast = isMakingFast;
        }

        public MooringLine Line { get; }
        public bool IsMakingFast { get; }
        public bool IsLettingGo => !IsMakingFast;
        public double Share { get; private set; } = Started;
        public bool IsDone => Share >= Done;
        public bool IsLineMade => IsMakingFast ? Share >= LineMadeShare : Share < LineMadeShare;

        public static LineWork MakingFast(MooringLine line)
        {
            return new LineWork(line, true);
        }

        public static LineWork LettingGo(MooringLine line)
        {
            return new LineWork(line, false);
        }

        public void Work(double seconds)
        {
            Share = Math.Min(Done, Share + seconds / JobSeconds);
        }
    }
}
