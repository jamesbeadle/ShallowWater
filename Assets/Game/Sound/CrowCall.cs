using System;

namespace ShallowWater.Game.Sound
{
    public static class CrowCall
    {
        private const double LengthSeconds = 0.3;
        private const double AttackSeconds = 0.03;
        private const double DecayFromSeconds = 0.16;
        private const double DecayPerSecond = 22;
        private const double StartHertz = 400;
        private const double EndHertz = 290;
        private const double PulseSharpness = 9;
        private const double RaspHertz = 70;
        private const double RaspDepth = 0.35;
        private const double Half = 0.5;
        private const double NoiseShare = 0.4;
        private const double ThroatHertz = 1400;
        private const double ThroatWidthHertz = 400;
        private const double Loudest = 0.6;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int Seed = 1939;
        private static readonly double PulseMean = (1 - Math.Exp(-PulseSharpness)) / PulseSharpness;

        public static float[] Samples(int samplesPerSecond)
        {
            var count = (int)(LengthSeconds * samplesPerSecond);
            var random = new Random(Seed);
            var throat = new Throat(ThroatHertz, ThroatWidthHertz, samplesPerSecond);
            var samples = new float[count];
            var phase = 0.0;
            for (var index = 0; index < count; index++)
            {
                var time = (double)index / samplesPerSecond;
                phase += HertzAt(time) / samplesPerSecond;
                var noise = random.NextDouble() * 2 - 1;
                var voiced = throat.Resonated(Pulse(phase) + noise * NoiseShare);
                samples[index] = (float)(Envelope(time) * Rasp(time) * voiced);
            }
            return Loudness.Set(samples, Loudest);
        }

        private static double HertzAt(double time)
        {
            return StartHertz + (EndHertz - StartHertz) * time / LengthSeconds;
        }

        private static double Pulse(double phase)
        {
            var sinceThePulse = phase - Math.Floor(phase);
            return Math.Exp(-sinceThePulse * PulseSharpness) - PulseMean;
        }

        private static double Rasp(double time)
        {
            return 1 - RaspDepth * (Half + Half * Math.Sin(FullTurnRadians * RaspHertz * time));
        }

        private static double Envelope(double time)
        {
            var attack = Math.Min(1, time / AttackSeconds);
            var sinceTheDecayBegan = Math.Max(0, time - DecayFromSeconds);
            return attack * Math.Exp(-sinceTheDecayBegan * DecayPerSecond);
        }
    }
}
