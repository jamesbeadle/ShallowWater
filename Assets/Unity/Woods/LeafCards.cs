using System.Collections.Generic;
using ShallowWater.Game.Woods;
using ShallowWater.Unity.Map;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.Woods
{
    public sealed class LeafCards
    {
        private const int WholeMesh = 0;
        private const int CornerChannel = 0;
        private const int CardChannel = 1;
        private const int Seed = 1938;
        private const float FullTurnRadians = 2 * Mathf.PI;
        private static readonly Vector2[] Corners = { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(1, 1), new Vector2(-1, 1) };
        private static readonly int[] TwoTriangles = { 0, 2, 1, 0, 3, 2 };

        private readonly List<Vector3> vertices = new List<Vector3>();
        private readonly List<Vector3> normals = new List<Vector3>();
        private readonly List<Vector2> corners = new List<Vector2>();
        private readonly List<Vector4> cards = new List<Vector4>();
        private readonly List<int> triangles = new List<int>();

        public static Mesh MeshOf(IReadOnlyList<LeafClump> leaves)
        {
            var random = new System.Random(Seed);
            var laid = new LeafCards();
            foreach (var clump in leaves) laid.Add(clump, (float)random.NextDouble() * FullTurnRadians);
            return laid.Built();
        }

        private void Add(LeafClump clump, float spin)
        {
            foreach (var corner in TwoTriangles) triangles.Add(vertices.Count + corner);
            var card = new Vector4((float)clump.SizeMetres, (float)clump.Seed, (float)clump.Depth, spin);
            var centre = WorldVectors.Of(clump.Centre);
            var outward = WorldVectors.Of(clump.Outward);
            foreach (var corner in Corners)
            {
                vertices.Add(centre);
                normals.Add(outward);
                corners.Add(corner);
                cards.Add(card);
            }
        }

        private Mesh Built()
        {
            var mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(CornerChannel, corners);
            mesh.SetUVs(CardChannel, cards);
            mesh.SetTriangles(triangles, WholeMesh);
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
