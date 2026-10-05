using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public static class PromptPill
    {
        private const string PillClass = "hud-prompt";
        private const string WordsClass = "hud-prompt__words";

        public static VisualElement Of(HudPrompt prompt)
        {
            var pill = OverlayElements.Glass(PillClass);
            pill.Add(KeyGlyphs.For(prompt.Keys));
            pill.Add(OverlayElements.Words(prompt.Words, WordsClass));
            return pill;
        }
    }
}
