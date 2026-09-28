using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class ClothStrings
    {
        private const double ClearOfTheClothMetres = 0.012;
        private const double RadiusMetres = 0.009;
        private const int Sides = 5;

        public static void Build(SurfaceShapes surfaces)
        {
            for (var span = 1; span < SparrowForm.ClothSpans; span++)
            {
                var along = SparrowForm.CabinFrontAlong + span * Sheeting.SpanMetres;
                surfaces.Add(Surface.Rope, Tube.Along(OverTheCloths(along), RadiusMetres, Sides));
            }
        }

        private static List<WorldPoint> OverTheCloths(double along)
        {
            var gunwale = Hull.GunwaleAt(along);
            var halfBeam = Hull.HalfBeamAt(along);
            var sideTop = gunwale + SparrowForm.SideClothMetres;
            var plankEdge = SparrowForm.TopPlankHalfWidthMetres;
            var path = new[]
            {
                new WorldPoint(-halfBeam, gunwale, along), new WorldPoint(-halfBeam, sideTop, along),
                new WorldPoint(-plankEdge, Sheeting.PlankTopMetres, along), new WorldPoint(plankEdge, Sheeting.PlankTopMetres, along),
                new WorldPoint(halfBeam, sideTop, along), new WorldPoint(halfBeam, gunwale, along)
            };
            return path.Select(Clear).ToList();
        }

        private static WorldPoint Clear(WorldPoint point)
        {
            var outward = new Offset(point.East, point.Height, 0).Normalised * ClearOfTheClothMetres;
            return outward.From(point);
        }
    }
}
