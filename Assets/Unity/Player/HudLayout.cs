using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class HudLayout
    {
        private const float MarginShare = 0.025f;
        private const float RadarShare = 0.26f;

        public static float Margin => Screen.height * MarginShare;
        public static float RadarDiameter => Screen.height * RadarShare;
        public static float BesideTheRadar => Margin * 2 + RadarDiameter;

        public static Rect RadarPlace
        {
            get
            {
                var diameter = RadarDiameter;
                return new Rect(Margin, Screen.height - Margin - diameter, diameter, diameter);
            }
        }
    }
}
