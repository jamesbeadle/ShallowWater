using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public readonly struct Outfit
    {
        public static readonly Outfit Askew = new Outfit(Surface.Coat, Surface.Trousers, true);
        public static readonly Outfit Farmer = new Outfit(Surface.Tweed, Surface.Corduroy, false);

        private Outfit(Surface coat, Surface trousers, bool hasAScarf)
        {
            Coat = coat;
            Trousers = trousers;
            HasAScarf = hasAScarf;
        }

        public Surface Coat { get; }
        public Surface Trousers { get; }
        public bool HasAScarf { get; }
    }
}
