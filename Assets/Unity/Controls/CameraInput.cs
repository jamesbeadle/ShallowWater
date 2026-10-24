using UnityEngine;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Controls
{
    public static class CameraInput
    {
        private const float MouseDegreesPerPixel = 0.25f;
        private const float StickDegreesPerSecond = 120f;
        private const float MetresPerScrollNotch = 0.01f;
        private const float StickDeadZone = 0.15f;

        public static Vector2 OrbitDegrees(bool isOnTheMouseAlone)
        {
            return MouseOrbit(isOnTheMouseAlone) + StickOrbit();
        }

        public static float ZoomMetres()
        {
            var mouse = Mouse.current;
            var isLookingAway = mouse == null || Attention.IsReading;
            if (isLookingAway) return 0;
            return -mouse.scroll.ReadValue().y * MetresPerScrollNotch;
        }

        private static Vector2 MouseOrbit(bool isOnTheMouseAlone)
        {
            var mouse = Mouse.current;
            var isLooking = IsLooking(mouse, isOnTheMouseAlone);
            if (!isLooking) return Vector2.zero;
            return mouse.delta.ReadValue() * MouseDegreesPerPixel;
        }

        private static bool IsLooking(Mouse mouse, bool isOnTheMouseAlone)
        {
            var isLookingAway = mouse == null || Attention.IsReading;
            if (isLookingAway) return false;
            return isOnTheMouseAlone || Buttons.IsAnyHeld(mouse.rightButton);
        }

        private static Vector2 StickOrbit()
        {
            var gamepad = Gamepad.current;
            if (gamepad == null) return Vector2.zero;
            var stick = gamepad.rightStick.ReadValue();
            if (stick.magnitude < StickDeadZone) return Vector2.zero;
            return stick * (StickDegreesPerSecond * Time.deltaTime);
        }
    }
}
