using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class RoofFittings
    {
        private const double IntoTheRoofMetres = 1.74;
        private static readonly GroundPoint SlidePushedOpen = new GroundPoint(BoatSides.Amidships, -8.45);
        private const double SlideHalfLengthMetres = 0.4;
        private const double SlideHalfWidthMetres = 0.32;
        private const double SlideTopMetres = 1.9;
        private static readonly GroundPoint PigeonBoxPlace = new GroundPoint(BoatSides.Amidships, -5.45);
        private const double PigeonBoxHalfLengthMetres = 0.28;
        private const double PigeonBoxHalfWidthMetres = 0.22;
        private const double PigeonBoxTopMetres = 1.98;
        private const double LidOverhangMetres = 0.03;
        private const double LidThicknessMetres = 0.025;
        private const double RailAcrossMetres = 0.78;
        private const double RailAboveTheRoofMetres = 0.07;
        private const double RailRadiusMetres = 0.016;
        private const int RailSides = 6;
        private const double RailFromAlong = -8.6;
        private const double RailToAlong = -4.5;
        private const int RailPosts = 5;
        private static readonly double[] Sides = { BoatSides.Port, BoatSides.Starboard };

        public static void Build(SurfaceShapes surfaces)
        {
            Box(surfaces, SlidePushedOpen, SlideHalfLengthMetres, SlideHalfWidthMetres, SlideTopMetres);
            Box(surfaces, PigeonBoxPlace, PigeonBoxHalfLengthMetres, PigeonBoxHalfWidthMetres, PigeonBoxTopMetres);
            var lidHalfLength = PigeonBoxHalfLengthMetres + LidOverhangMetres;
            var lid = Footprints.Oblong(PigeonBoxPlace, BoatSides.Ahead, lidHalfLength, PigeonBoxHalfWidthMetres + LidOverhangMetres);
            surfaces.AddSolid(Surface.CabinRoof, lid, PigeonBoxTopMetres, PigeonBoxTopMetres + LidThicknessMetres);
            foreach (var side in Sides) Handrail(surfaces, side * RailAcrossMetres);
        }

        private static void Box(SurfaceShapes surfaces, GroundPoint centre, double halfLength, double halfWidth, double top)
        {
            var footprint = Footprints.Oblong(centre, BoatSides.Ahead, halfLength, halfWidth);
            surfaces.AddSolid(Surface.CabinRoof, footprint, IntoTheRoofMetres, top);
        }

        private static void Handrail(SurfaceShapes surfaces, double across)
        {
            var height = SparrowForm.RoofHeightAt(across) + RailAboveTheRoofMetres;
            var rail = new[] { new WorldPoint(across, height, RailFromAlong), new WorldPoint(across, height, RailToAlong) };
            surfaces.Add(Surface.BoatIron, Tube.Along(rail, RailRadiusMetres, RailSides));
            var posts = Hull.Stations(RailFromAlong, RailToAlong, (RailToAlong - RailFromAlong) / (RailPosts - 1));
            foreach (var along in posts.Take(RailPosts)) Post(surfaces, across, along, height);
        }

        private static void Post(SurfaceShapes surfaces, double across, double along, double height)
        {
            var foot = new WorldPoint(across, IntoTheRoofMetres, along);
            var post = new[] { foot, new WorldPoint(across, height, along) };
            surfaces.Add(Surface.BoatIron, Tube.Along(post, RailRadiusMetres, RailSides));
        }
    }
}
