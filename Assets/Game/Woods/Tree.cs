using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public readonly struct Tree
    {
        public GroundPoint Position { get; }
        public double Scale { get; }
        public double TurnRadians { get; }
        public double Slenderness { get; }
        public double East => Position.East;
        public double North => Position.North;

        public Tree(GroundPoint position, double scale, double turnRadians, double slenderness)
        {
            Position = position;
            Scale = scale;
            TurnRadians = turnRadians;
            Slenderness = slenderness;
        }
    }
}
