using System;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class TitlePage
    {
        private const string PageClass = "title-page";
        private const string HeadingClass = "title-page__heading";
        private const string BackWords = "Back";

        public static VisualElement Front(VisualElement menu)
        {
            var page = OverlayElements.Block(PageClass);
            page.Add(TitleBranding.Lockup());
            page.Add(menu);
            return page;
        }

        public static VisualElement Inside(string heading, VisualElement body, Action back)
        {
            var page = OverlayElements.Block(PageClass);
            page.Add(OverlayElements.Eyebrow(TitleBranding.RunningHead));
            page.Add(OverlayElements.Words(heading.ToUpperInvariant(), HeadingClass));
            page.Add(body);
            page.Add(TitleButton.Of(BackWords, back));
            return page;
        }
    }
}
