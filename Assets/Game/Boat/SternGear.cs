using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class SternGear
    {
        private const double BladeBehindTheSternMetres = 0.47;
        private const double BladeHalfLengthMetres = 0.42;
        private const double BladeHalfThicknessMetres = 0.03;
        private static readonly Rise Blade = new Rise(-0.55, 0.35);
        private const double PostBehindTheSternMetres = 0.05;
        private const double PostHalfWidthMetres = 0.07;
        private static readonly Rise Post = new Rise(0.3, 1.15);
        private const double TillerFootMetres = 1.1;
        private const double TillerTopMetres = 1.32;
        private const double TillerReachMetres = 0.8;
        private const double TillerHalfWidthMetres = 0.025;

        public static void Build(SurfaceShapes surfaces)
        {
            var blade = Footprints.Oblong(Behind(BladeBehindTheSternMetres), BoatSides.Ahead, BladeHalfLengthMetres, BladeHalfThicknessMetres);
            surfaces.AddSolid(Surface.Hull, blade, Blade.FootMetres, Blade.TopMetres);
            var post = Footprints.Oblong(Behind(PostBehindTheSternMetres), BoatSides.Ahead, PostHalfWidthMetres, PostHalfWidthMetres);
            surfaces.AddSolid(Surface.Hull, post, Post.FootMetres, Post.TopMetres);
            surfaces.Add(Surface.Brass, Tiller());
        }

        private static GroundPoint Behind(double metres)
        {
            return new GroundPoint(BoatSides.Amidships, Hull.SternAlong - metres);
        }

        private static Shape Tiller()
        {
            var root = new WorldPoint(BoatSides.Amidships, TillerFootMetres, Hull.SternAlong - PostBehindTheSternMetres);
            var handle = new WorldPoint(BoatSides.Amidships, TillerTopMetres, root.North + TillerReachMetres);
            var sections = new[] { root, handle }.Select(Square).ToList();
            return Sweep.Lengthways(sections);
        }

        private static IReadOnlyList<WorldPoint> Square(WorldPoint centre)
        {
            var reach = TillerHalfWidthMetres;
            var low = centre.Height - reach;
            var high = centre.Height + reach;
            var along = centre.North;
            return new[]
            {
                new WorldPoint(-reach, low, along), new WorldPoint(-reach, high, along), new WorldPoint(reach, high, along),
                new WorldPoint(reach, low, along), new WorldPoint(-reach, low, along)
            };
        }
    }
}
