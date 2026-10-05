using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class BuildingPalette
    {
        public static readonly Tones Brick = new Tones(new Color(0.52f, 0.29f, 0.21f), new Color(0.43f, 0.22f, 0.16f), new Color(0.6f, 0.56f, 0.49f));
        public static readonly Color BrickFace = new Color(0.52f, 0.29f, 0.21f);
        public static readonly Color BurntHeader = new Color(0.27f, 0.2f, 0.22f);
        public static readonly Color BlueBrick = new Color(0.17f, 0.17f, 0.2f);
        public static readonly Color RubbedBrick = new Color(0.6f, 0.31f, 0.21f);
        public static readonly Color Limewash = new Color(0.86f, 0.84f, 0.76f);
        public static readonly Color TarredPlinth = new Color(0.07f, 0.07f, 0.07f);
        public static readonly Color Soot = new Color(0.12f, 0.11f, 0.1f);
        public static readonly Color[] WindowPaints =
        {
            new Color(0.84f, 0.79f, 0.66f), new Color(0.88f, 0.87f, 0.82f), new Color(0.11f, 0.22f, 0.15f), new Color(0.28f, 0.17f, 0.1f),
        };
        public static readonly Color[] DoorPaints =
        {
            new Color(0.1f, 0.22f, 0.14f), new Color(0.36f, 0.09f, 0.09f), new Color(0.06f, 0.06f, 0.07f), new Color(0.32f, 0.19f, 0.1f),
        };
        public static readonly Color Glass = new Color(0.09f, 0.11f, 0.13f);
        public static readonly Color Room = new Color(0.05f, 0.04f, 0.035f);
        public static readonly Color NetCurtain = new Color(0.8f, 0.79f, 0.74f);
        public static readonly Color[] Curtains = { new Color(0.42f, 0.16f, 0.13f), new Color(0.3f, 0.33f, 0.22f) };
        public static readonly Color DoorFurniture = new Color(0.3f, 0.24f, 0.12f);
        public static readonly Tones Slate = new Tones(new Color(0.25f, 0.26f, 0.29f), new Color(0.2f, 0.21f, 0.25f), new Color(0.38f, 0.4f, 0.2f));
        public static readonly Tones ClayTiles = new Tones(new Color(0.5f, 0.26f, 0.18f), new Color(0.36f, 0.2f, 0.16f), new Color(0.34f, 0.38f, 0.18f));
        public static readonly Tones RidgeTiles = new Tones(new Color(0.46f, 0.22f, 0.15f), new Color(0.3f, 0.17f, 0.13f), new Color(0.2f, 0.15f, 0.12f));
        public static readonly Tones ChimneyPot = new Tones(new Color(0.6f, 0.32f, 0.2f), new Color(0.35f, 0.25f, 0.2f), new Color(0.2f, 0.16f, 0.14f));
        public static readonly Tones Timber = new Tones(new Color(0.2f, 0.15f, 0.11f), new Color(0.28f, 0.22f, 0.16f), new Color(0.12f, 0.1f, 0.08f));
        public static readonly Tones Iron = new Tones(new Color(0.07f, 0.07f, 0.08f), new Color(0.18f, 0.12f, 0.09f), new Color(0.05f, 0.05f, 0.05f));
    }
}
