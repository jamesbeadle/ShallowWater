using System;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class TitleButton
    {
        private const string ButtonClass = "title-button";
        private const string WordsClass = "title-button__words";
        private const string DetailClass = "title-button__detail";

        public static Button Of(string words, Action chosen)
        {
            var button = new Button(chosen) { text = string.Empty };
            button.AddToClassList(ButtonClass);
            button.Add(OverlayElements.Heading(words, WordsClass));
            return button;
        }

        public static Button Of(string words, string detail, Action chosen)
        {
            var button = Of(words, chosen);
            button.Add(OverlayElements.Words(detail, DetailClass));
            return button;
        }

        public static Button Unavailable(string words, string detail)
        {
            var button = Of(words, detail, null);
            button.SetEnabled(false);
            button.focusable = false;
            return button;
        }
    }
}
