using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class HullShapes
    {
        public static void Build(SurfaceShapes surfaces)
        {
            var stations = Hull.Stations(Hull.SternAlong, Hull.StemAlong);
            surfaces.Add(Surface.Hull, Sweep.Lengthways(stations.Select(PortSide).ToList()));
            surfaces.Add(Surface.Hull, Sweep.Lengthways(stations.Select(StarboardSide).ToList()));
            surfaces.Add(Surface.Deck, Sweep.Lengthways(stations.Select(DeckAcross).ToList()));
            surfaces.Add(Surface.Hull, Transom());
        }

        private static IReadOnlyList<WorldPoint> PortSide(double along)
        {
            return new[] { Keel(along, BoatSides.Port), Gunwale(along, BoatSides.Port) };
        }

        private static IReadOnlyList<WorldPoint> StarboardSide(double along)
        {
            return new[] { Gunwale(along, BoatSides.Starboard), Keel(along, BoatSides.Starboard) };
        }

        private static IReadOnlyList<WorldPoint> DeckAcross(double along)
        {
            return new[] { Gunwale(along, BoatSides.Port), Gunwale(along, BoatSides.Starboard) };
        }

        private static Shape Transom()
        {
            var along = Hull.SternAlong;
            var port = BoatSides.Port;
            var starboard = BoatSides.Starboard;
            var corners = new[] { Keel(along, port), Gunwale(along, port), Gunwale(along, starboard), Keel(along, starboard) };
            return FacingPolygon.Towards(corners, BoatSides.Astern);
        }

        private static WorldPoint Gunwale(double along, double side)
        {
            return new WorldPoint(side * Hull.HalfBeamAt(along), Hull.GunwaleAt(along), along);
        }

        private static WorldPoint Keel(double along, double side)
        {
            return new WorldPoint(side * Hull.HalfBeamAt(along), Hull.KeelMetres, along);
        }
    }
}
