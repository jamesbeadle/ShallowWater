using ShallowWater.Unity.Controls;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Player
{
    public static class KeyInput
    {
        public static bool IsTogglingTheKey()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyPressedNow(keyboard?.hKey, gamepad?.selectButton);
        }
    }
}
