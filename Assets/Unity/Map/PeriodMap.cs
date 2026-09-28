using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class PeriodMap
    {
        private const int SizeBeforeLoading = 2;
        private const int SharpestAnisotropy = 8;

        public static void PrintOn(GameObject sheet, string imageName)
        {
            var renderer = sheet.GetComponent<Renderer>();
            var material = renderer.material;
            material.mainTexture = Printed(imageName);
        }

        private static Texture2D Printed(string imageName)
        {
            var texture = new Texture2D(SizeBeforeLoading, SizeBeforeLoading, TextureFormat.RGB24, true);
            texture.LoadImage(MapFiles.Image(imageName));
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.anisoLevel = SharpestAnisotropy;
            return texture;
        }
    }
}
