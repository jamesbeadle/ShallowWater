using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class RoadPalette
    {
        public static readonly Tones MainRoad = new Tones(new Color(0.22f, 0.22f, 0.23f), new Color(0.28f, 0.27f, 0.26f), new Color(0.15f, 0.15f, 0.16f));
        public static readonly Tones Road = new Tones(new Color(0.28f, 0.28f, 0.28f), new Color(0.34f, 0.33f, 0.31f), new Color(0.19f, 0.19f, 0.19f));
        public static readonly Tones Lane = new Tones(new Color(0.6f, 0.55f, 0.46f), new Color(0.5f, 0.45f, 0.37f), new Color(0.4f, 0.37f, 0.32f));
        public static readonly Tones RoadLine = new Tones(new Color(0.86f, 0.86f, 0.82f), new Color(0.72f, 0.72f, 0.68f), new Color(0.62f, 0.62f, 0.58f));
        public static readonly Tones TelegraphPole = new Tones(new Color(0.3f, 0.24f, 0.17f), new Color(0.25f, 0.21f, 0.16f), new Color(0.18f, 0.14f, 0.1f));
        public static readonly Tones Ballast = new Tones(new Color(0.38f, 0.35f, 0.32f), new Color(0.24f, 0.22f, 0.2f), new Color(0.2f, 0.15f, 0.11f));
        public static readonly Color Rail = new Color(0.36f, 0.3f, 0.26f);
    }
}
