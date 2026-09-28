using UnityEngine;

namespace ShallowWater.Unity.Sky
{
    public static class ShaderColours
    {
        public static void SetGlobal(int property, Color colour)
        {
            var isLinear = QualitySettings.activeColorSpace == ColorSpace.Linear;
            var asTheShaderSeesIt = isLinear ? colour.linear : colour;
            Shader.SetGlobalVector(property, asTheShaderSeesIt);
        }
    }
}
