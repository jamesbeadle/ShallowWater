using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public readonly struct CropTones
    {
        public CropTones(Color ground, Color worn, Color growth, Color accent)
        {
            Ground = ground;
            Worn = worn;
            Growth = growth;
            Accent = accent;
        }

        public Color Ground { get; }
        public Color Worn { get; }
        public Color Growth { get; }
        public Color Accent { get; }
    }
}
