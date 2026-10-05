using System;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class HudTextures
    {
        public const int KeyCapBorder = 20;
        private const int Size = 64;
        private const int FadeLength = 256;
        private const float CornerRadius = 0.32f;
        private const float HalfLine = 0.035f;
        private const float CapFill = 0.16f;
        private const float RingHalfLine = 0.07f;
        private const float RingRadius = 0.86f;
        private const float Smoothing = 2f / Size;
        private const float Whole = 1f;

        private static Texture2D keyCap;
        private static Texture2D disc;
        private static Texture2D ring;
        private static Texture2D fade;

        public static Texture2D KeyCap => keyCap = keyCap != null ? keyCap : Drawn(KeyCapCover);
        public static Texture2D Disc => disc = disc != null ? disc : Drawn(DiscCover);
        public static Texture2D Ring => ring = ring != null ? ring : Drawn(RingCover);
        public static Texture2D Fade => fade = fade != null ? fade : Faded();

        private static Texture2D Drawn(Func<Vector2, float> cover)
        {
            var texture = new Texture2D(Size, Size, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
            for (var row = 0; row < Size; row++)
            {
                for (var column = 0; column < Size; column++) texture.SetPixel(column, row, Covered(cover, column, row));
            }
            texture.Apply();
            return texture;
        }

        private static Color Covered(Func<Vector2, float> cover, int column, int row)
        {
            var place = new Vector2((column + 0.5f) / Size, (row + 0.5f) / Size) * 2f - Vector2.one;
            return new Color(Whole, Whole, Whole, Mathf.Clamp01(cover(place)));
        }

        private static float KeyCapCover(Vector2 place)
        {
            var inner = Whole - CornerRadius - HalfLine * 2f;
            var beyond = new Vector2(Mathf.Abs(place.x) - inner, Mathf.Abs(place.y) - inner);
            var outside = new Vector2(Mathf.Max(beyond.x, 0f), Mathf.Max(beyond.y, 0f));
            var distance = outside.magnitude + Mathf.Min(Mathf.Max(beyond.x, beyond.y), 0f) - CornerRadius;
            var line = Edge(Mathf.Abs(distance) - HalfLine);
            var fill = Edge(distance) * CapFill;
            return Mathf.Max(line, fill);
        }

        private static float DiscCover(Vector2 place)
        {
            return Edge(place.magnitude - (Whole - Smoothing));
        }

        private static float RingCover(Vector2 place)
        {
            return Edge(Mathf.Abs(place.magnitude - RingRadius) - RingHalfLine);
        }

        private static float Edge(float distance)
        {
            return Mathf.Clamp01(0.5f - distance / Smoothing);
        }

        private static Texture2D Faded()
        {
            var texture = new Texture2D(FadeLength, 1, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
            for (var column = 0; column < FadeLength; column++)
            {
                var away = (float)column / (FadeLength - 1);
                texture.SetPixel(column, 0, new Color(Whole, Whole, Whole, Whole - Mathf.SmoothStep(0f, Whole, away)));
            }
            texture.Apply();
            return texture;
        }
    }
}
