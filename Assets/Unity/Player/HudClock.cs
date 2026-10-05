using ShallowWater.Unity.Sky;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class HudClock : MonoBehaviour
    {
        private const int FontShare = 16;
        private const float WidthShare = 0.15f;
        private const float HeightShare = 0.08f;

        private DayAndNight day;
        private GUIStyle style;

        public void Reads(DayAndNight dayAndNight)
        {
            day = dayAndNight;
        }

        private void OnGUI()
        {
            if (day == null) return;
            style = style ?? HudStyle.Made(Screen.height / FontShare, TextAnchor.MiddleCenter);
            var margin = HudLayout.Margin;
            var width = Screen.height * WidthShare;
            var height = Screen.height * HeightShare;
            var place = new Rect(Screen.width - margin - width, margin, width, height);
            var now = day.Now;
            GUI.Label(place, now.ToString(), style);
        }
    }
}
