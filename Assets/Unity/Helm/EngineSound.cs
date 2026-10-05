using ShallowWater.Game.Sound;
using UnityEngine;

namespace ShallowWater.Unity.Helm
{
    public sealed class EngineSound : MonoBehaviour
    {
        private const int SamplesPerSecond = 22050;
        private const int Mono = 1;
        private const float TickoverBeatsPerSecond = 1f;
        private const float FullBeatsPerSecond = 2.4f;
        private const float QuietestBeat = 0.6f;
        private const float WaterVolume = 0.4f;
        private const float HeardFromMetres = 6f;
        private const float HeardToMetres = 250f;
        private const float AllAround = 1f;

        private BoatController helm;
        private AudioSource beats;
        private AudioSource water;
        private AudioClip beat;
        private float untilTheNextBeat;

        private void Start()
        {
            helm = GetComponent<BoatController>();
            beat = Clip("Bolinder beat", BolinderBeat.Samples(SamplesPerSecond));
            beats = Source();
            water = Source();
            water.clip = Clip("Water at the bow", WaterHush.Samples(SamplesPerSecond));
            water.loop = true;
            water.Play();
        }

        private void Update()
        {
            var work = Mathf.Abs(helm.EngineTurns);
            water.volume = WaterVolume * Mathf.Abs(helm.SpeedShare);
            untilTheNextBeat -= Time.deltaTime;
            if (untilTheNextBeat > 0) return;
            beats.PlayOneShot(beat, Mathf.Lerp(QuietestBeat, 1, work));
            untilTheNextBeat += 1 / Mathf.Lerp(TickoverBeatsPerSecond, FullBeatsPerSecond, work);
        }

        private AudioSource Source()
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.spatialBlend = AllAround;
            source.minDistance = HeardFromMetres;
            source.maxDistance = HeardToMetres;
            source.playOnAwake = false;
            return source;
        }

        private static AudioClip Clip(string name, float[] samples)
        {
            var clip = AudioClip.Create(name, samples.Length, Mono, SamplesPerSecond, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
