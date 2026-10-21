using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Looks;
using ShallowWater.Unity.Map;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.World
{
    public sealed class RopeMesh
    {
        private readonly GameObject rope;
        private readonly Mesh mesh;

        public RopeMesh(string name, Transform parent)
        {
            rope = new GameObject(name);
            rope.transform.SetParent(parent, false);
            mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            rope.AddComponent<MeshFilter>().sharedMesh = mesh;
            rope.AddComponent<MeshRenderer>().sharedMaterial = Finishes.For(Surface.Rope);
            rope.SetActive(false);
        }

        public void Lay(Shape shape)
        {
            ShapeMeshes.Filled(mesh, shape);
            rope.SetActive(true);
        }

        public void Show(bool isShown)
        {
            rope.SetActive(isShown);
        }

        public void TakeUp()
        {
            rope.SetActive(false);
        }
    }
}
