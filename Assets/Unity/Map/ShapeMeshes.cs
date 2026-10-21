using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Looks;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.Map
{
    public static class ShapeMeshes
    {
        private const int WholeMesh = 0;
        private const int SurfacePlaceChannel = 0;
        private const int FittingChannel = 1;

        public static GameObject Build(string name, Shape shape, Material finish)
        {
            var surface = new GameObject(name);
            var filter = surface.AddComponent<MeshFilter>();
            filter.sharedMesh = MeshOf(shape);
            var renderer = surface.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = finish;
            return surface;
        }

        public static List<GameObject> BuildEach(string layerName, SurfaceShapes surfaces)
        {
            var built = surfaces.BySurface.Select(surface => Build($"{layerName} {surface.Key}", surface.Value, Finishes.For(surface.Key)));
            return built.ToList();
        }

        public static Mesh MeshOf(Shape shape)
        {
            var mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            return Filled(mesh, shape);
        }

        public static Mesh Filled(Mesh mesh, Shape shape)
        {
            mesh.Clear();
            mesh.SetVertices(Vertices(shape));
            mesh.SetUVs(SurfacePlaceChannel, Places(shape));
            mesh.SetUVs(FittingChannel, Fittings(shape));
            mesh.SetTriangles(new List<int>(shape.Triangles), WholeMesh);
            mesh.RecalculateNormals();
            mesh.SetTangents(MeshTangents.Across(mesh.normals));
            mesh.RecalculateBounds();
            return mesh;
        }

        public static Mesh SmoothMeshOf(Shape shape)
        {
            var mesh = MeshOf(shape);
            var normals = NormalWelding.Welded(mesh.vertices, mesh.normals);
            mesh.normals = normals;
            mesh.SetTangents(MeshTangents.Across(normals));
            return mesh;
        }

        private static List<Vector3> Vertices(Shape shape)
        {
            var vertices = new List<Vector3>(shape.PointCount);
            foreach (var point in shape.Points) vertices.Add(WorldVectors.Of(point));
            return vertices;
        }

        private static List<Vector4> Fittings(Shape shape)
        {
            var fittings = new List<Vector4>(shape.PointCount);
            foreach (var fitting in shape.Fittings) fittings.Add(FittingVector(fitting));
            return fittings;
        }

        private static Vector4 FittingVector(Fitting fitting)
        {
            return new Vector4((float)fitting.WidthMetres, (float)fitting.HeightMetres, fitting.Pattern, fitting.Paint);
        }

        private static List<Vector2> Places(Shape shape)
        {
            var places = new List<Vector2>(shape.PointCount);
            foreach (var place in shape.Places) places.Add(new Vector2((float)place.Along, (float)place.Across));
            return places;
        }
    }
}
