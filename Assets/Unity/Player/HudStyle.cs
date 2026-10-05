using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class HudStyle
    {
        private const int CapPadding = 10;
        private const int Unpadded = 0;

        public static GUIStyle Made(int fontSize, TextAnchor anchor)
        {
            var style = new GUIStyle { fontSize = fontSize, alignment = anchor, wordWrap = false, clipping = TextClipping.Overflow };
            var normal = style.normal;
            normal.textColor = HudInk.Ink;
            return style;
        }

        public static GUIStyle Caps(int fontSize, TextAnchor anchor)
        {
            var style = Made(fontSize, anchor);
            style.fontStyle = FontStyle.Bold;
            return style;
        }

        public static GUIStyle KeyCap(int fontSize)
        {
            var style = Caps(fontSize, TextAnchor.MiddleCenter);
            var border = HudTextures.KeyCapBorder;
            style.border = new RectOffset(border, border, border, border);
            style.padding = new RectOffset(CapPadding, CapPadding, Unpadded, Unpadded);
            var normal = style.normal;
            normal.background = HudTextures.KeyCap;
            return style;
        }
    }
}
