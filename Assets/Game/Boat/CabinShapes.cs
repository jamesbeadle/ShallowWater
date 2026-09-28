using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class CabinShapes
    {
        private static readonly double[] Ends = { SparrowForm.CabinBackAlong, SparrowForm.CabinFrontAlong };

        public static void Build(SurfaceShapes surfaces)
        {
            surfaces.Add(Surface.Cabin, Sweep.Lengthways(Ends.Select(PortSide).ToList()));
            surfaces.Add(Surface.Cabin, Sweep.Lengthways(Ends.Select(StarboardSide).ToList()));
            surfaces.Add(Surface.CabinRoof, Sweep.Lengthways(Ends.Select(Roof).ToList()));
            surfaces.Add(Surface.CabinBack, FacingPolygon.Towards(Outline(SparrowForm.CabinBackAlong), BoatSides.Astern));
            surfaces.Add(Surface.Cabin, FacingPolygon.Towards(Outline(SparrowForm.CabinFrontAlong), BoatSides.Ahead));
        }

        private static IReadOnlyList<WorldPoint> PortSide(double along)
        {
            return new[] { Foot(along, BoatSides.Port), Top(along, BoatSides.Port) };
        }

        private static IReadOnlyList<WorldPoint> StarboardSide(double along)
        {
            return new[] { Top(along, BoatSides.Starboard), Foot(along, BoatSides.Starboard) };
        }

        private static IReadOnlyList<WorldPoint> Roof(double along)
        {
            return new[] { Top(along, BoatSides.Port), Crown(along), Top(along, BoatSides.Starboard) };
        }

        private static IReadOnlyList<WorldPoint> Outline(double along)
        {
            var port = BoatSides.Port;
            var starboard = BoatSides.Starboard;
            return new[] { Foot(along, port), Top(along, port), Crown(along), Top(along, starboard), Foot(along, starboard) };
        }

        private static WorldPoint Foot(double along, double side)
        {
            return new WorldPoint(side * SparrowForm.CabinHalfWidthMetres, SparrowForm.CabinFootMetres, along);
        }

        private static WorldPoint Top(double along, double side)
        {
            return new WorldPoint(side * SparrowForm.CabinHalfWidthMetres, SparrowForm.CabinSideTopMetres, along);
        }

        private static WorldPoint Crown(double along)
        {
            return new WorldPoint(BoatSides.Amidships, SparrowForm.CabinCrownMetres, along);
        }
    }
}
