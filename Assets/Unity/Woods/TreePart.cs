using ShallowWater.Game.Ground;
using ShallowWater.Game.Woods;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public sealed class TreePart
    {
        private const int WholeMesh = 0;

        private readonly Mesh mesh;
        private readonly Material material;
        private readonly Vector3 centre;
        private readonly Vector3 size;

        private TreePart(PrimitiveType shape, Color colour, Vector3 centre, Vector3 size)
        {
            mesh = Primitives.MeshOf(shape);
            material = Paint.InstancedOf(colour);
            this.centre = centre;
            this.size = size;
        }

        public static TreePart Trunk()
        {
            var height = (float)TreeForm.TrunkHeightMetres;
            var width = (float)TreeForm.TrunkWidthMetres;
            var centre = new Vector3(0, (float)Heights.GroundMetres + height / 2, 0);
            return new TreePart(PrimitiveType.Cylinder, Palette.Trunk, centre, new Vector3(width, height / Primitives.CylinderHeightUnits, width));
        }

        public static TreePart Crown()
        {
            var height = (float)TreeForm.CrownHeightMetres;
            var width = (float)TreeForm.CrownWidthMetres;
            var centre = new Vector3(0, (float)(Heights.GroundMetres + TreeForm.CrownBaseMetres) + height / 2, 0);
            return new TreePart(PrimitiveType.Sphere, Palette.Crown, centre, new Vector3(width, height, width));
        }

        public Matrix4x4 PlacedAt(Tree tree)
        {
            var scale = (float)tree.Scale;
            var ground = new Vector3((float)tree.East, 0, (float)tree.North);
            return Matrix4x4.TRS(ground + centre * scale, Quaternion.identity, size * scale);
        }

        public void Draw(Matrix4x4[] placements, Bounds bounds)
        {
            var parameters = new RenderParams(material) { worldBounds = bounds };
            Graphics.RenderMeshInstanced(parameters, mesh, WholeMesh, placements);
        }
    }
}
