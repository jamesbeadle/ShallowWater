using ShallowWater.Unity.Controls;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    [DefaultExecutionOrder(BeforeEveryFollower)]
    public sealed class OrbitCamera : MonoBehaviour
    {
        private const int BeforeEveryFollower = -100;
        private const float NearestShare = 0.35f;
        private const float FarthestShare = 3f;
        private const float EasePerSecond = 6f;
        private const float LowestAboveTheWaterMetres = 0.6f;
        private const float AtOnce = 1f;

        private Transform target;
        private CameraFraming framing;
        private float yawDegrees;
        private float pitchDegrees;
        private float distanceMetres;

        public Transform Target => target;
        public float PitchDegrees => pitchDegrees;
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

        private void Update()
        {
            if (target == null) return;
            Orbit();
        }

        private void LateUpdate()
        {
            if (target == null) return;
            var pivot = target.position + Vector3.up * framing.LookHeightMetres;
            var turn = Quaternion.Euler(pitchDegrees, YawDegrees, 0);
            var orbiting = pivot - turn * Vector3.forward * distanceMetres;
            var wanted = CameraClearance.KeptClear(pivot, orbiting);
            wanted.y = Mathf.Max(wanted.y, LowestAboveTheWaterMetres);
            var isPulledIn = wanted != orbiting;
            var ease = isPulledIn ? AtOnce : 1 - Mathf.Exp(-EasePerSecond * Time.deltaTime);
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
            var looking = framing.Looking;
            var orbit = CameraInput.OrbitDegrees() * looking.TurnShare;
            yawDegrees += orbit.x;
            pitchDegrees = Mathf.Clamp(pitchDegrees - orbit.y, looking.LowestPitchDegrees, looking.HighestPitchDegrees);
            var nearest = framing.DistanceMetres * NearestShare;
            var farthest = framing.DistanceMetres * FarthestShare;
            distanceMetres = Mathf.Clamp(distanceMetres + CameraInput.ZoomMetres() * distanceMetres, nearest, farthest);
        }
    }
}
