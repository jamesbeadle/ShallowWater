using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.World
{
    public static class Plume
    {
        private const string PlumeName = "Smoke";
        private const int CornersPerPuff = 4;
        private const int WholeMesh = 0;
        private const int CornerChannel = 0;
        private const int PuffChannel = 1;
        private static readonly Vector2[] Corners = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        private static readonly int[] CornerOrder = { 0, 2, 1, 1, 2, 3 };
        private static readonly Bounds Reach = new Bounds(Vector3.up * 4f, Vector3.one * 16f);

        public static void Rising(Transform parent, Vector3 localPlace, Material look, int puffs)
        {
            var plume = new GameObject(PlumeName);
            var placement = plume.transform;
            placement.SetParent(parent, false);
            placement.localPosition = localPlace;
            var filter = plume.AddComponent<MeshFilter>();
            filter.sharedMesh = Puffs(puffs);
            var renderer = plume.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = look;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        private static Mesh Puffs(int count)
        {
            var vertices = new List<Vector3>();
            var corners = new List<Vector2>();
            var indices = new List<Vector2>();
            var triangles = new List<int>();
            for (var puff = 0; puff < count; puff++)
            {
                var first = vertices.Count;
                foreach (var corner in Corners) vertices.Add(Vector3.zero);
                corners.AddRange(Corners);
                for (var corner = 0; corner < CornersPerPuff; corner++) indices.Add(new Vector2(puff, 0));
                foreach (var order in CornerOrder) triangles.Add(first + order);
            }
            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetUVs(CornerChannel, corners);
            mesh.SetUVs(PuffChannel, indices);
            mesh.SetTriangles(triangles, WholeMesh);
            mesh.bounds = Reach;
            return mesh;
        }
    }
}
