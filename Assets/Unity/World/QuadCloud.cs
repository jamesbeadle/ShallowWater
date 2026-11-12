using System.Collections.Generic;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class QuadCloud
    {
        private const int CornersPerQuad = 4;
        private const int WholeMesh = 0;
        private const int CornerChannel = 0;
        private const int IndexChannel = 1;
        private static readonly Vector2[] Corners = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        private static readonly int[] CornerOrder = { 0, 2, 1, 1, 2, 3 };

        public static Mesh Made(int count, Bounds reach)
        {
            var vertices = new List<Vector3>();
            var corners = new List<Vector2>();
            var indices = new List<Vector2>();
            var triangles = new List<int>();
            for (var quad = 0; quad < count; quad++)
            {
                var first = vertices.Count;
                foreach (var corner in Corners) vertices.Add(Vector3.zero);
                corners.AddRange(Corners);
                for (var corner = 0; corner < CornersPerQuad; corner++) indices.Add(new Vector2(quad, 0));
                foreach (var order in CornerOrder) triangles.Add(first + order);
            }
            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetUVs(CornerChannel, corners);
            mesh.SetUVs(IndexChannel, indices);
            mesh.SetTriangles(triangles, WholeMesh);
            mesh.bounds = reach;
            return mesh;
        }
    }
}
