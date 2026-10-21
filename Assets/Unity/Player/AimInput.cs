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
            var mouse = Mouse.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedNow(mouse?.leftButton, gamepad?.rightTrigger);
        }
    }
}
