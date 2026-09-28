using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using UnityEngine;

namespace ShallowWater.Unity.Helm
{
    public sealed class BoatController : MonoBehaviour
    {
        private const float LongestStepSeconds = 1f / 30;

        private readonly Throttle throttle = new Throttle();
        private Pound pound;

        public BoatMotion Motion { get; private set; }
        public float RudderShare => Motion == null ? 0 : (float)Motion.Rudder;
        public float ThrustShare => Motion == null ? 0 : (float)Motion.ThrustShare;
        public float SpeedShare => Motion == null ? 0 : (float)(Motion.SpeedMetresPerSecond / BoatHandling.TopSpeedMetresPerSecond);
        public float SwingRadiansPerSecond => Motion == null ? 0 : (float)Motion.SwingRadiansPerSecond;

        public void Launch(BoatMotion motion, Pound water)
        {
            Motion = motion;
            pound = water;
            Place();
        }

        private void Update()
        {
            if (Motion == null) return;
            WorkTheLever();
            var rudder = HelmInput.Rudder();
            var steps = Mathf.CeilToInt(Time.deltaTime / LongestStepSeconds);
            for (var step = 0; step < steps; step++) Steer(Time.deltaTime / steps, rudder);
            Place();
        }

        private void Steer(float seconds, double rudder)
        {
            Motion.Advance(seconds, throttle.Notch, rudder);
            Banks.Keep(Motion, pound);
        }

        private void WorkTheLever()
        {
            if (HelmInput.IsOpeningUp()) throttle.Forward();
            if (HelmInput.IsEasingOff()) throttle.Back();
            if (HelmInput.IsStopping()) throttle.Stop();
        }

        private void Place()
        {
            var position = Motion.Position;
            transform.position = new Vector3((float)position.East, (float)Heights.WaterMetres, (float)position.North);
            transform.rotation = Quaternion.Euler(0, (float)Motion.Bearing * Mathf.Rad2Deg, 0);
        }
    }
}
