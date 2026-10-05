using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class CropLook
    {
        private static readonly int Ground = Shader.PropertyToID("_Ground");
        private static readonly int GroundWorn = Shader.PropertyToID("_GroundWorn");
        private static readonly int Growth = Shader.PropertyToID("_Growth");
        private static readonly int Accent = Shader.PropertyToID("_Accent");
        private static readonly int Pattern = Shader.PropertyToID("_Pattern");
        private static readonly int RowMetres = Shader.PropertyToID("_RowMetres");
        private static readonly int Cover = Shader.PropertyToID("_Cover");
        private static readonly int Relief = Shader.PropertyToID("_Relief");
        private static readonly int Lifted = Shader.PropertyToID("_Lifted");

        public static Material Of(CropTones tones, CropPattern pattern)
        {
            var material = LookShaders.Made(LookShaders.Crops);
            material.SetColor(Ground, tones.Ground);
            material.SetColor(GroundWorn, tones.Worn);
            material.SetColor(Growth, tones.Growth);
            material.SetColor(Accent, tones.Accent);
            material.SetFloat(Pattern, (float)pattern.Kind);
            material.SetFloat(RowMetres, pattern.RowMetres);
            material.SetFloat(Cover, pattern.Cover);
            material.SetFloat(Relief, pattern.ReliefMetres);
            material.SetFloat(Lifted, pattern.Lifted);
            return material;
        }
    }
}
