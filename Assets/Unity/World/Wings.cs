using ShallowWater.Game.Wildlife;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class Wings
    {
        private const string Name = "Wing";

        private readonly Transform right;
        private readonly Transform left;

        private Wings(Transform right, Transform left)
        {
            this.right = right;
            this.left = left;
        }

        public static Wings On(Transform body)
        {
            return new Wings(Hung(body, WingShapes.RightSide), Hung(body, WingShapes.LeftSide));
        }

        public void Beat(double seconds)
        {
            var raisedDegrees = (float)WingPose.RaisedAt(seconds) * Mathf.Rad2Deg;
            right.localRotation = Quaternion.Euler(0, 0, raisedDegrees);
            left.localRotation = Quaternion.Euler(0, 0, -raisedDegrees);
        }

        public void Fold()
        {
            var backDegrees = (float)WingPose.FoldedBackRadians * Mathf.Rad2Deg;
            var droopDegrees = (float)WingPose.FoldedDroopRadians * Mathf.Rad2Deg;
            right.localRotation = Quaternion.Euler(0, backDegrees, -droopDegrees);
            left.localRotation = Quaternion.Euler(0, -backDegrees, droopDegrees);
        }

        private static Transform Hung(Transform body, double side)
        {
            var wing = new GameObject(Name).transform;
            wing.SetParent(body, false);
            wing.localPosition = WorldVectors.Of(CorvidShapes.ShoulderOf(side));
            foreach (var piece in ShapeMeshes.BuildEach(Name, WingShapes.Of(side))) Gatherings.Within(wing, piece);
            return wing;
        }
    }
}
