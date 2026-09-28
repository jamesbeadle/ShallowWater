using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public static class InstancedDraws
    {
        private const int MostPerDraw = 1023;
        private const int WholeMesh = 0;

        public static void Draw(RenderParams parameters, Mesh mesh, Matrix4x4[] placements)
        {
            for (var first = 0; first < placements.Length; first += MostPerDraw)
            {
                var count = Mathf.Min(MostPerDraw, placements.Length - first);
                Graphics.RenderMeshInstanced(parameters, mesh, WholeMesh, placements, count, first);
            }
        }
    }
}
