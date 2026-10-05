using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public readonly struct Chord
    {
        public Chord(GroundPoint from, GroundPoint to)
        {
            From = from;
            To = to;
        }

        public GroundPoint From { get; }
        public GroundPoint To { get; }
    }
}
