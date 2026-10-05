using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class KeyCaps
    {
        private const char Between = '|';
        private const float GapShare = 0.25f;

        public static float Draw(Vector2 corner, float height, string keys, GUIStyle capStyle)
        {
            var x = corner.x;
            foreach (var key in keys.Split(Between))
            {
                var width = Mathf.Max(height, HudText.WidthOf(key, capStyle));
                GUI.Label(new Rect(x, corner.y, width, height), key, capStyle);
                x += width + height * GapShare;
            }
            return x - corner.x;
        }

        public static void Prompt(Vector2 corner, float height, HudPrompt prompt, GUIStyle capStyle, GUIStyle wordStyle, Color tint)
        {
            GUI.color = tint;
            var capsWidth = Draw(corner, height, prompt.Keys, capStyle);
            GUI.color = HudInk.Unchanged;
            var words = new Rect(corner.x + capsWidth, corner.y, HudText.WidthOf(prompt.Words, wordStyle), height);
            HudText.Shadowed(words, prompt.Words, wordStyle, tint);
        }
    }
}
