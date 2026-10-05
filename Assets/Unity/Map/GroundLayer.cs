using ShallowWater.Game.Ground;
using ShallowWater.Unity.Looks;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class GroundLayer
    {
        private const string GroundName = "Ground";
        private const string CountrysideName = "Countryside";
        private const string BeyondTheFarmlandName = "Beyond the farmland";
        private const string PeriodMapName = "Period map";
        private const float LayFlatDegrees = 90f;
        private const float QuadThickness = 1f;

        public static PrintedSheet Lay(GameObject fields)
        {
            var record = MapFiles.Layer<GroundRecord>(MapLayers.Ground);
            var sheet = new MapSheet(record.west, record.east, record.south, record.north);
            var beyond = Sheet(BeyondTheFarmlandName, sheet, Heights.BeyondTheFarmlandMetres);
            var beyondRenderer = beyond.GetComponent<Renderer>();
            beyondRenderer.sharedMaterial = FieldsLook.Countryside();
            var countryside = new GameObject(CountrysideName);
            var gathering = countryside.transform;
            Within(gathering, beyond);
            Within(gathering, fields);
            var periodMap = Sheet(PeriodMapName, sheet, Heights.GroundMetres);
            var print = PeriodMap.PrintOn(periodMap, record.image);
            var toggle = new GameObject(GroundName).AddComponent<PeriodMapToggle>();
            toggle.Between(countryside, periodMap);
            return new PrintedSheet(print, sheet);
        }

        private static void Within(Transform parent, GameObject part)
        {
            var placing = part.transform;
            placing.SetParent(parent, true);
        }

        private static GameObject Sheet(string name, MapSheet sheet, double heightMetres)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = name;
            Place(quad.transform, sheet, heightMetres);
            return quad;
        }

        private static void Place(Transform placement, MapSheet sheet, double heightMetres)
        {
            var centre = sheet.Centre;
            placement.position = new Vector3((float)centre.East, (float)heightMetres, (float)centre.North);
            placement.rotation = Quaternion.Euler(LayFlatDegrees, 0, 0);
            placement.localScale = new Vector3((float)sheet.WidthMetres, (float)sheet.DepthMetres, QuadThickness);
        }
    }
}
