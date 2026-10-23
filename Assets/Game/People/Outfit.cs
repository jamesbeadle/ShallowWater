using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public readonly struct Outfit
    {
        private const double GreatcoatSkirtMetres = 0.56;
        private const double JacketSkirtMetres = 0.24;
        private const double DoubleBreastedOverlapRadians = 0.5;
        private const double SingleBreastedOverlapRadians = 0.06;

        public static readonly Outfit Askew = new Outfit(Surface.Coat, Surface.Trousers, true, GreatcoatSkirtMetres, DoubleBreastedOverlapRadians);
        public static readonly Outfit Farmer = new Outfit(Surface.Tweed, Surface.Corduroy, false, JacketSkirtMetres, SingleBreastedOverlapRadians);

        private Outfit(Surface coat, Surface trousers, bool hasAScarf, double skirtMetres, double frontOverlapRadians)
        {
            Coat = coat;
            Trousers = trousers;
            HasAScarf = hasAScarf;
            SkirtMetres = skirtMetres;
            FrontOverlapRadians = frontOverlapRadians;
        }

        public Surface Coat { get; }
        public Surface Trousers { get; }
        public bool HasAScarf { get; }
        public double SkirtMetres { get; }
        public double FrontOverlapRadians { get; }
        public bool IsDoubleBreasted => FrontOverlapRadians > SingleBreastedOverlapRadians;
    }
}
