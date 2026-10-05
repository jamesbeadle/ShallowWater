using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class PromptColumn
    {
        private const string ColumnClass = "hud-prompts";
        private const string EnteringClass = "hud-prompt--entering";
        private const long EnteringDelayMilliseconds = 30;

        private readonly VisualElement column = OverlayElements.Block(ColumnClass);
        private readonly List<HudPrompt> shown = new List<HudPrompt>();

        public PromptColumn(VisualElement screen)
        {
            screen.Add(column);
        }

        public void Show(IReadOnlyList<HudPrompt> prompts)
        {
            var isUnchanged = prompts.SequenceEqual(shown);
            if (isUnchanged) return;
            shown.Clear();
            shown.AddRange(prompts);
            column.Clear();
            foreach (var prompt in prompts) column.Add(Entering(PromptPill.Of(prompt)));
        }

        private static VisualElement Entering(VisualElement pill)
        {
            pill.AddToClassList(EnteringClass);
            var scheduler = pill.schedule;
            scheduler.Execute(() => pill.RemoveFromClassList(EnteringClass)).StartingIn(EnteringDelayMilliseconds);
            return pill;
        }
    }
}
