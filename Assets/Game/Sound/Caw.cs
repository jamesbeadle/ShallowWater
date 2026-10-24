using System;

namespace ShallowWater.Game.Sound
{
    public sealed class Caw
    {
        private const double AttackSeconds = 0.02;
        private const double FadeSeconds = 0.03;
        private const double DecayFromShare = 0.6;
        private const double DecayPerSecond = 14;
        private const double PulseSharpness = 7;
        private const double JitterShare = 0.04;
        private const double RaspHertz = 55;
        private const double RaspDepth = 0.3;
        private const double Half = 0.5;
        private const double BreathShare = 0.2;
        private const double OpeningBreathShare = 0.6;
        private const double OpeningSeconds = 0.04;
        private const double DirectShare = 0.35;
        private const double FullTurnRadians = 2 * Math.PI;
        private static readonly double PulseMean = (1 - Math.Exp(-PulseSharpness)) / PulseSharpness;
        private static readonly Formant[] Mouth = { new Formant(800, 250, 1), new Formant(1700, 350, 0.6), new Formant(2800, 500, 0.35) };

        private readonly double lengthSeconds;
        private readonly double pitch;
        private readonly double loudness;

        public Caw(double lengthSeconds, double pitch, double loudness)
        {
            this.lengthSeconds = lengthSeconds;
            this.pitch = pitch;
            this.loudness = loudness;
        }

        public float[] Samples(int samplesPerSecond, Random random)
        {
            var count = (int)(lengthSeconds * samplesPerSecond);
            var mouth = new Formants(samplesPerSecond, DirectShare, Mouth);
            var samples = new float[count];
            var phase = 0.0;
            var jitter = 1.0;
            for (var index = 0; index < count; index++)
            {
                var time = (double)index / samplesPerSecond;
                var share = time / lengthSeconds;
                var pulsesSoFar = Math.Floor(phase);
                phase += pitch * jitter * CawPitch.HertzAt(share) / samplesPerSecond;
                var isANewPulse = Math.Floor(phase) > pulsesSoFar;
                if (isANewPulse) jitter = 1 + Spread(random) * JitterShare;
                var breath = Spread(random) * BreathAt(time);
                var voiced = mouth.Shaped(Pulse(phase) + breath);
                samples[index] = (float)(loudness * Envelope(time, share) * Rasp(time) * voiced);
            }
            return samples;
        }

        private static double Spread(Random random)
        {
            return random.NextDouble() * 2 - 1;
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

        private static double BreathAt(double time)
        {
            var opening = Math.Max(0, 1 - time / OpeningSeconds);
            return BreathShare + OpeningBreathShare * opening;
        }

        private double Envelope(double time, double share)
        {
            var attack = Math.Min(1, time / AttackSeconds);
            var sinceTheDecayBegan = Math.Max(0, share - DecayFromShare) * lengthSeconds;
            var fade = Math.Min(1, (1 - share) * lengthSeconds / FadeSeconds);
            return attack * Math.Exp(-sinceTheDecayBegan * DecayPerSecond) * fade;
        }
    }
}
