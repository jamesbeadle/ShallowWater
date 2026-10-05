using ShallowWater.Unity.Sky;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class HudClock : MonoBehaviour
    {
        private const int FontShare = 24;
        private const float WidthShare = 0.3f;
        private const float HeightShare = 0.07f;

        private DayAndNight day;
        private GUIStyle style;

        public void Reads(DayAndNight dayAndNight)
        {
            day = dayAndNight;
        }

        private void OnGUI()
        {
            if (day == null) return;
            style = style ?? HudStyle.Made(Screen.height / FontShare, TextAnchor.UpperRight);
            var margin = HudLayout.Margin;
            var width = Screen.height * WidthShare;
            var height = Screen.height * HeightShare;
            var place = new Rect(Screen.width - margin - width, margin, width, height);
            var now = day.Now;
            HudText.Shadowed(place, now.ToString(), style);
        }
    }
}
