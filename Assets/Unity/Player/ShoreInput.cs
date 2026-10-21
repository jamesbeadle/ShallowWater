using ShallowWater.Unity.Controls;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Player
{
    public static class ShoreInput
    {
        public static bool IsSteppingAcross()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedNow(keyboard?.eKey, gamepad?.buttonSouth);
        }

        public static bool IsTalking()
        {
            return IsSteppingAcross();
        }

        public static bool IsMooring()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedNow(keyboard?.tKey, gamepad?.buttonNorth);
        }
    }
}
