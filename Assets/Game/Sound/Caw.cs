using System;

namespace ShallowWater.Game.Sound
{
    public sealed class Caw
    {
        private const double AttackSeconds = 0.012;
        private const double FadeSeconds = 0.04;
        private const double DecayFromShare = 0.5;
        private const double DecayPerSecond = 9;
        private const double BreathShare = 0.45;
        private const double OpeningBreathShare = 0.8;
        private const double OpeningSeconds = 0.03;
        private const double DirectShare = 0.06;
        private static readonly Formant[] Beak = { new Formant(1150, 450, 1), new Formant(2050, 550, 0.75), new Formant(3300, 800, 0.45) };

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
            var beak = new Formants(samplesPerSecond, DirectShare, Beak);
            var syrinx = new Syrinx(samplesPerSecond, random);
            var samples = new float[count];
            for (var index = 0; index < count; index++)
            {
                var time = (double)index / samplesPerSecond;
                var share = time / lengthSeconds;
                var hertz = pitch * CawPitch.HertzAt(share);
                var sounded = syrinx.Sounded(hertz, BreathAt(time));
                samples[index] = (float)(loudness * Envelope(time, share) * beak.Shaped(sounded));
            }
            return samples;
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
