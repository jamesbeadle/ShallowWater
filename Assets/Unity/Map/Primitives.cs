using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class Primitives
    {
        private static Material pipelineDefault;

        public static Mesh MeshOf(PrimitiveType type)
        {
            var sample = GameObject.CreatePrimitive(type);
            var filter = sample.GetComponent<MeshFilter>();
            var mesh = filter.sharedMesh;
            Object.Destroy(sample);
            return mesh;
        }

        public static Material PipelineDefault()
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
