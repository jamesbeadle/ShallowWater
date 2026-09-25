using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace ShallowWater.Unity.Helm
{
    public static class HelmInput
    {
        public static double Throttle()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return 0;
            var ahead = IsPressed(keyboard.wKey) || IsPressed(keyboard.upArrowKey);
            var astern = IsPressed(keyboard.sKey) || IsPressed(keyboard.downArrowKey);
            return Axis(ahead, astern);
        }

        public static double Rudder()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return 0;
            var starboard = IsPressed(keyboard.dKey) || IsPressed(keyboard.rightArrowKey);
            var port = IsPressed(keyboard.aKey) || IsPressed(keyboard.leftArrowKey);
            return Axis(starboard, port);
        }

        private static bool IsPressed(KeyControl key)
        {
            return key.isPressed;
        }

        private static double Axis(bool isPositive, bool isNegative)
        {
            if (isPositive == isNegative) return 0;
            if (isPositive) return 1;
            return -1;
        }
    }
}
