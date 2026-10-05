using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class BackDoors
    {
        private const double ThicknessMetres = 0.035;
        private const double Back = SparrowForm.CabinBackAlong;
        private const double Face = Back - ThicknessMetres;
        private const double Foot = SparrowForm.DoorsFootMetres;
        private const double Top = SparrowForm.DoorsTopMetres;
        private const double Half = 0.5;
        private static readonly double[] HingesAboveTheFootMetres = { 0.15, 0.68 };
        private const double HingeLengthMetres = 0.08;
        private const double HingeRadiusMetres = 0.012;
        private const double HookAboveTheFootMetres = 0.5;
        private const double HookRadiusMetres = 0.005;
        private static readonly Offset[] HookFromTheFreeEdge =
        {
            new Offset(0.045, 0, 0), new Offset(0.045, 0, -0.045), new Offset(0.02, 0, -0.05),
            new Offset(-0.022, 0, -0.046), new Offset(-0.03, 0, -0.038)
        };
        private const int RoundSides = 6;
        private static readonly double[] Sides = { BoatSides.Port, BoatSides.Starboard };

        public static void Build(SurfaceShapes surfaces)
        {
            foreach (var side in Sides) PinnedBack(surfaces, side);
        }

        private static void PinnedBack(SurfaceShapes surfaces, double side)
        {
            var hinge = side * SparrowForm.HatchHalfWidthMetres;
            var free = hinge + side * SparrowForm.DoorWidthMetres;
            surfaces.Add(Surface.BackDoors, FacingPolygon.Towards(Upright(hinge, Face, free, Face), BoatSides.Astern));
            surfaces.Add(Surface.Cabin, FacingPolygon.Towards(Upright(hinge, Back, hinge, Face), new GroundPoint(-side, BoatSides.Amidships)));
            surfaces.Add(Surface.Cabin, FacingPolygon.Towards(Upright(free, Back, free, Face), new GroundPoint(side, BoatSides.Amidships)));
            var middle = new GroundPoint((hinge + free) * Half, (Back + Face) * Half);
            var footprint = Footprints.Oblong(middle, BoatSides.Ahead, ThicknessMetres * Half, SparrowForm.DoorWidthMetres * Half);
            surfaces.Add(Surface.Cabin, Extrusion.Roof(footprint, Top));
            foreach (var height in HingesAboveTheFootMetres) Hinge(surfaces, hinge, Foot + height);
            Hook(surfaces, free, side);
        }

        private static IReadOnlyList<WorldPoint> Upright(double fromAcross, double fromAlong, double toAcross, double toAlong)
        {
            return new[]
            {
                new WorldPoint(fromAcross, Foot, fromAlong), new WorldPoint(fromAcross, Top, fromAlong),
                new WorldPoint(toAcross, Top, toAlong), new WorldPoint(toAcross, Foot, toAlong)
            };
        }

        private static void Hinge(SurfaceShapes surfaces, double across, double foot)
        {
            var knuckle = new[] { new WorldPoint(across, foot, Face), new WorldPoint(across, foot + HingeLengthMetres, Face) };
            surfaces.Add(Surface.BoatIron, Tube.Along(knuckle, HingeRadiusMetres, RoundSides));
        }

        private static void Hook(SurfaceShapes surfaces, double freeEdge, double side)
        {
            var eye = new WorldPoint(freeEdge, Foot + HookAboveTheFootMetres, Back);
            var hook = HookFromTheFreeEdge.Select(bend => new Offset(side * bend.Eastward, bend.Upward, bend.Northward).From(eye)).ToList();
            surfaces.Add(Surface.BoatIron, Tube.Along(hook, HookRadiusMetres, RoundSides));
        }
    }
}
