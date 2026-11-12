using System.Collections.Generic;
using ShallowWater.Game.Day;
using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class SkyPalette
    {
        private static readonly SkyColours Day = new SkyColours
        {
            Zenith = new Color(0.4f, 0.42f, 0.44f),
            Horizon = new Color(0.56f, 0.55f, 0.52f),
            Haze = new Color(0.52f, 0.51f, 0.48f),
            Glow = new Color(0.62f, 0.52f, 0.4f),
            CloudLight = new Color(0.66f, 0.65f, 0.63f),
            CloudShade = new Color(0.3f, 0.31f, 0.33f),
            Light = new Color(0.86f, 0.82f, 0.74f),
            SkyAbove = new Color(0.4f, 0.42f, 0.45f),
            LightAround = new Color(0.38f, 0.37f, 0.34f),
            GroundBelow = new Color(0.15f, 0.14f, 0.11f)
        };

        private static readonly SkyColours Twilight = new SkyColours
        {
            Zenith = new Color(0.17f, 0.15f, 0.22f),
            Horizon = new Color(0.86f, 0.34f, 0.14f),
            Haze = new Color(0.46f, 0.24f, 0.16f),
            Glow = new Color(1f, 0.4f, 0.12f),
            CloudLight = new Color(0.78f, 0.32f, 0.16f),
            CloudShade = new Color(0.2f, 0.13f, 0.15f),
            Light = new Color(1f, 0.48f, 0.24f),
            SkyAbove = new Color(0.24f, 0.2f, 0.26f),
            LightAround = new Color(0.36f, 0.24f, 0.2f),
            GroundBelow = new Color(0.1f, 0.08f, 0.07f)
        };

        private static readonly SkyColours Night = new SkyColours
        {
            Zenith = new Color(0.01f, 0.015f, 0.04f),
            Horizon = new Color(0.04f, 0.06f, 0.11f),
            Haze = new Color(0.05f, 0.06f, 0.09f),
            Glow = new Color(0.12f, 0.13f, 0.16f),
            CloudLight = new Color(0.1f, 0.11f, 0.15f),
            CloudShade = new Color(0.03f, 0.035f, 0.05f),
            Light = new Color(0.55f, 0.65f, 0.9f),
            SkyAbove = new Color(0.06f, 0.08f, 0.14f),
            LightAround = new Color(0.05f, 0.06f, 0.09f),
            GroundBelow = new Color(0.02f, 0.02f, 0.03f)
        };

        private static readonly Dictionary<DayStage, SkyColours> ByStage = new Dictionary<DayStage, SkyColours>
        {
            { DayStage.Night, Night },
            { DayStage.Twilight, Twilight },
            { DayStage.Day, Day }
        };

        public static SkyColours At(StageBlend stage)
        {
            var earlier = ByStage[stage.Earlier];
            var later = ByStage[stage.Later];
            return SkyColours.Between(earlier, later, (float)stage.Share);
        }
    }
}
