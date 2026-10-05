using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class Hatchway
    {
        private const double Half = 0.5;
        private static readonly GroundPoint TowardsStarboard = new GroundPoint(BoatSides.Starboard, BoatSides.Amidships);
        private static readonly GroundPoint TowardsPort = new GroundPoint(BoatSides.Port, BoatSides.Amidships);
        private const double Back = SparrowForm.CabinBackAlong;
        private const double Front = SparrowForm.HatchFrontAlong;
        private const double Floor = SparrowForm.HatchFloorMetres;
        private const double HalfWidth = SparrowForm.HatchHalfWidthMetres;

        public static WorldPoint EdgeAt(double along, double side)
        {
            var across = side * HalfWidth;
            return new WorldPoint(across, SparrowForm.RoofHeightAt(across), along);
        }

        public static void Build(SurfaceShapes surfaces)
        {
            surfaces.Add(Surface.Deck, FacingPolygon.Towards(Side(BoatSides.Port), TowardsStarboard));
            surfaces.Add(Surface.Deck, FacingPolygon.Towards(Side(BoatSides.Starboard), TowardsPort));
            surfaces.Add(Surface.Deck, FacingPolygon.Towards(End(Front), BoatSides.Astern));
            surfaces.Add(Surface.Deck, FacingPolygon.Towards(End(Back), BoatSides.Ahead));
            var middle = new GroundPoint(BoatSides.Amidships, (Back + Front) * Half);
            var floor = Footprints.Oblong(middle, BoatSides.Ahead, (Front - Back) * Half, HalfWidth);
            surfaces.Add(Surface.Deck, Extrusion.Roof(floor, Floor));
        }

        private static IReadOnlyList<WorldPoint> Side(double side)
        {
            var across = side * HalfWidth;
            return new[] { new WorldPoint(across, Floor, Back), new WorldPoint(across, Floor, Front), EdgeAt(Front, side), EdgeAt(Back, side) };
        }

        private static IReadOnlyList<WorldPoint> End(double along)
        {
            var port = BoatSides.Port * HalfWidth;
            var starboard = BoatSides.Starboard * HalfWidth;
            var crown = new WorldPoint(BoatSides.Amidships, SparrowForm.CabinCrownMetres, along);
            return new[] { new WorldPoint(port, Floor, along), EdgeAt(along, BoatSides.Port), crown, EdgeAt(along, BoatSides.Starboard), new WorldPoint(starboard, Floor, along) };
        }
    }
}
