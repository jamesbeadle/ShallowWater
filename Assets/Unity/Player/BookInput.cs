using ShallowWater.Unity.Controls;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Player
{
    public static class BookInput
    {
        public static bool IsAskedFor()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedToRead(keyboard?.tabKey, keyboard?.iKey, gamepad?.buttonEast);
        }

        public static bool IsPutAway()
        {
            var keyboard = Keyboard.current;
            return Buttons.IsAnyPressedToRead(keyboard?.escapeKey);
        }
    }
}
