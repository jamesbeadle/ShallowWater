using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class Paint
    {
        public static Material Of(Color colour)
        {
            var material = new Material(Primitives.PipelineDefault());
            material.color = colour;
            return material;
        }

        public static Material InstancedOf(Color colour)
        {
            var material = Of(colour);
            material.enableInstancing = true;
            return material;
        }
    }
}
