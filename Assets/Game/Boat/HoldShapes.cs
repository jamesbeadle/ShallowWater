using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class HoldShapes
    {
        private const double Half = 0.5;
        private const double PlankSeatMetres = 0.01;

        public static void Build(SurfaceShapes surfaces)
        {
            var stations = Hull.Stations(SparrowForm.CabinFrontAlong, SparrowForm.HoldFrontAlong);
            surfaces.Add(Surface.Cloths, Sweep.Lengthways(stations.Select(Sheeted).ToList()));
            surfaces.Add(Surface.Cloths, FacingPolygon.Towards(Sheeted(SparrowForm.CabinFrontAlong), BoatSides.Astern));
            surfaces.Add(Surface.Cratch, FacingPolygon.Towards(Sheeted(SparrowForm.HoldFrontAlong), BoatSides.Ahead));
            TopPlank(surfaces);
        }

        private static IReadOnlyList<WorldPoint> Sheeted(double along)
        {
            var gunwale = Hull.GunwaleAt(along);
            var halfBeam = Hull.HalfBeamAt(along);
            var sideTop = gunwale + SparrowForm.SideClothMetres;
            var ridge = new WorldPoint(BoatSides.Amidships, SparrowForm.TopPlankMetres, along);
            return new[]
            {
                new WorldPoint(-halfBeam, gunwale, along), new WorldPoint(-halfBeam, sideTop, along), ridge,
                new WorldPoint(halfBeam, sideTop, along), new WorldPoint(halfBeam, gunwale, along)
            };
        }

        private static void TopPlank(SurfaceShapes surfaces)
        {
            var back = SparrowForm.CabinFrontAlong;
            var front = SparrowForm.HoldFrontAlong;
            var middle = new GroundPoint(BoatSides.Amidships, (back + front) * Half);
            var plank = Footprints.Oblong(middle, BoatSides.Ahead, (front - back) * Half, SparrowForm.TopPlankHalfWidthMetres);
            var foot = SparrowForm.TopPlankMetres - PlankSeatMetres;
            surfaces.AddSolid(Surface.Deck, plank, foot, SparrowForm.TopPlankMetres + SparrowForm.TopPlankThicknessMetres);
        }
    }
}
