using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class HudStyle
    {
        private static readonly Color Ink = new Color(0.93f, 0.9f, 0.82f);
        private static readonly Color Shade = new Color(0f, 0f, 0f, 0.35f);
        private const int Pad = 8;

        public static GUIStyle Made(int fontSize)
        {
            var backing = new Texture2D(1, 1);
            backing.SetPixel(0, 0, Shade);
            backing.Apply();
            var style = new GUIStyle { fontSize = fontSize, alignment = TextAnchor.LowerLeft, padding = new RectOffset(Pad, Pad, Pad, Pad) };
            var normal = style.normal;
            normal.textColor = Ink;
            normal.background = backing;
            return style;
        }
    }
}
