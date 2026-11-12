using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Sound
{
    public static class CrowCall
    {
        private const int BoutCount = 6;
        private const double Loudest = 0.7;
        private const int Seed = 1939;
        private const double ShortestCawSeconds = 0.26;
        private const double CawSecondsSpread = 0.16;
        private const double ShortestPauseSeconds = 0.22;
        private const double PauseSecondsSpread = 0.25;
        private const double BoutPitchSpread = 0.04;
        private const double PitchFallPerCaw = 0.97;
        private const double LoudnessFallPerCaw = 0.9;
        private static readonly float[] NoPause = new float[0];
        private static readonly int[] CawsInABout = { 1, 2, 2, 3, 3, 3, 4 };

        public static IReadOnlyList<float[]> Bouts(int samplesPerSecond)
        {
            return Enumerable.Range(0, BoutCount).Select(index => Bout(samplesPerSecond, new Random(Seed + index))).ToList();
        }

        private static float[] Bout(int samplesPerSecond, Random random)
        {
            var caws = CawsInABout[random.Next(CawsInABout.Length)];
            var pitch = 1 + (random.NextDouble() * 2 - 1) * BoutPitchSpread;
            var loudness = 1.0;
            var samples = new List<float>();
            for (var index = 0; index < caws; index++)
            {
                var isAfterAnother = samples.Any();
                var pause = isAfterAnother ? Pause(samplesPerSecond, random) : NoPause;
                var lengthSeconds = ShortestCawSeconds + random.NextDouble() * CawSecondsSpread;
                var caw = new Caw(lengthSeconds, pitch, loudness);
                samples.AddRange(pause);
                samples.AddRange(caw.Samples(samplesPerSecond, random));
                pitch *= PitchFallPerCaw;
                loudness *= LoudnessFallPerCaw;
            }
            return Loudness.Set(samples.ToArray(), Loudest);
        }

        private static float[] Pause(int samplesPerSecond, Random random)
        {
            var seconds = ShortestPauseSeconds + random.NextDouble() * PauseSecondsSpread;
            return new float[(int)(seconds * samplesPerSecond)];
        }
    }
}
