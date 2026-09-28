using ShallowWater.Game.Boat;
using ShallowWater.Unity.Helm;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class WakeTrail : MonoBehaviour
    {
        private const int Samples = 24;
        private const float SampleEverySeconds = 0.6f;
        private const float Unused = 0;
        private const int Newest = 0;
        private const int NoSamples = 0;
        private static readonly int PathProperty = Shader.PropertyToID("_WakePath");
        private static readonly int ThrustProperty = Shader.PropertyToID("_WakeThrust");
        private static readonly int SamplesProperty = Shader.PropertyToID("_WakeSamples");

        private readonly Vector4[] path = new Vector4[Samples];
        private readonly Vector4[] thrusts = new Vector4[Samples];
        private readonly Vector4[] aged = new Vector4[Samples];
        private BoatController helm;
        private float sinceTheLastSample;
        private int count;

        private void Start()
        {
            helm = GetComponent<BoatController>();
        }

        private void LateUpdate()
        {
            sinceTheLastSample += Time.deltaTime;
            var isDueASample = sinceTheLastSample >= SampleEverySeconds || count == NoSamples;
            if (isDueASample) KeepTheNewest();
            var stern = transform.position - transform.forward * (float)BoatSize.HalfLengthMetres;
            var speed = Mathf.Abs(helm.SpeedShare) * (float)BoatHandling.TopSpeedMetresPerSecond;
            path[Newest] = new Vector4(stern.x, stern.z, Time.time, speed);
            thrusts[Newest] = new Vector4(helm.ThrustShare, Unused, Unused, Unused);
            for (var sample = 0; sample < Samples; sample++) aged[sample] = Aged(path[sample]);
            Shader.SetGlobalVectorArray(PathProperty, aged);
            Shader.SetGlobalVectorArray(ThrustProperty, thrusts);
            Shader.SetGlobalFloat(SamplesProperty, count);
        }

        private void KeepTheNewest()
        {
            for (var sample = Samples - 1; sample > Newest; sample--)
            {
                path[sample] = path[sample - 1];
                thrusts[sample] = thrusts[sample - 1];
            }
            count = Mathf.Min(count + 1, Samples);
            sinceTheLastSample = 0;
        }

        private static Vector4 Aged(Vector4 sample)
        {
            return new Vector4(sample.x, sample.y, Time.time - sample.z, sample.w);
        }
    }
}
