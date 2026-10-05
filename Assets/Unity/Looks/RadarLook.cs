using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class RadarLook
    {
        private static readonly int Centre = Shader.PropertyToID("_Centre");
        private static readonly int Reach = Shader.PropertyToID("_Reach");
        private static readonly int Heading = Shader.PropertyToID("_Heading");
        private static readonly int Pointing = Shader.PropertyToID("_Pointing");

        public static Material Made(Texture2D map, Vector2 reach)
        {
            var material = LookShaders.Made(LookShaders.Radar);
            material.mainTexture = map;
            material.SetVector(Reach, reach);
            return material;
        }

        public static void Turn(Material face, Vector2 centre, float headingDegrees, float pointingDegrees)
        {
            face.SetVector(Centre, centre);
            face.SetFloat(Heading, headingDegrees * Mathf.Deg2Rad);
            face.SetFloat(Pointing, pointingDegrees * Mathf.Deg2Rad);
        }
    }
}
