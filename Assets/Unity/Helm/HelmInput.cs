using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Helm
{
    public static class HelmInput
    {
        public static double Throttle()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return 0;
            var ahead = keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed;
            var astern = keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed;
            return Axis(ahead, astern);
        }

        public static double Rudder()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return 0;
            var starboard = keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed;
            var port = keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed;
            return Axis(starboard, port);
        }

        private static double Axis(bool isPositive, bool isNegative)
        {
            if (isPositive == isNegative) return 0;
            if (isPositive) return 1;
            return -1;
        }
    }
}
