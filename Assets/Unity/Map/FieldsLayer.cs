using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Fields;
using ShallowWater.Game.Pound;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class FieldsLayer
    {
        private const string FieldsName = "Fields";
        private const string HedgerowsName = "Hedgerows";

        public static Countryside Lay(Pound pound)
        {
            var unfarmed = MapAreas.ReadIfPresent(MapLayers.Woods).Concat(MapAreas.ReadIfPresent(MapLayers.Villages));
            var farmland = new Farmland(pound.Line, MapLines.ReadIfPresent(MapLayers.River), KeptClearOf(pound), unfarmed);
            var shapes = CountrysideShapes.Of(FieldPlan.Of(farmland), farmland);
            var fields = Gathered(FieldsName, ShapeMeshes.BuildEach(FieldsName, shapes.Fields));
            var hedgerows = shapes.HedgerowTiles.SelectMany(tile => ShapeMeshes.BuildEach(HedgerowsName, tile));
            Gathered(HedgerowsName, hedgerows);
            return new Countryside(fields, shapes.HedgerowTrees);
        }

        private static KeptClear KeptClearOf(Pound pound)
        {
            var keptClear = new KeptClear();
            keptClear.AroundThePound(pound.Line);
            foreach (var canal in MapLines.ReadIfPresent(MapLayers.Canal)) keptClear.AroundCanal(canal);
            foreach (var river in MapLines.ReadIfPresent(MapLayers.River)) keptClear.AroundTheRiver(river);
            foreach (var railway in MapLines.ReadIfPresent(MapLayers.Railway)) keptClear.AroundTheRailway(railway);
            foreach (var road in RoadsLayer.OnTheMap()) keptClear.AroundRoad(road);
            return keptClear;
        }

        private static GameObject Gathered(string name, IEnumerable<GameObject> parts)
        {
            var gathering = new GameObject(name);
            var parent = gathering.transform;
            foreach (var part in parts) Within(parent, part);
            return gathering;
        }

        private static void Within(Transform parent, GameObject part)
        {
            var placing = part.transform;
            placing.SetParent(parent, false);
        }
    }
}
