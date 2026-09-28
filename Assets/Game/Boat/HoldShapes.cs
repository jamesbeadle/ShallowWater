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
        private const double ClothStationMetres = 0.15;
        private const double MastAlong = 3.3;
        private const double MastHalfWidthMetres = 0.07;
        private const double MastFootMetres = 1.2;
        private const double MastAboveThePlankMetres = 0.45;
        private const double MastCapMetres = 0.05;

        public static void Build(SurfaceShapes surfaces)
        {
            var stations = Hull.Stations(SparrowForm.CabinFrontAlong, SparrowForm.HoldFrontAlong, ClothStationMetres);
            surfaces.Add(Surface.Cloths, Sweep.Lengthways(stations.Select(Sheeting.Sagging).ToList()));
            surfaces.Add(Surface.Cloths, FacingPolygon.Towards(Sheeting.Sagging(SparrowForm.CabinFrontAlong), BoatSides.Astern));
            surfaces.Add(Surface.Cratch, FacingPolygon.Towards(Sheeting.Sagging(SparrowForm.HoldFrontAlong), BoatSides.Ahead));
            TopPlank(surfaces);
            Mast(surfaces);
            ClothStrings.Build(surfaces);
        }

        private static void TopPlank(SurfaceShapes surfaces)
        {
            var back = SparrowForm.CabinFrontAlong;
            var front = SparrowForm.HoldFrontAlong;
            var middle = new GroundPoint(BoatSides.Amidships, (back + front) * Half);
            var plank = Footprints.Oblong(middle, BoatSides.Ahead, (front - back) * Half, SparrowForm.TopPlankHalfWidthMetres);
            var foot = SparrowForm.TopPlankMetres - PlankSeatMetres;
            surfaces.AddSolid(Surface.Deck, plank, foot, Sheeting.PlankTopMetres);
        }

        private static void Mast(SurfaceShapes surfaces)
        {
            var place = new GroundPoint(BoatSides.Amidships, MastAlong);
            var mast = Footprints.Oblong(place, BoatSides.Ahead, MastHalfWidthMetres, MastHalfWidthMetres);
            var top = Sheeting.PlankTopMetres + MastAboveThePlankMetres;
            surfaces.AddSolid(Surface.Deck, mast, MastFootMetres, top);
            surfaces.AddSolid(Surface.BoatIron, mast, top, top + MastCapMetres);
        }
    }
}
