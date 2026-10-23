using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class CameraClearance
    {
        private const float LensRadiusMetres = 0.3f;

        public static Vector3 KeptClear(Vector3 pivot, Vector3 wanted)
        {
            var towardsTheCamera = wanted - pivot;
            var distance = towardsTheCamera.magnitude;
            var isBlocked = Physics.SphereCast(pivot, LensRadiusMetres, towardsTheCamera, out var block, distance);
            return isBlocked ? pivot + towardsTheCamera.normalized * block.distance : wanted;
        }
    }
}
