using UnityEngine.InputSystem.Controls;

namespace ShallowWater.Unity.Controls
{
    public static class Buttons
    {
        public static bool IsAnyPressedNow(params ButtonControl[] buttons)
        {
            if (Attention.IsReading) return false;
            return IsAnyPressedToRead(buttons);
        }

        public static bool IsAnyPressedToRead(params ButtonControl[] buttons)
        {
            foreach (var button in buttons)
            {
                var isPressed = button != null && button.wasPressedThisFrame;
                if (isPressed) return true;
            }
            return false;
        }

        public static bool IsAnyHeld(params ButtonControl[] buttons)
        {
            if (Attention.IsReading) return false;
            foreach (var button in buttons)
            {
                var isHeld = button != null && button.isPressed;
                if (isHeld) return true;
            }
            return false;
        }

        public static float Axis(bool isPositive, bool isNegative)
        {
            if (isPositive == isNegative) return 0;
            return isPositive ? 1 : -1;
        }
    }
}
