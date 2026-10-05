using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class CabinBack
    {
        private const double Back = SparrowForm.CabinBackAlong;
        private const double Doorway = SparrowForm.HatchHalfWidthMetres;
        private const double Foot = SparrowForm.CabinFootMetres;
        private const double Sill = SparrowForm.HatchFloorMetres;
        private static readonly double[] Sides = { BoatSides.Port, BoatSides.Starboard };

        public static void Build(SurfaceShapes surfaces)
        {
            foreach (var side in Sides) surfaces.Add(Surface.Cabin, FacingPolygon.Towards(BesideTheDoorway(side), BoatSides.Astern));
            surfaces.Add(Surface.Cabin, FacingPolygon.Towards(UnderTheSill(), BoatSides.Astern));
            BackDoors.Build(surfaces);
        }

        private static IReadOnlyList<WorldPoint> BesideTheDoorway(double side)
        {
            var jambFoot = new WorldPoint(side * Doorway, Foot, Back);
            return new[] { CabinSection.Foot(Back, side), CabinSection.Top(Back, side), Hatchway.EdgeAt(Back, side), jambFoot };
        }

        private static IReadOnlyList<WorldPoint> UnderTheSill()
        {
            var port = BoatSides.Port * Doorway;
            var starboard = BoatSides.Starboard * Doorway;
            return new[]
            {
                new WorldPoint(port, Foot, Back), new WorldPoint(port, Sill, Back),
                new WorldPoint(starboard, Sill, Back), new WorldPoint(starboard, Foot, Back)
            };
        }
    }
}
