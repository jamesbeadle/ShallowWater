using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public static class MouseGlyph
    {
        private const string MouseClass = "hud-mouse";
        private const string ButtonClass = "hud-mouse__button";
        private const string LeftClass = "hud-mouse__button--left";
        private const string RightClass = "hud-mouse__button--right";
        private const string PressedClass = "hud-mouse__button--pressed";
        private const string WheelClass = "hud-mouse__wheel";
        private const string TurnedClass = "hud-mouse__wheel--turned";

        public static VisualElement Looking()
        {
            var pressedRightButton = OverlayElements.Block(ButtonClass, RightClass, PressedClass);
            return Mouse(pressedRightButton, OverlayElements.Block(WheelClass));
        }

        public static VisualElement Zooming()
        {
            var turnedWheel = OverlayElements.Block(WheelClass, TurnedClass);
            return Mouse(OverlayElements.Block(ButtonClass, RightClass), turnedWheel);
        }

        private static VisualElement Mouse(VisualElement rightButton, VisualElement wheel)
        {
            var mouse = OverlayElements.Block(MouseClass);
            mouse.Add(OverlayElements.Block(ButtonClass, LeftClass));
            mouse.Add(rightButton);
            mouse.Add(wheel);
            return mouse;
        }
    }
}
