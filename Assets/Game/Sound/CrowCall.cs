using System;

namespace ShallowWater.Game.Sound
{
    public static class CrowCall
    {
        private const double LengthSeconds = 0.32;
        private const double AttackSeconds = 0.025;
        private const double DecayFromSeconds = 0.14;
        private const double DecayPerSecond = 16;
        private const double StartHertz = 540;
        private const double EndHertz = 360;
        private const double SecondHarmonic = 2;
        private const double SecondHarmonicShare = 0.6;
        private const double ThirdHarmonic = 3;
        private const double ThirdHarmonicShare = 0.35;
        private const double RaspHertz = 95;
        private const double RaspDepth = 0.55;
        private const double Half = 0.5;
        private const double NoiseShare = 0.25;
        private const double Loudest = 0.7;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int Seed = 1939;

        public static float[] Samples(int samplesPerSecond)
        {
            var count = (int)(LengthSeconds * samplesPerSecond);
            var random = new Random(Seed);
            var samples = new float[count];
            var phase = 0.0;
            for (var index = 0; index < count; index++)
            {
                var time = (double)index / samplesPerSecond;
                phase += FullTurnRadians * HertzAt(time) / samplesPerSecond;
                var noise = random.NextDouble() * 2 - 1;
                samples[index] = (float)(Loudest * Envelope(time) * Rasp(time) * (Tone(phase) + noise * NoiseShare));
            }
            return samples;
        }

        private static double HertzAt(double time)
        {
            return StartHertz + (EndHertz - StartHertz) * time / LengthSeconds;
        }

        private static double Tone(double phase)
        {
            return Math.Sin(phase) + Math.Sin(SecondHarmonic * phase) * SecondHarmonicShare + Math.Sin(ThirdHarmonic * phase) * ThirdHarmonicShare;
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
