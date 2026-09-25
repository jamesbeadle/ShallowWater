using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class Paint
    {
        private static Material pipelineDefault;

        public static Material Of(Color colour)
        {
            var material = new Material(PipelineDefault());
            material.color = colour;
            return material;
        }

        private static Material PipelineDefault()
        {
            var isKnown = pipelineDefault != null;
            if (isKnown) return pipelineDefault;
            var sample = GameObject.CreatePrimitive(PrimitiveType.Quad);
            var renderer = sample.GetComponent<Renderer>();
            pipelineDefault = renderer.sharedMaterial;
            Object.Destroy(sample);
            return pipelineDefault;
        }
    }
}
