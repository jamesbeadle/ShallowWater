using ShallowWater.Unity.Helm;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class Riding : MonoBehaviour
    {
        private const float RollDegrees = 0.5f;
        private const float RollPeriodSeconds = 4.3f;
        private const float PitchDegrees = 0.25f;
        private const float PitchPeriodSeconds = 6.1f;
        private const float HeaveMetres = 0.015f;
        private const float HeavePeriodSeconds = 3.7f;
        private const float SquatDegrees = 1.6f;
        private const float SurgeDegrees = 0.5f;
        private const float HeelDegreesPerSwing = 12f;
        private const float EasePerSecond = 1.2f;
        private const float FullTurnRadians = 2 * Mathf.PI;
        private const float Level = 0;

        private BoatController helm;
        private float squat;
        private float heel;

        public void Under(BoatController boatHelm)
        {
            helm = boatHelm;
        }

        private void Update()
        {
            var time = Time.time;
            EaseTowardsTheHelm();
            var roll = Swing(time, RollPeriodSeconds) * RollDegrees + heel;
            var pitch = Swing(time, PitchPeriodSeconds) * PitchDegrees - squat;
            var heave = Swing(time, HeavePeriodSeconds) * HeaveMetres;
            transform.localRotation = Quaternion.Euler(pitch, Level, roll);
            transform.localPosition = Vector3.up * heave;
        }

        private void EaseTowardsTheHelm()
        {
            if (helm == null) return;
            var ease = EasePerSecond * Time.deltaTime;
            var speed = helm.SpeedShare;
            var wantedSquat = speed * Mathf.Abs(speed) * SquatDegrees + helm.ThrustShare * SurgeDegrees;
            squat = Mathf.Lerp(squat, wantedSquat, ease);
            heel = Mathf.Lerp(heel, helm.SwingRadiansPerSecond * speed * HeelDegreesPerSwing, ease);
        }

        private static float Swing(float time, float periodSeconds)
        {
            return Mathf.Sin(time * FullTurnRadians / periodSeconds);
        }
    }
}
