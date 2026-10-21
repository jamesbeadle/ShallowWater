using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Mooring
{
    public sealed class HandOnTheLine
    {
        private const double NoTime = 0;
        private static readonly GroundPoint Still = new GroundPoint(0, 0);

        public GroundPoint Place { get; private set; }
        public GroundPoint Velocity { get; private set; } = Still;

        public void Rest(GroundPoint place)
        {
            Place = place;
            Velocity = Still;
        }

        public void MoveTo(GroundPoint place, double seconds)
        {
            var isAnInstant = seconds <= NoTime;
            Velocity = isAnInstant ? Velocity : (place - Place) * (1 / seconds);
            Place = place;
        }
    }
}
