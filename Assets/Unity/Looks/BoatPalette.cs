using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class BoatPalette
    {
        public static readonly Tones Hull = new Tones(new Color(0.05f, 0.05f, 0.05f), new Color(0.1f, 0.09f, 0.08f), new Color(0.03f, 0.03f, 0.03f));
        public static readonly Tones Deck = new Tones(new Color(0.34f, 0.27f, 0.19f), new Color(0.27f, 0.22f, 0.16f), new Color(0.2f, 0.16f, 0.12f));
        public static readonly Tones CabinRoof = new Tones(new Color(0.3f, 0.15f, 0.11f), new Color(0.26f, 0.14f, 0.1f), new Color(0.2f, 0.11f, 0.08f));
        public static readonly Tones Cloths = new Tones(new Color(0.13f, 0.13f, 0.12f), new Color(0.19f, 0.18f, 0.16f), new Color(0.08f, 0.08f, 0.08f));
        public static readonly Tones Cream = new Tones(new Color(0.84f, 0.78f, 0.62f), new Color(0.7f, 0.64f, 0.5f), new Color(0.6f, 0.55f, 0.44f));
        public static readonly Tones Brass = new Tones(new Color(0.8f, 0.62f, 0.3f), new Color(0.6f, 0.45f, 0.2f), new Color(0.5f, 0.38f, 0.18f));
        public static readonly Color CabinFrame = new Color(0.07f, 0.22f, 0.14f);
        public static readonly Color CabinPanel = new Color(0.52f, 0.09f, 0.07f);
        public static readonly Color CoachLine = new Color(0.87f, 0.78f, 0.5f);
    }
}
