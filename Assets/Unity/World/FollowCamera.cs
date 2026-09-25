using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class FollowCamera : MonoBehaviour
    {
        private const float HeightAboveWater = 9f;
        private const float DistanceAstern = 16f;
        private const float EasePerSecond = 2.5f;

        private Transform boat;

        public void Follow(Transform boatTransform)
        {
            boat = boatTransform;
        }

        private void LateUpdate()
        {
            if (boat == null) return;
            var wanted = boat.position - boat.forward * DistanceAstern + Vector3.up * HeightAboveWater;
            transform.position = Vector3.Lerp(transform.position, wanted, EasePerSecond * Time.deltaTime);
            transform.LookAt(boat.position);
        }
    }
}
