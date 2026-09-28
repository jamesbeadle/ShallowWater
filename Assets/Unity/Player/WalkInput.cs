using ShallowWater.Game.Ground;
using ShallowWater.Unity.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Player
{
    public static class WalkInput
    {
        private const float StickDeadZone = 0.15f;

        public static GroundPoint Wish(float cameraYawDegrees)
        {
            var input = Vector2.ClampMagnitude(Keys() + Stick(), 1);
            var yaw = cameraYawDegrees * Mathf.Deg2Rad;
            var forward = GroundPoint.Facing(yaw);
            return forward.RightAngleClockwise * input.x + forward * input.y;
        }

        public static bool IsRunning()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            return Buttons.IsAnyHeld(keyboard?.leftShiftKey, keyboard?.rightShiftKey, gamepad?.leftStickButton, gamepad?.rightTrigger);
        }

        private static Vector2 Keys()
        {
            var keyboard = Keyboard.current;
            var ahead = Buttons.IsAnyHeld(keyboard?.wKey, keyboard?.upArrowKey);
            var back = Buttons.IsAnyHeld(keyboard?.sKey, keyboard?.downArrowKey);
            var right = Buttons.IsAnyHeld(keyboard?.dKey, keyboard?.rightArrowKey);
            var left = Buttons.IsAnyHeld(keyboard?.aKey, keyboard?.leftArrowKey);
            return new Vector2(Buttons.Axis(right, left), Buttons.Axis(ahead, back));
        }

        private static Vector2 Stick()
        {
            var gamepad = Gamepad.current;
            if (gamepad == null) return Vector2.zero;
            var stick = gamepad.leftStick.ReadValue();
            return stick.magnitude < StickDeadZone ? Vector2.zero : stick;
        }
    }
}
