using ShallowWater.Unity.Overlay;
using ShallowWater.Unity.Sky;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Player
{
    public sealed class HudClock : MonoBehaviour
    {
        private DayAndNight day;
        private ClockFace face;

        public void Reads(DayAndNight dayAndNight, VisualElement screen)
        {
            day = dayAndNight;
            face = new ClockFace(screen, day.Now);
        }

        private void LateUpdate()
        {
            if (face == null) return;
            face.Show(day.Now);
        }
    }
}
