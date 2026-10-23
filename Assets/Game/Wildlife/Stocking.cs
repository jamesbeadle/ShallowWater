using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public readonly struct Stocking
    {
        public Stocking(QuarryKind kind, int count, IReadOnlyList<WorldPoint> stations)
        {
            Kind = kind;
            Count = count;
            Stations = stations;
        }

        public QuarryKind Kind { get; }
        public int Count { get; }
        public IReadOnlyList<WorldPoint> Stations { get; }
    }
}
