using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Mooring
{
    public sealed class Hang
    {
        private const double TouchMetres = 0.001;
        private const int NoContact = -1;

        private readonly Lead lead;
        private readonly double curvature;
        private readonly double[] heights;
        private readonly bool[] isResting;

        private Hang(Lead lead, double fromHeight, double toHeight, double curvature)
        {
            this.lead = lead;
            this.curvature = curvature;
            heights = new double[lead.Count];
            isResting = new bool[lead.Count];
            heights[0] = fromHeight;
            heights[lead.Count - 1] = toHeight;
            Between(0, lead.Count - 1);
            LengthMetres = Measured();
        }

        public static Hang Of(Lead lead, double fromHeight, double toHeight, double curvature)
        {
            return new Hang(lead, fromHeight, toHeight, curvature);
        }

        public IReadOnlyList<double> Heights => heights;
        public double LengthMetres { get; }

        public bool IsRestingAt(int index)
        {
            return isResting[index];
        }

        private void Between(int from, int to)
        {
            var deepest = TouchMetres;
            var contact = NoContact;
            for (var index = from + 1; index < to; index++)
            {
                heights[index] = Hanging(from, to, index);
                var below = lead[index].RestingHeight - heights[index];
                if (below <= deepest) continue;
                deepest = below;
                contact = index;
            }
            var isClear = contact == NoContact;
            if (isClear) return;
            heights[contact] = lead[contact].RestingHeight;
            isResting[contact] = true;
            Between(from, contact);
            Between(contact, to);
        }

        private double Hanging(int from, int to, int index)
        {
            var along = lead[index].Along;
            var start = lead[from].Along;
            var end = lead[to].Along;
            var share = (along - start) / (end - start);
            var chord = heights[from] + (heights[to] - heights[from]) * share;
            return chord - curvature * (along - start) * (end - along);
        }

        private double Measured()
        {
            var length = 0.0;
            for (var index = 1; index < lead.Count; index++)
            {
                var run = lead[index].Along - lead[index - 1].Along;
                var rise = heights[index] - heights[index - 1];
                length += Math.Sqrt(run * run + rise * rise);
            }
            return length;
        }
    }
}
