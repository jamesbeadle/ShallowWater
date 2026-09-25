using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class Palette
    {
        public static readonly Color Water = new Color(0.16f, 0.20f, 0.17f);
        public static readonly Color Towpath = new Color(0.45f, 0.38f, 0.27f);
        public static readonly Color Bank = new Color(0.30f, 0.34f, 0.18f);

        private static readonly Dictionary<Surface, Color> BySurface = new Dictionary<Surface, Color>
        {
            { Surface.Water, Water },
            { Surface.Towpath, Towpath },
            { Surface.Bank, Bank },
        };

        public static Color For(Surface surface)
        {
            return BySurface[surface];
        }
    }
}
