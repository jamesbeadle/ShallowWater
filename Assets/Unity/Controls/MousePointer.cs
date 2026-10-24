using UnityEngine;

namespace ShallowWater.Unity.Controls
{
    public static class MousePointer
    {
        public static void Lock(bool isLocked)
        {
            Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isLocked;
        }
    }
}
