using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Roads
{
    public readonly struct RoadsidePiece
    {
        public RoadsidePiece(Span span, Roadside roadside)
        {
            Span = span;
            Roadside = roadside;
        }

        public Span Span { get; }
        public Roadside Roadside { get; }
    }
}
