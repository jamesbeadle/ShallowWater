using ShallowWater.Unity.Controls;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Saving
{
    public static class SaveInput
    {
        public static bool IsAskedFor()
        {
            var keyboard = Keyboard.current;
            return Buttons.IsAnyPressedNow(keyboard?.f5Key);
        }
    }
}
