using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class HudText
    {
        private const float ShadowShare = 0.07f;
        private const float LeastShadowPixels = 1f;

        public static void Shadowed(Rect place, string text, GUIStyle style)
        {
            Shadowed(place, text, style, HudInk.Unchanged);
        }

        public static void Shadowed(Rect place, string text, GUIStyle style, Color tint)
        {
            var drop = Mathf.Max(LeastShadowPixels, style.fontSize * ShadowShare);
            var shadow = HudInk.Shadow;
            GUI.color = new Color(shadow.r, shadow.g, shadow.b, shadow.a * tint.a);
            GUI.Label(new Rect(place.x + drop, place.y + drop, place.width, place.height), text, style);
            GUI.color = tint;
            GUI.Label(place, text, style);
            GUI.color = HudInk.Unchanged;
        }

        public static float WidthOf(string text, GUIStyle style)
        {
            return style.CalcSize(new GUIContent(text)).x;
        }
    }
}
