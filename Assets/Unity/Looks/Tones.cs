using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public readonly struct Tones
    {
        public Tones(Color main, Color worn, Color accent)
        {
            Main = main;
            Worn = worn;
            Accent = accent;
        }

        public Color Main { get; }
        public Color Worn { get; }
        public Color Accent { get; }
    }
}
