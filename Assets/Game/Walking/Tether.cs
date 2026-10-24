using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public readonly struct Tether
    {
        public static readonly Tether None = new Tether(new GroundPoint(0, 0), double.PositiveInfinity);

        public Tether(GroundPoint anchor, double reachMetres)
        {
            Anchor = anchor;
            ReachMetres = reachMetres;
        }

        public GroundPoint Anchor { get; }
        public double ReachMetres { get; }

        public GroundPoint HeldBack(GroundPoint feet)
        {
            var away = feet - Anchor;
            var isWithinReach = away.Length <= ReachMetres;
            if (isWithinReach) return feet;
            return Anchor + away.Normalised * ReachMetres;
        }
    }
}
