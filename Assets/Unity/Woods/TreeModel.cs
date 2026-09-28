using ShallowWater.Game.Woods;
using ShallowWater.Unity.Looks;
using ShallowWater.Unity.Map;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.Woods
{
    public sealed class TreeModel
    {
        private readonly Mesh wood;
        private readonly Mesh leaves;
        private readonly Material bark;
        private readonly Material foliage;

        public TreeModel(TreeFigure figure, TreeSpecies species)
        {
            wood = ShapeMeshes.SmoothMeshOf(figure.Wood);
            leaves = LeafCards.MeshOf(figure.Leaves);
            bark = Finishes.For(TreeSurfaces.BarkOf(species));
            foliage = Finishes.For(TreeSurfaces.LeavesOf(species));
        }

        public void Draw(TreePlacements placements, Bounds bounds, Camera camera)
        {
            var shadows = placements.IsCastingShadows ? ShadowCastingMode.On : ShadowCastingMode.Off;
            InstancedDraws.Draw(Parameters(bark, bounds, camera, shadows), wood, placements.Matrices);
            InstancedDraws.Draw(Parameters(foliage, bounds, camera, shadows), leaves, placements.Matrices);
        }

        private static RenderParams Parameters(Material material, Bounds bounds, Camera camera, ShadowCastingMode shadows)
        {
            return new RenderParams(material)
            {
                worldBounds = bounds,
                camera = camera,
                shadowCastingMode = shadows,
                receiveShadows = true
            };
        }
    }
}
