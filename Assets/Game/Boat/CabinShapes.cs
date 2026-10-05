using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class CabinShapes
    {
        private static readonly double[] Ends = { SparrowForm.CabinBackAlong, SparrowForm.CabinFrontAlong };
        private static readonly double[] AheadOfTheHatch = { SparrowForm.HatchFrontAlong, SparrowForm.CabinFrontAlong };
        private static readonly double[] BesideTheHatch = { SparrowForm.CabinBackAlong, SparrowForm.HatchFrontAlong };

        public static void Build(SurfaceShapes surfaces)
        {
            surfaces.Add(Surface.Cabin, Sweep.Lengthways(Ends.Select(PortSide).ToList()));
            surfaces.Add(Surface.Cabin, Sweep.Lengthways(Ends.Select(StarboardSide).ToList()));
            surfaces.Add(Surface.Cabin, Sweep.Lengthways(Ends.Select(along => Lip(along, BoatSides.Port)).ToList()));
            surfaces.Add(Surface.Cabin, Sweep.Lengthways(Ends.Select(along => Lip(along, BoatSides.Starboard)).ToList()));
            surfaces.Add(Surface.CabinRoof, Sweep.Lengthways(AheadOfTheHatch.Select(Roof).ToList()));
            surfaces.Add(Surface.CabinRoof, Sweep.Lengthways(BesideTheHatch.Select(along => RoofBesideTheHatch(along, BoatSides.Port)).ToList()));
            surfaces.Add(Surface.CabinRoof, Sweep.Lengthways(BesideTheHatch.Select(along => RoofBesideTheHatch(along, BoatSides.Starboard)).ToList()));
            Hatchway.Build(surfaces);
            surfaces.Add(Surface.CabinBack, FacingPolygon.Towards(Outline(SparrowForm.CabinBackAlong), BoatSides.Astern));
            surfaces.Add(Surface.Cabin, FacingPolygon.Towards(Outline(SparrowForm.CabinFrontAlong), BoatSides.Ahead));
            RoofFittings.Build(surfaces);
        }

        private static IReadOnlyList<WorldPoint> PortSide(double along)
        {
            return new[] { Foot(along, BoatSides.Port), Top(along, BoatSides.Port) };
        }

        private static IReadOnlyList<WorldPoint> StarboardSide(double along)
        {
            return new[] { Top(along, BoatSides.Starboard), Foot(along, BoatSides.Starboard) };
        }

        private static IReadOnlyList<WorldPoint> Lip(double along, double side)
        {
            var top = SparrowForm.CabinSideTopMetres + SparrowForm.RoofLipProudMetres;
            var lip = new Rise(SparrowForm.CabinSideTopMetres - SparrowForm.RoofLipHeightMetres, top);
            return Beading.Across(along, side, SparrowForm.CabinTopHalfWidthMetres, lip, SparrowForm.RoofLipProudMetres);
        }

        private static IReadOnlyList<WorldPoint> Roof(double along)
        {
            return new[] { Top(along, BoatSides.Port), Crown(along), Top(along, BoatSides.Starboard) };
        }

        private static IReadOnlyList<WorldPoint> RoofBesideTheHatch(double along, double side)
        {
            var edge = Hatchway.EdgeAt(along, side);
            var top = Top(along, side);
            var isPort = side == BoatSides.Port;
            return isPort ? new[] { top, edge } : new[] { edge, top };
        }

        private static IReadOnlyList<WorldPoint> Outline(double along)
        {
            var port = BoatSides.Port;
            var starboard = BoatSides.Starboard;
            return new[] { Foot(along, port), Top(along, port), Crown(along), Top(along, starboard), Foot(along, starboard) };
        }

        private static WorldPoint Foot(double along, double side)
        {
            return new WorldPoint(side * SparrowForm.CabinFootHalfWidthMetres, SparrowForm.CabinFootMetres, along);
        }

        private static WorldPoint Top(double along, double side)
        {
            return new WorldPoint(side * SparrowForm.CabinTopHalfWidthMetres, SparrowForm.CabinSideTopMetres, along);
        }

        private static WorldPoint Crown(double along)
        {
            return new WorldPoint(BoatSides.Amidships, SparrowForm.CabinCrownMetres, along);
        }
    }
}
