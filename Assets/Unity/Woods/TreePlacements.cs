using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public sealed class TreePlacements
    {
        public TreePlacements(TreeModel model, Matrix4x4[] matrices, bool isCastingShadows)
        {
            Model = model;
            Matrices = matrices;
            IsCastingShadows = isCastingShadows;
        }

        public TreeModel Model { get; }
        public Matrix4x4[] Matrices { get; }
        public bool IsCastingShadows { get; }

        public void Draw(Bounds bounds, Camera camera)
        {
            Model.Draw(this, bounds, camera);
        }
    }
}
