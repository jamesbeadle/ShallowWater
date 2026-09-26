using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.Map
{
    public static class ShapeMeshes
    {
        private const int WholeMesh = 0;

        public static GameObject Build(string name, Shape shape, Color colour)
        {
            var surface = new GameObject(name);
            var filter = surface.AddComponent<MeshFilter>();
            filter.sharedMesh = MeshOf(shape);
            var renderer = surface.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = Paint.Of(colour);
            return surface;
        }

        public static void BuildEach(string layerName, SurfaceShapes surfaces)
        {
            foreach (var surface in surfaces.BySurface)
            {
                Build($"{layerName} {surface.Key}", surface.Value, Palette.For(surface.Key));
            }
        }

        public static Mesh MeshOf(Shape shape)
        {
            var mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.SetVertices(Vertices(shape));
            mesh.SetTriangles(new List<int>(shape.Triangles), WholeMesh);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static List<Vector3> Vertices(Shape shape)
        {
            var vertices = new List<Vector3>(shape.PointCount);
            foreach (var point in shape.Points) vertices.Add(WorldVectors.Of(point));
            return vertices;
        }
    }
}
