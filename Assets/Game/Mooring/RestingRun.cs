using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Mooring
{
    public readonly struct RestingRun
    {
        private const int NoRun = -1;
        private const double LevelMetres = 0.05;

        private RestingRun(Lead lead, int first, int last)
        {
            First = first;
            Last = last;
            StartMetres = lead[first].Along;
            LengthMetres = lead[last].Along - StartMetres;
            IsLevel = RiseOver(lead, first, last) < LevelMetres;
        }

        public int First { get; }
        public int Last { get; }
        public double StartMetres { get; }
        public double LengthMetres { get; }
        public bool IsLevel { get; }

        private static double RiseOver(Lead lead, int first, int last)
        {
            var lowest = double.MaxValue;
            var highest = double.MinValue;
            for (var index = first; index <= last; index++)
            {
                var resting = lead[index].RestingHeight;
                lowest = Math.Min(lowest, resting);
                highest = Math.Max(highest, resting);
            }
            return highest - lowest;
        }

        public static List<RestingRun> In(Lead lead, Hang hang)
        {
            var runs = new List<RestingRun>();
            var first = NoRun;
            for (var index = 0; index < lead.Count; index++)
            {
                var isResting = hang.IsRestingAt(index);
                var isStarting = isResting && first == NoRun;
                var isEnding = !isResting && first != NoRun;
                if (isStarting) first = index;
                if (!isEnding) continue;
                runs.Add(new RestingRun(lead, first, index - 1));
                first = NoRun;
            }
            return runs;
        }
    }
}
