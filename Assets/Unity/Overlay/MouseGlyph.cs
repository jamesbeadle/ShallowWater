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

        public static VisualElement Firing()
        {
            var pressedLeftButton = OverlayElements.Block(ButtonClass, LeftClass, PressedClass);
            return Mouse(pressedLeftButton, OverlayElements.Block(ButtonClass, RightClass), OverlayElements.Block(WheelClass));
        }

        public static VisualElement Looking()
        {
            var pressedRightButton = OverlayElements.Block(ButtonClass, RightClass, PressedClass);
            return Mouse(OverlayElements.Block(ButtonClass, LeftClass), pressedRightButton, OverlayElements.Block(WheelClass));
        }

        public static VisualElement Zooming()
        {
            var turnedWheel = OverlayElements.Block(WheelClass, TurnedClass);
            return Mouse(OverlayElements.Block(ButtonClass, LeftClass), OverlayElements.Block(ButtonClass, RightClass), turnedWheel);
        }

        private static VisualElement Mouse(VisualElement leftButton, VisualElement rightButton, VisualElement wheel)
        {
            var mouse = OverlayElements.Block(MouseClass);
            mouse.Add(leftButton);
            mouse.Add(rightButton);
            mouse.Add(wheel);
            return mouse;
        }
    }
}
