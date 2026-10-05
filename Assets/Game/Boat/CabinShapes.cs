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
            CabinBack.Build(surfaces);
            surfaces.Add(Surface.Cabin, FacingPolygon.Towards(Front(), BoatSides.Ahead));
            RoofFittings.Build(surfaces);
        }

        private static IReadOnlyList<WorldPoint> PortSide(double along)
        {
            return new[] { CabinSection.Foot(along, BoatSides.Port), CabinSection.Top(along, BoatSides.Port) };
        }

        private static IReadOnlyList<WorldPoint> StarboardSide(double along)
        {
            return new[] { CabinSection.Top(along, BoatSides.Starboard), CabinSection.Foot(along, BoatSides.Starboard) };
        }

        private static IReadOnlyList<WorldPoint> Lip(double along, double side)
        {
            var top = SparrowForm.CabinSideTopMetres + SparrowForm.RoofLipProudMetres;
            var lip = new Rise(SparrowForm.CabinSideTopMetres - SparrowForm.RoofLipHeightMetres, top);
            return Beading.Across(along, side, SparrowForm.CabinTopHalfWidthMetres, lip, SparrowForm.RoofLipProudMetres);
        }

        private static IReadOnlyList<WorldPoint> Roof(double along)
        {
            return new[] { CabinSection.Top(along, BoatSides.Port), CabinSection.Crown(along), CabinSection.Top(along, BoatSides.Starboard) };
        }

        private static IReadOnlyList<WorldPoint> RoofBesideTheHatch(double along, double side)
        {
            var edge = Hatchway.EdgeAt(along, side);
            var top = CabinSection.Top(along, side);
            var isPort = side == BoatSides.Port;
            return isPort ? new[] { top, edge } : new[] { edge, top };
        }

        private static IReadOnlyList<WorldPoint> Front()
        {
            var along = SparrowForm.CabinFrontAlong;
            var port = BoatSides.Port;
            var starboard = BoatSides.Starboard;
            return new[]
            {
                CabinSection.Foot(along, port), CabinSection.Top(along, port), CabinSection.Crown(along),
                CabinSection.Top(along, starboard), CabinSection.Foot(along, starboard)
            };
        }
    }
}
