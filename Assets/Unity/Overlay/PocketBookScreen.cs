using System;
using ShallowWater.Game.Kit;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class PocketBookScreen
    {
        private const string BackdropClass = "book-backdrop";
        private const string ClosedClass = "book-backdrop--closed";
        private const string BookClass = "book";
        private const string PageClass = "book__page";
        private const string LeftPageClass = "book__page--left";
        private const string RightPageClass = "book__page--right";
        private const string SpineClass = "book__spine";
        private const string FootClass = "book__foot";
        private const string HintClass = "hud-hint";
        private const string HintHiddenClass = "hud-hint--hidden";
        private const string ReadingClass = "hud-safe-area--reading";

        private readonly VisualElement screen;
        private readonly VisualElement backdrop = OverlayElements.Block(BackdropClass, ClosedClass);
        private readonly VisualElement hint;

        public PocketBookScreen(VisualElement screen, Kit kit, HudPrompt open, HudPrompt close, Action takeOut)
        {
            this.screen = screen;
            var book = OverlayElements.Block(BookClass);
            var left = OverlayElements.Block(PageClass, LeftPageClass);
            var right = OverlayElements.Block(PageClass, RightPageClass);
            book.Add(left);
            book.Add(OverlayElements.Block(SpineClass));
            book.Add(right);
            KitPage = new KitPage(left, kit);
            Day = new DayPage(right);
            backdrop.Add(book);
            backdrop.Add(Foot(close));
            backdrop.pickingMode = PickingMode.Ignore;
            var table = screen.parent;
            table.Add(backdrop);
            hint = Hint(open, takeOut);
        }

        public KitPage KitPage { get; }
        public DayPage Day { get; }

        public void Open(bool isOpen)
        {
            backdrop.EnableInClassList(ClosedClass, !isOpen);
            backdrop.pickingMode = isOpen ? PickingMode.Position : PickingMode.Ignore;
            screen.EnableInClassList(ReadingClass, isOpen);
            hint.EnableInClassList(HintHiddenClass, isOpen);
        }

        private VisualElement Hint(HudPrompt open, Action takeOut)
        {
            var pill = PromptPill.Of(open);
            pill.AddToClassList(HintClass);
            pill.RegisterCallback<ClickEvent>(_ => takeOut());
            var hints = HintsRow.On(screen);
            hints.Add(pill);
            return pill;
        }

        private static VisualElement Foot(HudPrompt close)
        {
            var foot = OverlayElements.Block(FootClass);
            foot.Add(PromptPill.Of(close));
            return foot;
        }
    }
}
