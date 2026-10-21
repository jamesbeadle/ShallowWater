using UnityEngine;

namespace ShallowWater.Unity.Sound
{
    public static class Clips
    {
        public const int SamplesPerSecond = 22050;
        private const int Mono = 1;
        private const int FromTheStart = 0;

        public static AudioClip Of(string name, float[] samples)
        {
            var clip = AudioClip.Create(name, samples.Length, Mono, SamplesPerSecond, false);
            clip.SetData(samples, FromTheStart);
            return clip;
        }
    }
}
