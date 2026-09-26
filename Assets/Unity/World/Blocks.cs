using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class Blocks
    {
        public static GameObject Place(PrimitiveType shape, string name, Material paint, Placement placement)
        {
            var block = GameObject.CreatePrimitive(shape);
            block.name = name;
            placement.Apply(block.transform);
            var renderer = block.GetComponent<Renderer>();
            renderer.sharedMaterial = paint;
            return block;
        }
    }
}
