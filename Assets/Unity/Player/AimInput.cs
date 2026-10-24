using ShallowWater.Unity.Controls;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Player
{
    public static class AimInput
    {
        public static bool IsTogglingTheAim()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedNow(keyboard?.zKey, gamepad?.leftTrigger);
        }

        public static bool IsChangingTheRifle()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            var dpad = gamepad?.dpad;
            return Buttons.IsAnyPressedNow(keyboard?.xKey, dpad?.up);
        }

        public static bool IsFiring()
        {
            var keyboard = Keyboard.current;
            var mouse = Mouse.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedNow(keyboard?.spaceKey, mouse?.leftButton, gamepad?.rightTrigger);
        }
    }
}
