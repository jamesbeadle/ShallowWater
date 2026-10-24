using ShallowWater.Unity.Controls;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Player
{
    public static class AimInput
    {
        public static bool IsShouldering()
        {
            var mouse = Mouse.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyHeld(mouse?.rightButton, gamepad?.leftTrigger);
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
