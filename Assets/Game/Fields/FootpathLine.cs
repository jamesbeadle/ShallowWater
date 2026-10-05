using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public readonly struct FootpathLine
    {
        private const double TooParallel = 1e-6;
        private const double Start = 0;
        private const double End = 1;

        private readonly GroundPoint from;
        private readonly GroundPoint to;

        public FootpathLine(GroundPoint from, GroundPoint to)
        {
            this.from = from;
            this.to = to;
        }

        public bool IsCrossing(Hedgerow hedgerow, out double alongMetres, out double sine)
        {
            var hedgeRun = hedgerow.To - hedgerow.From;
            var pathRun = to - from;
            var turn = hedgeRun.Cross(pathRun);
            alongMetres = Start;
            sine = Math.Abs(turn) / (hedgeRun.Length * pathRun.Length);
            var isParallel = Math.Abs(turn) < TooParallel;
            if (isParallel) return false;
            var toThePath = from - hedgerow.From;
            var hedgeShare = toThePath.Cross(pathRun) / turn;
            var pathShare = toThePath.Cross(hedgeRun) / turn;
            alongMetres = hedgeShare * hedgeRun.Length;
            return IsWithin(hedgeShare) && IsWithin(pathShare);
        }

        private static bool IsWithin(double share)
        {
            return share >= Start && share <= End;
        }
    }
}
