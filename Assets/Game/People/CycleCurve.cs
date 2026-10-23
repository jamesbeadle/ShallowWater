using System;
using System.Linq;

namespace ShallowWater.Game.People
{
    public sealed class CycleCurve
    {
        private const double Whole = 1;
        private readonly double[] shares;
        private readonly double[] values;

        public CycleCurve(params (double share, double value)[] keys)
        {
            var ordered = keys.OrderBy(key => key.share).ToArray();
            shares = ordered.Select(key => key.share).ToArray();
            values = ordered.Select(key => key.value).ToArray();
        }

        public double At(double share)
        {
            var cycle = IntoTheKeyedTurn(share - Math.Floor(share));
            var key = LastKeyBefore(cycle);
            var next = (key + 1) % shares.Length;
            var isWrapping = next == 0;
            var span = isWrapping ? shares[next] + Whole - shares[key] : shares[next] - shares[key];
            var along = (cycle - shares[key]) / span;
            return values[key] + (values[next] - values[key]) * Smoothing.Step(along);
        }

        private double IntoTheKeyedTurn(double cycle)
        {
            var isBeforeTheFirstKey = cycle < shares[0];
            return isBeforeTheFirstKey ? cycle + Whole : cycle;
        }

        private int LastKeyBefore(double cycle)
        {
            var key = 0;
            while (key + 1 < shares.Length && shares[key + 1] <= cycle) key++;
            return key;
        }
    }
}
