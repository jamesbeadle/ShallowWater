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
        public static readonly Color River = new Color(0.20f, 0.29f, 0.33f);
        public static readonly Color Railway = new Color(0.27f, 0.23f, 0.21f);
        public static readonly Color MainRoad = new Color(0.52f, 0.51f, 0.49f);
        public static readonly Color Road = new Color(0.61f, 0.58f, 0.52f);
        public static readonly Color Lane = new Color(0.66f, 0.58f, 0.44f);

        private static readonly Dictionary<Surface, Color> BySurface = new Dictionary<Surface, Color>
        {
            { Surface.Water, Water },
            { Surface.Towpath, Towpath },
            { Surface.Bank, Bank },
            { Surface.River, River },
            { Surface.Railway, Railway },
            { Surface.MainRoad, MainRoad },
            { Surface.Road, Road },
            { Surface.Lane, Lane },
        };

        public static Color For(Surface surface)
        {
            return BySurface[surface];
        }
    }
}
