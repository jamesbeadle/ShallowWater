using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class SlateLook
    {
        private static readonly int Moss = Shader.PropertyToID("_Moss");

        public static Material Of(Tones tones)
        {
            var material = LookShaders.Made(LookShaders.Slate);
            material.SetColor(LookProperties.Colour, tones.Main);
            material.SetColor(LookProperties.Variation, tones.Worn);
            material.SetColor(Moss, tones.Accent);
            return material;
        }
    }
}
