using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class HudStyle
    {
        private const string BadgeFontPath = "Hud/Fonts/JosefinSans-SemiBold";

        public static GUIStyle Badge(int fontSize)
        {
            var font = Resources.Load<Font>(BadgeFontPath);
            var style = new GUIStyle { font = font, fontSize = fontSize, alignment = TextAnchor.MiddleCenter, clipping = TextClipping.Overflow };
            var normal = style.normal;
            normal.textColor = HudInk.Ink;
            return style;
        }
    }
}
