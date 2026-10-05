using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public sealed class SkyColours
    {
        public Color Zenith { get; set; }
        public Color Horizon { get; set; }
        public Color Haze { get; set; }
        public Color Glow { get; set; }
        public Color CloudLight { get; set; }
        public Color CloudShade { get; set; }
        public Color Light { get; set; }
        public Color SkyAbove { get; set; }
        public Color LightAround { get; set; }
        public Color GroundBelow { get; set; }

        public static SkyColours Between(SkyColours earlier, SkyColours later, float share)
        {
            return new SkyColours
            {
                Zenith = Color.Lerp(earlier.Zenith, later.Zenith, share),
                Horizon = Color.Lerp(earlier.Horizon, later.Horizon, share),
                Haze = Color.Lerp(earlier.Haze, later.Haze, share),
                Glow = Color.Lerp(earlier.Glow, later.Glow, share),
                CloudLight = Color.Lerp(earlier.CloudLight, later.CloudLight, share),
                CloudShade = Color.Lerp(earlier.CloudShade, later.CloudShade, share),
                Light = Color.Lerp(earlier.Light, later.Light, share),
                SkyAbove = Color.Lerp(earlier.SkyAbove, later.SkyAbove, share),
                LightAround = Color.Lerp(earlier.LightAround, later.LightAround, share),
                GroundBelow = Color.Lerp(earlier.GroundBelow, later.GroundBelow, share)
            };
        }
    }
}
