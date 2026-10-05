using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class SlateLook
    {
        private static readonly int Moss = Shader.PropertyToID("_Moss");
        private static readonly int MossAmount = Shader.PropertyToID("_MossAmount");
        private static readonly int UnitWidth = Shader.PropertyToID("_UnitWidth");
        private static readonly int Gauge = Shader.PropertyToID("_Gauge");
        private static readonly int Lap = Shader.PropertyToID("_Lap");

        public static Material Of(Tones tones, RoofCourses courses)
        {
            var material = LookShaders.Made(LookShaders.Slate);
            material.SetColor(LookProperties.Colour, tones.Main);
            material.SetColor(LookProperties.Variation, tones.Worn);
            material.SetColor(Moss, tones.Accent);
            material.SetFloat(MossAmount, courses.Moss);
            material.SetFloat(UnitWidth, courses.UnitWidthMetres);
            material.SetFloat(Gauge, courses.GaugeMetres);
            material.SetFloat(Lap, courses.LapMetres);
            material.SetFloat(LookProperties.Smoothness, courses.Smoothness);
            return material;
        }
    }
}
