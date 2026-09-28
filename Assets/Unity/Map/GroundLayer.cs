using ShallowWater.Game.Ground;
using ShallowWater.Unity.Looks;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class GroundLayer
    {
        private const string GroundName = "Ground";
        private const string CountrysideName = "Countryside";
        private const string PeriodMapName = "Period map";
        private const float LayFlatDegrees = 90f;
        private const float QuadThickness = 1f;

        public static void Lay()
        {
            var record = MapFiles.Layer<GroundRecord>(MapLayers.Ground);
            var sheet = new MapSheet(record.west, record.east, record.south, record.north);
            var countryside = Sheet(CountrysideName, sheet);
            var countrysideRenderer = countryside.GetComponent<Renderer>();
            countrysideRenderer.sharedMaterial = FieldsLook.Countryside();
            var periodMap = Sheet(PeriodMapName, sheet);
            PeriodMap.PrintOn(periodMap, record.image);
            var toggle = new GameObject(GroundName).AddComponent<PeriodMapToggle>();
            toggle.Between(countryside, periodMap);
        }

        private static GameObject Sheet(string name, MapSheet sheet)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = name;
            Place(quad.transform, sheet);
            return quad;
        }

        private static void Place(Transform placement, MapSheet sheet)
        {
            var centre = sheet.Centre;
            placement.position = new Vector3((float)centre.East, (float)Heights.GroundMetres, (float)centre.North);
            placement.rotation = Quaternion.Euler(LayFlatDegrees, 0, 0);
            placement.localScale = new Vector3((float)sheet.WidthMetres, (float)sheet.DepthMetres, QuadThickness);
        }
    }
}
