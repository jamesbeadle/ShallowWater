using ShallowWater.Game.Missions;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class SpeechCard
    {
        private const string RowClass = "hud-speech-row";
        private const string CardClass = "hud-speech";
        private const string HiddenClass = "hud-speech--hidden";
        private const string WordsClass = "hud-speech__words";

        private readonly VisualElement card = OverlayElements.Glass(CardClass, HiddenClass);
        private readonly Label speaker = OverlayElements.Eyebrow(string.Empty);
        private readonly Label words = OverlayElements.Words(string.Empty, WordsClass);

        public SpeechCard(VisualElement screen)
        {
            var row = OverlayElements.Block(RowClass);
            row.pickingMode = PickingMode.Ignore;
            card.pickingMode = PickingMode.Ignore;
            card.Add(speaker);
            card.Add(words);
            row.Add(card);
            screen.Add(row);
        }

        public void Show(Conversation conversation)
        {
            var isTalking = conversation.IsTalking;
            card.EnableInClassList(HiddenClass, !isTalking);
            if (!isTalking) return;
            speaker.text = conversation.Speaker.ToUpperInvariant();
            words.text = conversation.Line;
        }
    }
}
