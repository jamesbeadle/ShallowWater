using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class Fittings
    {
        private const int RoundSides = 10;
        private const double IntoTheRoofMetres = 1.72;
        private static readonly GroundPoint ChimneyPlace = new GroundPoint(-0.45, -7.2);
        private const double ChimneyRadiusMetres = 0.075;
        private const double ChimneyTopMetres = 2.45;
        private const double BandProudMetres = 0.012;
        private static readonly Rise LowerBand = new Rise(2.02, 2.08);
        private static readonly Rise TopBand = new Rise(2.38, 2.46);
        private static readonly GroundPoint ExhaustPlace = new GroundPoint(0.45, -5.2);
        private const double ExhaustRadiusMetres = 0.045;
        private const double ExhaustTopMetres = 2.05;
        private static readonly GroundPoint WaterCanPlace = new GroundPoint(0.35, -8.4);
        private const double WaterCanRadiusMetres = 0.14;
        private const double WaterCanTopMetres = 2.12;

        public static void Build(SurfaceShapes surfaces)
        {
            Round(surfaces, Surface.Hull, ChimneyPlace, ChimneyRadiusMetres, new Rise(IntoTheRoofMetres, ChimneyTopMetres));
            Round(surfaces, Surface.Brass, ChimneyPlace, ChimneyRadiusMetres + BandProudMetres, LowerBand);
            Round(surfaces, Surface.Brass, ChimneyPlace, ChimneyRadiusMetres + BandProudMetres, TopBand);
            Round(surfaces, Surface.Hull, ExhaustPlace, ExhaustRadiusMetres, new Rise(IntoTheRoofMetres, ExhaustTopMetres));
            Round(surfaces, Surface.Cratch, WaterCanPlace, WaterCanRadiusMetres, new Rise(IntoTheRoofMetres, WaterCanTopMetres));
            SternGear.Build(surfaces);
        }

        private static void Round(SurfaceShapes surfaces, Surface surface, GroundPoint centre, double radiusMetres, Rise rise)
        {
            var footprint = Footprints.Round(centre, radiusMetres, RoundSides);
            surfaces.AddSolid(surface, footprint, rise.FootMetres, rise.TopMetres);
        }
    }
}
