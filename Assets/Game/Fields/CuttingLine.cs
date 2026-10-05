using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public readonly struct CuttingLine
    {
        private const double Halfway = 0.5;

        public CuttingLine(GroundPoint through, GroundPoint direction)
        {
            Through = through;
            Direction = direction.Normalised;
        }

        public GroundPoint Through { get; }
        public GroundPoint Direction { get; }
        public GroundPoint Across => Direction.RightAngleClockwise;
        public CuttingLine Reversed => new CuttingLine(Through, Direction * -1);

        public double LeftOf(GroundPoint place)
        {
            return Direction.Cross(place - Through);
        }

        public GroundPoint CrossingOf(GroundPoint from, GroundPoint to)
        {
            var fromSide = LeftOf(from);
            var share = fromSide / (fromSide - LeftOf(to));
            return from + (to - from) * share;
        }

        public static CuttingLine Between(GroundPoint keep, GroundPoint other)
        {
            var middle = (keep + other) * Halfway;
            var towardsTheOther = other - keep;
            return new CuttingLine(middle, towardsTheOther.RightAngleClockwise);
        }
    }
}
