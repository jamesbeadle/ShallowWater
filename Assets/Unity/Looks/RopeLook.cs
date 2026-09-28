using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class RopeLook
    {
        private const float LaidTwists = 30f;
        private const float LaidStrands = 30f;
        private const float StrandDepthMetres = 0.002f;
        private const float HairTwists = 1.5f;
        private const float HairStrands = 400f;
        private const float HairDepthMetres = 0.001f;
        private const float Matt = 0.1f;
        private const float Tarred = 0.25f;
        private static readonly int Dark = Shader.PropertyToID("_Dark");
        private static readonly int Twist = Shader.PropertyToID("_Twist");
        private static readonly int Strands = Shader.PropertyToID("_Strands");
        private static readonly int StrandDepth = Shader.PropertyToID("_StrandDepth");

        public static Material Cotton()
        {
            return Laid(BoatPalette.CottonRope, BoatPalette.CottonRopeShade, LaidTwists, LaidStrands, StrandDepthMetres, Matt);
        }

        public static Material TarredHemp()
        {
            return Laid(BoatPalette.TarredRope, BoatPalette.TarredRopeShade, LaidTwists, LaidStrands, StrandDepthMetres, Tarred);
        }

        public static Material HorseHair()
        {
            return Laid(BoatPalette.HorseHair, BoatPalette.HorseHairShade, HairTwists, HairStrands, HairDepthMetres, Matt);
        }

        private static Material Laid(Color colour, Color dark, float twists, float strands, float depthMetres, float smoothness)
        {
            var material = LookShaders.Made(LookShaders.Rope);
            material.SetColor(LookProperties.Colour, colour);
            material.SetColor(Dark, dark);
            material.SetFloat(Twist, twists);
            material.SetFloat(Strands, strands);
            material.SetFloat(StrandDepth, depthMetres);
            material.SetFloat(LookProperties.Smoothness, smoothness);
            return material;
        }
    }
}
