using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class FollowCamera : MonoBehaviour
    {
        private const float HeightAboveWater = 4.5f;
        private const float DistanceAstern = 15f;
        private const float LookAheadMetres = 8f;
        private const float LookAboveTheWaterMetres = 1.5f;
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
            var aimedAt = boat.position + boat.forward * LookAheadMetres + Vector3.up * LookAboveTheWaterMetres;
            transform.LookAt(aimedAt);
        }
    }
}
