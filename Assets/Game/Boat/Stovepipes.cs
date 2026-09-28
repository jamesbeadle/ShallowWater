using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class Stovepipes
    {
        public static readonly WorldPoint ChimneyFoot = new WorldPoint(-0.45, 1.72, -7.3);
        public static readonly WorldPoint ExhaustFoot = new WorldPoint(0.45, 1.72, -5.0);
        public const double ChimneyTopMetres = 0.705;
        public const double ExhaustTopMetres = 0.47;
        private const int RoundSides = 12;
        private const double BandRadiusMetres = 0.083;
        private const double BandHeightMetres = 0.04;
        private static readonly double[] ChimneyBandsMetres = { 0.2, 0.45 };
        private const double ExhaustBandMetres = 0.33;
        private static readonly WorldPoint ChainHook = new WorldPoint(-0.2, 1.77, -6.95);
        private static readonly Offset ChainFromTheFoot = new Offset(0.08, ChimneyTopMetres - 0.15, 0);

        private static readonly ProfilePoint[] Chimney =
        {
            new ProfilePoint(0.075, 0), new ProfilePoint(0.075, 0.6), new ProfilePoint(0.092, 0.625),
            new ProfilePoint(0.092, 0.68), new ProfilePoint(0.06, 0.7), new ProfilePoint(0, ChimneyTopMetres)
        };

        private static readonly ProfilePoint[] Exhaust =
        {
            new ProfilePoint(0.05, 0), new ProfilePoint(0.05, 0.3), new ProfilePoint(0.065, 0.33), new ProfilePoint(0.065, 0.4),
            new ProfilePoint(0.05, 0.42), new ProfilePoint(0.05, ExhaustTopMetres), new ProfilePoint(0, ExhaustTopMetres)
        };

        public static void Build(SurfaceShapes surfaces)
        {
            surfaces.Add(Surface.BoatIron, Lathe.Turned(ChimneyFoot, Chimney, RoundSides));
            foreach (var band in ChimneyBandsMetres) Band(surfaces, ChimneyFoot, band);
            surfaces.Add(Surface.BoatIron, Lathe.Turned(ExhaustFoot, Exhaust, RoundSides));
            Band(surfaces, ExhaustFoot, ExhaustBandMetres);
            Chain.Hang(surfaces, ChainFromTheFoot.From(ChimneyFoot), ChainHook);
        }

        private static void Band(SurfaceShapes surfaces, WorldPoint foot, double heightMetres)
        {
            var band = new[] { new ProfilePoint(BandRadiusMetres, heightMetres), new ProfilePoint(BandRadiusMetres, heightMetres + BandHeightMetres) };
            surfaces.Add(Surface.Brass, Lathe.Turned(foot, band, RoundSides));
        }
    }
}
