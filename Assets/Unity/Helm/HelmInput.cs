using ShallowWater.Unity.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Helm
{
    public static class HelmInput
    {
        private const float StickDeadZone = 0.2f;

        public static bool IsOpeningUp()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedNow(keyboard?.wKey, keyboard?.upArrowKey, gamepad?.rightShoulder);
        }

        public static bool IsEasingOff()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedNow(keyboard?.sKey, keyboard?.downArrowKey, gamepad?.leftShoulder);
        }

        public static bool IsStopping()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedNow(keyboard?.spaceKey, gamepad?.buttonWest);
        }

        public static double Rudder()
        {
            var keyboard = Keyboard.current;
            var toStarboard = Buttons.IsAnyHeld(keyboard?.dKey, keyboard?.rightArrowKey);
            var toPort = Buttons.IsAnyHeld(keyboard?.aKey, keyboard?.leftArrowKey);
            var keys = Buttons.Axis(toStarboard, toPort);
            return Mathf.Clamp(keys + StickAcross(), -1, 1);
        }

        private static float StickAcross()
        {
            var gamepad = Gamepad.current;
            if (gamepad == null) return 0;
            var across = gamepad.leftStick.ReadValue().x;
            return Mathf.Abs(across) < StickDeadZone ? 0 : across;
        }
    }
}
