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
        private Vector3 Pivot => target.position + Vector3.up * framing.LookHeightMetres;
        private Quaternion Facing => Quaternion.Euler(pitchDegrees, YawDegrees, 0);
        private Vector3 Place => Pivot - Facing * Vector3.forward * distanceMetres;

        public void Follow(Transform newTarget, CameraFraming newFraming)
        {
            var yaw = target == null ? YawOf(newTarget) : YawDegrees;
            var isCuttingFromTheEye = framing.IsAtTheEye;
            target = newTarget;
            framing = newFraming;
            yawDegrees = IsTurnedWithTheTarget ? yaw - TargetYaw : yaw;
            pitchDegrees = framing.PitchDegrees;
            distanceMetres = framing.DistanceMetres;
            if (isCuttingFromTheEye) transform.position = Place;
            var looking = framing.Looking;
            MousePointer.Lock(looking.IsOnTheMouseAlone);
        }

        private void Update()
        {
            if (target == null) return;
            Turn();
        }

        private void LateUpdate()
        {
            if (target == null) return;
            if (framing.IsAtTheEye) Sight();
            if (!framing.IsAtTheEye) Orbit();
        }

        private static float YawOf(Transform placed)
        {
            var angles = placed.eulerAngles;
            return angles.y;
        }

        private void Turn()
        {
            var looking = framing.Looking;
            var turn = CameraInput.OrbitDegrees(looking.IsOnTheMouseAlone) * looking.TurnShare;
            yawDegrees += turn.x;
            pitchDegrees = Mathf.Clamp(pitchDegrees - turn.y, looking.LowestPitchDegrees, looking.HighestPitchDegrees);
            if (framing.IsAtTheEye) return;
            Zoom();
        }

        private void Zoom()
        {
            var nearest = framing.DistanceMetres * NearestShare;
            var farthest = framing.DistanceMetres * FarthestShare;
            distanceMetres = Mathf.Clamp(distanceMetres + CameraInput.ZoomMetres() * distanceMetres, nearest, farthest);
        }

        private void Sight()
        {
            transform.SetPositionAndRotation(Place, Facing);
        }

        private void Orbit()
        {
            var pivot = Pivot;
            var place = Place;
            var wanted = CameraClearance.KeptClear(pivot, place);
            wanted.y = Mathf.Max(wanted.y, LowestAboveTheWaterMetres);
            var isPulledIn = wanted != place;
            var ease = isPulledIn ? AtOnce : 1 - Mathf.Exp(-EasePerSecond * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, wanted, ease);
            transform.LookAt(pivot);
        }
    }
}
