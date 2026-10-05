using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class HudTextures
    {
        private const int Size = 64;
        private const float Smoothing = 2f / Size;
        private const float Whole = 1f;

        private static Texture2D disc;

        public static Texture2D Disc => disc = disc != null ? disc : Drawn();

        private static Texture2D Drawn()
        {
            var texture = new Texture2D(Size, Size, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
            for (var row = 0; row < Size; row++)
            {
                for (var column = 0; column < Size; column++) texture.SetPixel(column, row, Covered(column, row));
            }
            texture.Apply();
            return texture;
        }

        private static Color Covered(int column, int row)
        {
            var place = new Vector2((column + 0.5f) / Size, (row + 0.5f) / Size) * 2f - Vector2.one;
            return new Color(Whole, Whole, Whole, DiscCover(place));
        }

        private static float DiscCover(Vector2 place)
        {
            return Edge(place.magnitude - (Whole - Smoothing));
        }

        private static float Edge(float distance)
        {
            return Mathf.Clamp01(0.5f - distance / Smoothing);
        }
    }
}
