using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Sound
{
    public static class CrowCall
    {
        private const double BetweenCawsSeconds = 0.16;
        private const double Loudest = 0.6;
        private const int Seed = 1939;
        private static readonly Caw[] Bout =
        {
            new Caw(lengthSeconds: 0.34, pitch: 1, loudness: 1),
            new Caw(lengthSeconds: 0.31, pitch: 0.95, loudness: 0.9),
            new Caw(lengthSeconds: 0.27, pitch: 0.9, loudness: 0.78),
        };

        public static float[] Samples(int samplesPerSecond)
        {
            var random = new Random(Seed);
            var pause = new float[(int)(BetweenCawsSeconds * samplesPerSecond)];
            var samples = new List<float>();
            foreach (var caw in Bout)
            {
                var isAfterAnother = samples.Any();
                if (isAfterAnother) samples.AddRange(pause);
                samples.AddRange(caw.Samples(samplesPerSecond, random));
            }
            return Loudness.Set(samples.ToArray(), Loudest);
        }
    }
}
