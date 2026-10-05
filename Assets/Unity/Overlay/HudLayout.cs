using UnityEngine;

namespace ShallowWater.Unity.Overlay
{
    public static class HudLayout
    {
        public const float MatchTheHeight = 1f;
        private const float MarginShare = 0.025f;
        private const float RadarShare = 0.26f;
        private const int ReferenceWidth = 1920;
        private const int ReferenceHeight = 1080;

        public static Vector2Int ReferenceScreen => new Vector2Int(ReferenceWidth, ReferenceHeight);
        public static float MarginUnits => ReferenceHeight * MarginShare;
        public static float BesideTheRadarUnits => ReferenceHeight * (RadarShare + MarginShare);
        public static float Margin => Screen.height * MarginShare;
        public static float RadarDiameter => Screen.height * RadarShare;

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
