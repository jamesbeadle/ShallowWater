using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public static class OverlayElements
    {
        private const string TextClass = "hud-text";
        private const string DisplayClass = "hud-display";
        private const string GlassClass = "hud-glass";
        private const string EyebrowClass = "hud-eyebrow";

        public static VisualElement Block(params string[] classNames)
        {
            return Classed(new VisualElement(), classNames);
        }

        public static VisualElement Glass(params string[] classNames)
        {
            var glass = Block(classNames);
            glass.AddToClassList(GlassClass);
            return glass;
        }

        public static Label Words(string text, params string[] classNames)
        {
            var label = Classed(new Label(text), classNames);
            label.AddToClassList(TextClass);
            return label;
        }

        public static Label Heading(string text, params string[] classNames)
        {
            var label = Words(text, classNames);
            label.AddToClassList(DisplayClass);
            return label;
        }

        public static Label Eyebrow(string text)
        {
            return Heading(text, EyebrowClass);
        }

        private static T Classed<T>(T element, string[] classNames) where T : VisualElement
        {
            foreach (var className in classNames) element.AddToClassList(className);
            return element;
        }
    }
}
