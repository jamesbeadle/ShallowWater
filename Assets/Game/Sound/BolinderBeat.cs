using System;

namespace ShallowWater.Game.Sound
{
    public static class BolinderBeat
    {
        private const double LengthSeconds = 0.8;
        private const double AttackSeconds = 0.004;
        private const double ThumpHertz = 44;
        private const double ThumpDecayPerSecond = 7;
        private const double KnockHertz = 88;
        private const double KnockDecayPerSecond = 15;
        private const double KnockShare = 0.35;
        private const double ClankHertz = 610;
        private const double ClankDecayPerSecond = 45;
        private const double ClankShare = 0.07;
        private const double PuffDecayPerSecond = 11;
        private const double PuffShare = 0.45;
        private const double PuffSmoothing = 0.06;
        private const double Loudest = 0.7;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int Seed = 1912;

        public static float[] Samples(int samplesPerSecond)
        {
            var count = (int)(LengthSeconds * samplesPerSecond);
            var random = new Random(Seed);
            var samples = new float[count];
            var puff = 0.0;
            for (var index = 0; index < count; index++)
            {
                var time = (double)index / samplesPerSecond;
                puff += (random.NextDouble() * 2 - 1 - puff) * PuffSmoothing;
                var attack = Math.Min(1, time / AttackSeconds);
                samples[index] = (float)(attack * Loudest * (Engine(time) + puff * PuffShare * Math.Exp(-time * PuffDecayPerSecond)));
            }
            return samples;
        }

        private static double Engine(double time)
        {
            var thump = Math.Sin(FullTurnRadians * ThumpHertz * time) * Math.Exp(-time * ThumpDecayPerSecond);
            var knock = Math.Sin(FullTurnRadians * KnockHertz * time) * Math.Exp(-time * KnockDecayPerSecond) * KnockShare;
            var clank = Math.Sin(FullTurnRadians * ClankHertz * time) * Math.Exp(-time * ClankDecayPerSecond) * ClankShare;
            return thump + knock + clank;
        }
    }
}
