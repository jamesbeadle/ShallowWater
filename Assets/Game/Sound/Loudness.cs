using System;

namespace ShallowWater.Game.Sound
{
    public static class Loudness
    {
        private const float Silence = 0;

        public static float[] Set(float[] samples, double loudest)
        {
            var peak = Peak(samples);
            if (peak == Silence) return samples;
            var gain = (float)(loudest / peak);
            for (var index = 0; index < samples.Length; index++) samples[index] *= gain;
            return samples;
        }

        private static float Peak(float[] samples)
        {
            var peak = Silence;
            foreach (var sample in samples) peak = Math.Max(peak, Math.Abs(sample));
            return peak;
        }
    }
}
