using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class HudInk
    {
        public static readonly Color Ink = new Color(0.95f, 0.92f, 0.84f);
        public static readonly Color Faint = new Color(0.95f, 0.92f, 0.84f, 0.45f);
        public static readonly Color Shadow = new Color(0f, 0f, 0f, 0.6f);
        public static readonly Color Backdrop = new Color(0.03f, 0.03f, 0.05f, 0.62f);
        public static readonly Color Ahead = new Color(0.98f, 0.76f, 0.38f);
        public static readonly Color Astern = new Color(0.9f, 0.45f, 0.33f);
        public static readonly Color Rim = new Color(0.12f, 0.12f, 0.16f, 0.92f);
        public static readonly Color Unchanged = Color.white;
        public static readonly Color Hint = new Color(1f, 1f, 1f, 0.55f);
    }
}
