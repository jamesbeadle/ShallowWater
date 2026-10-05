using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class RidgeTiles
    {
        private const double HalfWidthMetres = 0.13;
        private const double CrownAboveTheRidgeMetres = 0.07;
        private const double WingsAboveTheRidgeMetres = -0.02;
        private const double ViewingHeightMetres = 3;
        private const double Half = 0.5;
        private static readonly double[] Wings = { -1, 1 };

        public static void Lay(SurfaceShapes surfaces, WorldPoint from, WorldPoint to, GroundPoint across)
        {
            var tiles = new Shape();
            var fromCrown = Raised(from, CrownAboveTheRidgeMetres);
            var toCrown = Raised(to, CrownAboveTheRidgeMetres);
            foreach (var wing in Wings)
            {
                var sideways = across * (wing * HalfWidthMetres);
                var fromWing = Moved(from, sideways, WingsAboveTheRidgeMetres);
                var toWing = Moved(to, sideways, WingsAboveTheRidgeMetres);
                var viewer = Moved(Middle(from, to), sideways, ViewingHeightMetres);
                Facet.Add(tiles, new[] { fromWing, toWing, toCrown, fromCrown }, viewer);
            }
            surfaces.Add(Surface.RidgeTiles, tiles);
        }

        private static WorldPoint Raised(WorldPoint point, double metres)
        {
            return new WorldPoint(point.East, point.Height + metres, point.North);
        }

        private static WorldPoint Moved(WorldPoint point, GroundPoint sideways, double upMetres)
        {
            return new WorldPoint(point.East + sideways.East, point.Height + upMetres, point.North + sideways.North);
        }

        private static WorldPoint Middle(WorldPoint from, WorldPoint to)
        {
            return new WorldPoint((from.East + to.East) * Half, (from.Height + to.Height) * Half, (from.North + to.North) * Half);
        }
    }
}
