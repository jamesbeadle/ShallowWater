using System;

namespace ShallowWater.Game.Sound
{
    public static class RifleCrack
    {
        private const double LengthSeconds = 0.45;
        private const double AttackSeconds = 0.0008;
        private const double CrackDecayPerSecond = 60;
        private const double ReportHertz = 70;
        private const double ReportDecayPerSecond = 14;
        private const double ReportShare = 0.5;
        private const double EchoDelaySeconds = 0.12;
        private const double EchoShare = 0.22;
        private const double EchoDecayPerSecond = 9;
        private const double Loudest = 0.9;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int Seed = 1938;

        public static float[] Samples(int samplesPerSecond)
        {
            var count = (int)(LengthSeconds * samplesPerSecond);
            var random = new Random(Seed);
            var samples = new float[count];
            for (var index = 0; index < count; index++)
            {
                var time = (double)index / samplesPerSecond;
                var noise = random.NextDouble() * 2 - 1;
                var attack = Math.Min(1, time / AttackSeconds);
                samples[index] = (float)(attack * (Crack(time, noise) + Echo(time, noise)));
            }
            return Loudness.Set(samples, Loudest);
        }

        private static double Crack(double time, double noise)
        {
            var crack = noise * Math.Exp(-time * CrackDecayPerSecond);
            var report = Math.Sin(FullTurnRadians * ReportHertz * time) * Math.Exp(-time * ReportDecayPerSecond) * ReportShare;
            return crack + report;
        }

        private static double Echo(double time, double noise)
        {
            var sinceTheEcho = time - EchoDelaySeconds;
            if (sinceTheEcho < 0) return 0;
            return noise * EchoShare * Math.Exp(-sinceTheEcho * EchoDecayPerSecond);
        }
    }
}
