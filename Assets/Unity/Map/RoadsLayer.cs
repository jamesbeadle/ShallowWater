using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Roads;

namespace ShallowWater.Unity.Map
{
    public static class RoadsLayer
    {
        private const string LayerName = "Roads";

        public static RoadNetwork Lay(Pound pound)
        {
            var roads = OnTheMap();
            var network = new RoadNetwork(roads, new RoadSurroundings(Clearings(pound, roads), Villages()));
            var isOnTheMap = OptionalLayer.IsPresent(MapLayers.Roads);
            if (isOnTheMap) Pave(network);
            return network;
        }

        private static void Pave(RoadNetwork network)
        {
            Gatherings.Of(LayerName, Solidity.Hardened(ShapeMeshes.BuildEach(LayerName, network.Shapes)));
        }

        public static List<Road> OnTheMap()
        {
            var roads = new List<Road>();
            if (!MapFiles.Has(MapLayers.Roads)) return roads;
            foreach (var record in MapLines.Records(MapLayers.Roads)) AddRoad(roads, record);
            return roads;
        }

        private static void AddRoad(List<Road> roads, LineRecord record)
        {
            var line = record.ToLine();
            if (!line.IsDrawable) return;
            var roadClass = RoadClasses.For(record.kind);
            roads.Add(new Road(line, roadClass, roads.Count));
        }

        private static RoadsideClearings Clearings(Pound pound, IEnumerable<Road> roads)
        {
            var clearings = new RoadsideClearings();
            clearings.ClearAroundThePound(pound.Line);
            ClearAroundLayer(MapLayers.Canal, clearings.ClearAroundCanal);
            ClearAroundLayer(MapLayers.River, clearings.ClearAroundTheRiver);
            ClearAroundLayer(MapLayers.Railway, clearings.ClearAroundTheRailway);
            foreach (var road in roads) clearings.ClearAroundRoad(road);
            return clearings;
        }

        private static void ClearAroundLayer(string layerName, Action<GroundLine> clearAround)
        {
            if (!MapFiles.Has(layerName)) return;
            foreach (var line in MapLines.Read(layerName)) clearAround(line);
        }

        private static List<Area> Villages()
        {
            if (!OptionalLayer.IsPresent(MapLayers.Villages)) return new List<Area>();
            return MapAreas.Read(MapLayers.Villages);
        }
    }
}
