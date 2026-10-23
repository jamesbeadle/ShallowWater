using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Ground
{
    public readonly struct Span
    {
        private const double Half = 0.5;

        public Span(double from, double to)
        {
            From = from;
            To = to;
        }

        public static Span Around(double middle, double reach)
        {
            return new Span(middle - reach, middle + reach);
        }

        public double From { get; }
        public double To { get; }
        public double Length => To - From;
        public double Middle => From + Length * Half;

        public bool IsAround(double along)
        {
            return along >= From && along <= To;
        }

        public IEnumerable<Span> Pieces(double longestMetres)
        {
            var count = Math.Max(1, (int)Math.Ceiling(Length / longestMetres));
            var pieceLength = Length / count;
            for (var piece = 0; piece < count; piece++) yield return new Span(From + pieceLength * piece, From + pieceLength * (piece + 1));
        }
    }
}
