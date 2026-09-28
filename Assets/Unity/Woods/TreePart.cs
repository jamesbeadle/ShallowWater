using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Woods;
using ShallowWater.Unity.Looks;
using ShallowWater.Unity.Map;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.Woods
{
    public sealed class TreePart
    {
        private const int WholeMesh = 0;
        private const float Unstretched = 1f;
        private const float AtTheFoot = 0f;
        private const float AtTheMiddle = 0.5f;

        private readonly Mesh mesh;
        private readonly Material material;
        private readonly Vector3 size;
        private readonly float baseMetres;
        private readonly float originShareOfTheHeight;
        private readonly bool isStretchedBySlenderness;

        private TreePart(Shape shape, Surface surface, Vector3 size, float baseMetres, float originShareOfTheHeight, bool isStretchedBySlenderness)
        {
            mesh = ShapeMeshes.MeshOf(shape);
            material = Finishes.For(surface);
            this.size = size;
            this.baseMetres = baseMetres;
            this.originShareOfTheHeight = originShareOfTheHeight;
            this.isStretchedBySlenderness = isStretchedBySlenderness;
        }

        public static TreePart Trunk()
        {
            var height = (float)TreeForm.TrunkHeightMetres;
            var width = (float)TreeForm.TrunkWidthMetres;
            return new TreePart(TrunkShape.Tapered(), Surface.Bark, new Vector3(width, height, width), AtTheFoot, AtTheFoot, false);
        }

        public static TreePart Crown()
        {
            var height = (float)TreeForm.CrownHeightMetres;
            var width = (float)TreeForm.CrownWidthMetres;
            var crownBase = (float)TreeForm.CrownBaseMetres;
            return new TreePart(CrownShape.Lumpy(), Surface.Crown, new Vector3(width, height, width), crownBase, AtTheMiddle, true);
        }

        public Matrix4x4 PlacedAt(Tree tree)
        {
            var scale = (float)tree.Scale;
            var stretch = isStretchedBySlenderness ? (float)tree.Slenderness : Unstretched;
            var stretched = new Vector3(size.x, size.y * stretch, size.z) * scale;
            var lift = (float)Heights.GroundMetres + (baseMetres * scale + stretched.y * originShareOfTheHeight);
            var origin = new Vector3((float)tree.East, lift, (float)tree.North);
            var turn = Quaternion.Euler(0, (float)tree.TurnRadians * Mathf.Rad2Deg, 0);
            return Matrix4x4.TRS(origin, turn, stretched);
        }

        public void Draw(Matrix4x4[] placements, Bounds bounds)
        {
            var parameters = new RenderParams(material)
            {
                worldBounds = bounds,
                shadowCastingMode = ShadowCastingMode.On,
                receiveShadows = true
            };
            Graphics.RenderMeshInstanced(parameters, mesh, WholeMesh, placements);
        }
    }
}
