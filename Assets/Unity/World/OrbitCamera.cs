using ShallowWater.Unity.Controls;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class OrbitCamera : MonoBehaviour
    {
        private const float LowestPitchDegrees = -5f;
        private const float HighestPitchDegrees = 70f;
        private const float NearestShare = 0.35f;
        private const float FarthestShare = 3f;
        private const float EasePerSecond = 6f;
        private const float LowestAboveTheWaterMetres = 0.6f;

        private Transform target;
        private CameraFraming framing;
        private float yawDegrees;
        private float pitchDegrees;
        private float distanceMetres;

        public Transform Target => target;
        public float YawDegrees => IsTurnedWithTheTarget ? TargetYaw + yawDegrees : yawDegrees;
        private bool IsTurnedWithTheTarget => framing.IsTurnedWithTheTarget;
        private float TargetYaw => YawOf(target);

        public void Follow(Transform newTarget, CameraFraming newFraming)
        {
            var yaw = target == null ? YawOf(newTarget) : YawDegrees;
            target = newTarget;
            framing = newFraming;
            yawDegrees = IsTurnedWithTheTarget ? yaw - TargetYaw : yaw;
            pitchDegrees = framing.PitchDegrees;
            distanceMetres = framing.DistanceMetres;
        }

        private void LateUpdate()
        {
            if (target == null) return;
            Orbit();
            var pivot = target.position + Vector3.up * framing.LookHeightMetres;
            var turn = Quaternion.Euler(pitchDegrees, YawDegrees, 0);
            var wanted = pivot - turn * Vector3.forward * distanceMetres;
            wanted.y = Mathf.Max(wanted.y, LowestAboveTheWaterMetres);
            var ease = 1 - Mathf.Exp(-EasePerSecond * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, wanted, ease);
            transform.LookAt(pivot);
        }

        private static float YawOf(Transform placed)
        {
            var angles = placed.eulerAngles;
            return angles.y;
        }

        private void Orbit()
        {
            var orbit = CameraInput.OrbitDegrees();
            yawDegrees += orbit.x;
            pitchDegrees = Mathf.Clamp(pitchDegrees - orbit.y, LowestPitchDegrees, HighestPitchDegrees);
            var nearest = framing.DistanceMetres * NearestShare;
            var farthest = framing.DistanceMetres * FarthestShare;
            distanceMetres = Mathf.Clamp(distanceMetres + CameraInput.ZoomMetres() * distanceMetres, nearest, farthest);
        }
    }
}
