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
            var fields = Gatherings.Of(FieldsName, Solidity.Hardened(ShapeMeshes.BuildEach(FieldsName, shapes.Fields)));
            var hedgerows = shapes.HedgerowTiles.SelectMany(tile => ShapeMeshes.BuildEach(HedgerowsName, tile));
            Gatherings.Of(HedgerowsName, Solidity.Hardened(hedgerows));
            return new Countryside(fields, shapes.Standards, shapes.Hedges);
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
    }
}
