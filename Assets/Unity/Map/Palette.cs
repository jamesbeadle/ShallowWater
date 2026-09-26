using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class Palette
    {
        public static readonly Color Water = new Color(0.16f, 0.20f, 0.17f);
        public static readonly Color Towpath = new Color(0.43f, 0.35f, 0.24f);
        public static readonly Color PavedTowpath = new Color(0.47f, 0.46f, 0.44f);
        public static readonly Color Coping = new Color(0.64f, 0.62f, 0.57f);
        public static readonly Color Bollard = new Color(0.07f, 0.07f, 0.08f);
        public static readonly Color Grass = new Color(0.32f, 0.44f, 0.19f);
        public static readonly Color Bank = new Color(0.27f, 0.37f, 0.16f);
        public static readonly Color River = new Color(0.20f, 0.29f, 0.33f);
        public static readonly Color Railway = new Color(0.27f, 0.23f, 0.21f);
        public static readonly Color MainRoad = new Color(0.21f, 0.21f, 0.22f);
        public static readonly Color Road = new Color(0.28f, 0.28f, 0.28f);
        public static readonly Color Lane = new Color(0.62f, 0.57f, 0.47f);
        public static readonly Color RoadLine = new Color(0.86f, 0.86f, 0.82f);
        public static readonly Color Kerb = new Color(0.66f, 0.65f, 0.62f);
        public static readonly Color Pavement = new Color(0.55f, 0.54f, 0.52f);
        public static readonly Color Hedge = new Color(0.22f, 0.34f, 0.13f);
        public static readonly Color GardenHedge = new Color(0.24f, 0.38f, 0.16f);
        public static readonly Color TelegraphPole = new Color(0.24f, 0.18f, 0.12f);
        public static readonly Color Wall = new Color(0.52f, 0.32f, 0.26f);
        public static readonly Color Roof = new Color(0.30f, 0.29f, 0.31f);
        public static readonly Color Trunk = new Color(0.33f, 0.26f, 0.18f);
        public static readonly Color Crown = new Color(0.27f, 0.33f, 0.14f);

        private static readonly Dictionary<Surface, Color> BySurface = new Dictionary<Surface, Color>
        {
            { Surface.Water, Water },
            { Surface.Towpath, Towpath },
            { Surface.PavedTowpath, PavedTowpath },
            { Surface.Coping, Coping },
            { Surface.Bollard, Bollard },
            { Surface.Grass, Grass },
            { Surface.Bank, Bank },
            { Surface.River, River },
            { Surface.Railway, Railway },
            { Surface.MainRoad, MainRoad },
            { Surface.Road, Road },
            { Surface.Lane, Lane },
            { Surface.RoadLine, RoadLine },
            { Surface.Kerb, Kerb },
            { Surface.Pavement, Pavement },
            { Surface.Hedge, Hedge },
            { Surface.GardenHedge, GardenHedge },
            { Surface.TelegraphPole, TelegraphPole },
            { Surface.Wall, Wall },
            { Surface.Roof, Roof },
        };

        public static Color For(Surface surface)
        {
            return BySurface[surface];
        }
    }
}
