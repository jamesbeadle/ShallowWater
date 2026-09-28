using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class Bobbing : MonoBehaviour
    {
        private const float RollDegrees = 0.5f;
        private const float RollPeriodSeconds = 4.3f;
        private const float PitchDegrees = 0.25f;
        private const float PitchPeriodSeconds = 6.1f;
        private const float HeaveMetres = 0.015f;
        private const float HeavePeriodSeconds = 3.7f;
        private const float FullTurnRadians = 2 * Mathf.PI;
        private const float Level = 0;

        private void Update()
        {
            var time = Time.time;
            var roll = Swing(time, RollPeriodSeconds) * RollDegrees;
            var pitch = Swing(time, PitchPeriodSeconds) * PitchDegrees;
            var heave = Swing(time, HeavePeriodSeconds) * HeaveMetres;
            transform.localRotation = Quaternion.Euler(pitch, Level, roll);
            transform.localPosition = Vector3.up * heave;
        }

        private static float Swing(float time, float periodSeconds)
        {
            return Mathf.Sin(time * FullTurnRadians / periodSeconds);
        }
    }
}
