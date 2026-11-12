using System.Collections.Generic;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public sealed class TitlePages
    {
        private const string HolderClass = "title__pages";
        private const string InsideClass = "title-screen--inside";
        private const string EnteringClass = "title-page--entering";
        private const long EnteringDelayMilliseconds = 30;
        private const int FrontPageAlone = 1;

        private readonly VisualElement holder = OverlayElements.Block(HolderClass);
        private readonly Stack<VisualElement> trail = new Stack<VisualElement>();
        private readonly Stack<Focusable> leftAt = new Stack<Focusable>();
        private readonly VisualElement title;

        public TitlePages(VisualElement titleScreen)
        {
            title = titleScreen;
            title.Add(holder);
            title.RegisterCallback<NavigationCancelEvent>(_ => Back());
            title.RegisterCallback<PointerDownEvent>(KeepTheFocus);
        }

        private bool IsInside => trail.Count > FrontPageAlone;

        public void Open(VisualElement page)
        {
            var focus = title.focusController;
            leftAt.Push(focus.focusedElement);
            trail.Push(page);
            Show(page, page.Q<Button>());
        }

        public void Back()
        {
            if (!IsInside) return;
            trail.Pop();
            Show(trail.Peek(), leftAt.Pop());
        }

        private void Show(VisualElement page, Focusable resting)
        {
            holder.Clear();
            holder.Add(page);
            title.EnableInClassList(InsideClass, IsInside);
            page.AddToClassList(EnteringClass);
            var scheduler = page.schedule;
            scheduler.Execute(() => Settle(page, resting)).StartingIn(EnteringDelayMilliseconds);
        }

        private static void Settle(VisualElement page, Focusable resting)
        {
            page.RemoveFromClassList(EnteringClass);
            resting?.Focus();
        }

        private void KeepTheFocus(PointerDownEvent press)
        {
            var focus = title.focusController;
            focus.IgnoreEvent(press);
        }
    }
}
