using System;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class TitleButton
    {
        private const string ButtonClass = "title-button";
        private const string MarkClass = "title-button__mark";
        private const string WordsClass = "title-button__words";
        private const string DetailClass = "title-button__detail";

        public static Button Of(string words, Action chosen)
        {
            var button = new Button(chosen) { text = string.Empty };
            button.RemoveFromClassList(Button.ussClassName);
            button.AddToClassList(ButtonClass);
            button.Add(OverlayElements.Block(MarkClass));
            button.Add(OverlayElements.Words(words.ToUpperInvariant(), WordsClass));
            button.RegisterCallback<PointerMoveEvent>(_ => button.Focus());
            return button;
        }

        public static Button Of(string words, string detail, Action chosen)
        {
            var button = Of(words, chosen);
            button.Add(OverlayElements.Words(detail, DetailClass));
            return button;
        }
    }
}
