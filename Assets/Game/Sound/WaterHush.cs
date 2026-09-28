using System;

namespace ShallowWater.Game.Sound
{
    public static class WaterHush
    {
        private const double LengthSeconds = 3;
        private const double JoinSeconds = 0.5;
        private const double Smoothing = 0.12;
        private const double SlowSwell = 0.004;
        private const double Loudest = 0.5;
        private const int Seed = 1938;

        public static float[] Samples(int samplesPerSecond)
        {
            var count = (int)(LengthSeconds * samplesPerSecond);
            var join = (int)(JoinSeconds * samplesPerSecond);
            var noise = Noise(count + join);
            var samples = new float[count];
            for (var index = 0; index < count; index++) samples[index] = (float)(noise[index] * Loudest);
            for (var index = 0; index < join; index++)
            {
                var share = (double)index / join;
                samples[index] = (float)((noise[count + index] * (1 - share) + noise[index] * share) * Loudest);
            }
            return samples;
        }

        private static double[] Noise(int count)
        {
            var random = new Random(Seed);
            var noise = new double[count];
            var smooth = 0.0;
            var swell = 0.0;
            for (var index = 0; index < count; index++)
            {
                smooth += (random.NextDouble() * 2 - 1 - smooth) * Smoothing;
                swell += (random.NextDouble() - swell) * SlowSwell;
                noise[index] = smooth * swell * 2;
            }
            return noise;
        }
    }
}
