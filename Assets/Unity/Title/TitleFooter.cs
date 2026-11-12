using ShallowWater.Unity.Overlay;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class TitleFooter
    {
        private const string FooterClass = "title__footer";
        private const string HintsClass = "title__hints";
        private const string BackHintClass = "title__hint--back";

        private static readonly HudPrompt Choose = new HudPrompt("Enter", "Choose");
        private static readonly HudPrompt GoBack = new HudPrompt("Esc", "Back");

        private static string Version => $"v{Application.version}".ToUpperInvariant();

        public static VisualElement Of()
        {
            var footer = OverlayElements.Block(FooterClass);
            footer.Add(Hints());
            footer.Add(OverlayElements.Eyebrow(Version));
            return footer;
        }

        private static VisualElement Hints()
        {
            var hints = OverlayElements.Block(HintsClass);
            var back = PromptPill.Hint(GoBack);
            back.AddToClassList(BackHintClass);
            hints.Add(PromptPill.Hint(Choose));
            hints.Add(back);
            return hints;
        }
    }
}
