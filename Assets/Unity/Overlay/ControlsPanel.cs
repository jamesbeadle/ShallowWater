using System.Collections.Generic;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class ControlsPanel
    {
        private const string Eyebrow = "CONTROLS";
        private const string PanelClass = "hud-controls";
        private const string HiddenClass = "hud-controls--hidden";
        private const string HintClass = "hud-hint";
        private const string HintHiddenClass = "hud-hint--hidden";
        private const string TitleClass = "hud-title";
        private const string RowClass = "hud-row";
        private const string ActionClass = "hud-row__action";
        private const string FooterClass = "hud-footer";
        private const string FooterWordsClass = "hud-footer__words";

        private readonly VisualElement hint;
        private readonly VisualElement panel = OverlayElements.Glass(PanelClass, HiddenClass);
        private readonly Label title = OverlayElements.Heading(string.Empty, TitleClass);
        private readonly VisualElement rows = OverlayElements.Block();

        public ControlsPanel(VisualElement screen, HudPrompt open, HudPrompt close)
        {
            hint = PromptPill.Of(open);
            hint.AddToClassList(HintClass);
            hint.RegisterCallback<ClickEvent>(_ => Turn(isOpening: true));
            panel.Add(OverlayElements.Eyebrow(Eyebrow));
            panel.Add(title);
            panel.Add(rows);
            panel.Add(Footer(close));
            screen.Add(panel);
            screen.Add(hint);
        }

        public bool IsOpen { get; private set; }

        public void Toggle()
        {
            Turn(!IsOpen);
        }

        public void Show(string heading, IEnumerable<HudPrompt> controls)
        {
            title.text = heading;
            rows.Clear();
            foreach (var control in controls) rows.Add(Row(control));
        }

        private void Turn(bool isOpening)
        {
            IsOpen = isOpening;
            panel.EnableInClassList(HiddenClass, !isOpening);
            hint.EnableInClassList(HintHiddenClass, isOpening);
        }

        private VisualElement Footer(HudPrompt close)
        {
            var footer = OverlayElements.Block(FooterClass);
            footer.Add(KeyGlyphs.For(close.Keys));
            footer.Add(OverlayElements.Words(close.Words, FooterWordsClass));
            footer.RegisterCallback<ClickEvent>(_ => Turn(isOpening: false));
            return footer;
        }

        private static VisualElement Row(HudPrompt control)
        {
            var row = OverlayElements.Block(RowClass);
            row.Add(OverlayElements.Words(control.Words, ActionClass));
            row.Add(KeyGlyphs.For(control.Keys));
            return row;
        }
    }
}
