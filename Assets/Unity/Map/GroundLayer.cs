using ShallowWater.Game.Ground;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class GroundLayer
    {
        private const float LayFlatDegrees = 90f;
        private const float QuadThickness = 1f;
        private const int SizeBeforeLoading = 2;
        private const int SharpestAnisotropy = 8;

        public static void Lay()
        {
            var record = MapFiles.Layer<GroundRecord>(MapLayers.Ground);
            var sheet = new MapSheet(record.west, record.east, record.south, record.north);
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = "Period map";
            Place(quad.transform, sheet);
            Paint(quad, PeriodMap(record.image));
        }

        private static void Place(Transform placement, MapSheet sheet)
        {
            var centre = sheet.Centre;
            placement.position = new Vector3((float)centre.East, (float)Heights.GroundMetres, (float)centre.North);
            placement.rotation = Quaternion.Euler(LayFlatDegrees, 0, 0);
            placement.localScale = new Vector3((float)sheet.WidthMetres, (float)sheet.DepthMetres, QuadThickness);
        }

        private static Texture2D PeriodMap(string imageName)
        {
            var texture = new Texture2D(SizeBeforeLoading, SizeBeforeLoading, TextureFormat.RGB24, true);
            texture.LoadImage(MapFiles.Image(imageName));
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.anisoLevel = SharpestAnisotropy;
            return texture;
        }

        private static void Paint(GameObject quad, Texture2D texture)
        {
            var renderer = quad.GetComponent<Renderer>();
            var material = renderer.material;
            material.mainTexture = texture;
        }
    }
}
