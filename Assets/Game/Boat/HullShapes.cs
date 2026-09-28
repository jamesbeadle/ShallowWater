using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class HullShapes
    {
        private const double PlateMetres = 0.04;

        public static void Build(SurfaceShapes surfaces)
        {
            var stations = Hull.Stations(Hull.SternAlong, Hull.StemAlong);
            surfaces.Add(Surface.Hull, Sweep.Lengthways(stations.Select(PortSide).ToList()));
            surfaces.Add(Surface.Hull, Sweep.Lengthways(stations.Select(StarboardSide).ToList()));
            surfaces.Add(Surface.RubbingStrake, Sweep.Lengthways(stations.Select(PortCapping).ToList()));
            surfaces.Add(Surface.RubbingStrake, Sweep.Lengthways(stations.Select(StarboardCapping).ToList()));
            surfaces.Add(Surface.RubbingStrake, Sweep.Lengthways(stations.Select(PortInside).ToList()));
            surfaces.Add(Surface.RubbingStrake, Sweep.Lengthways(stations.Select(StarboardInside).ToList()));
            surfaces.Add(Surface.Deck, Sweep.Lengthways(stations.Select(DeckAcross).ToList()));
            surfaces.Add(Surface.Hull, Transom());
            RubbingStrakes.Build(surfaces);
        }

        private static IReadOnlyList<WorldPoint> PortSide(double along)
        {
            return new[] { Keel(along, BoatSides.Port), Gunwale(along, BoatSides.Port, 0) };
        }

        private static IReadOnlyList<WorldPoint> StarboardSide(double along)
        {
            return new[] { Gunwale(along, BoatSides.Starboard, 0), Keel(along, BoatSides.Starboard) };
        }

        private static IReadOnlyList<WorldPoint> PortCapping(double along)
        {
            return new[] { Gunwale(along, BoatSides.Port, 0), Gunwale(along, BoatSides.Port, PlateMetres) };
        }

        private static IReadOnlyList<WorldPoint> StarboardCapping(double along)
        {
            return new[] { Gunwale(along, BoatSides.Starboard, PlateMetres), Gunwale(along, BoatSides.Starboard, 0) };
        }

        private static IReadOnlyList<WorldPoint> PortInside(double along)
        {
            return new[] { Gunwale(along, BoatSides.Port, PlateMetres), DeckEdge(along, BoatSides.Port) };
        }

        private static IReadOnlyList<WorldPoint> StarboardInside(double along)
        {
            return new[] { DeckEdge(along, BoatSides.Starboard), Gunwale(along, BoatSides.Starboard, PlateMetres) };
        }

        private static IReadOnlyList<WorldPoint> DeckAcross(double along)
        {
            return new[] { DeckEdge(along, BoatSides.Port), DeckEdge(along, BoatSides.Starboard) };
        }

        private static Shape Transom()
        {
            var along = Hull.SternAlong;
            var port = BoatSides.Port;
            var starboard = BoatSides.Starboard;
            var corners = new[] { Keel(along, port), Gunwale(along, port, 0), Gunwale(along, starboard, 0), Keel(along, starboard) };
            return FacingPolygon.Towards(corners, BoatSides.Astern);
        }

        private static WorldPoint Gunwale(double along, double side, double inboard)
        {
            var across = side * (Hull.HalfBeamAt(along) - inboard);
            return new WorldPoint(across, Hull.GunwaleAt(along), along + Hull.RakeAt(along));
        }

        private static WorldPoint DeckEdge(double along, double side)
        {
            var deck = Hull.DeckAt(along);
            var across = side * (Hull.HalfBeamAt(along) - PlateMetres);
            return new WorldPoint(across, deck, along + Hull.RakeAt(along, deck));
        }

        private static WorldPoint Keel(double along, double side)
        {
            return new WorldPoint(side * Hull.HalfBeamAt(along), Hull.KeelMetres, along);
        }
    }
}
