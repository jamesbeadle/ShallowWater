using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public readonly struct Tree
    {
        public GroundPoint Position { get; }
        public double Scale { get; }
        public double East => Position.East;
        public double North => Position.North;

        public Tree(GroundPoint position, double scale)
        {
            Position = position;
            Scale = scale;
        }
    }
}
