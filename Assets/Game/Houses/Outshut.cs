using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public sealed class Outshut
    {
        public static readonly Outshut None = new Outshut(new Opening(0, 0, 0, 0), false, new List<Joinery>(), new List<Joinery>());

        public Outshut(Opening span, bool isOnTheRight, IReadOnlyList<Joinery> back, IReadOnlyList<Joinery> side)
        {
            Span = span;
            IsOnTheRight = isOnTheRight;
            Back = back;
            Side = side;
        }

        public Opening Span { get; }
        public bool IsOnTheRight { get; }
        public double AlongMetres => Span.Left;
        public double WidthMetres => Span.WidthMetres;
        public double LowEavesMetres => Span.Bottom;
        public double HighEavesMetres => Span.Top;
        public IReadOnlyList<Joinery> Back { get; }
        public IReadOnlyList<Joinery> Side { get; }
        public bool IsBuilt => WidthMetres > 0;
    }
}
