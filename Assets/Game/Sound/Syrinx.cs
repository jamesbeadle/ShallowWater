using System;

namespace ShallowWater.Game.Sound
{
    public sealed class Syrinx
    {
        private const double PulseSharpness = 12;
        private const double JitterShare = 0.07;
        private const double ShimmerShare = 0.3;
        private const double WeakPulseStrength = 0.55;
        private const double FullStrength = 1;
        private static readonly double PulseMean = (1 - Math.Exp(-PulseSharpness)) / PulseSharpness;

        private readonly int samplesPerSecond;
        private readonly Random random;
        private double phase;
        private double jitter = 1;
        private double strength = FullStrength;
        private bool isTheWeakPulse;

        public Syrinx(int samplesPerSecond, Random random)
        {
            this.samplesPerSecond = samplesPerSecond;
            this.random = random;
        }

        public double Sounded(double hertz, double breathShare)
        {
            var pulsesSoFar = Math.Floor(phase);
            phase += hertz * jitter / samplesPerSecond;
            var isANewPulse = Math.Floor(phase) > pulsesSoFar;
            if (isANewPulse) Roughen();
            return Pulse() * strength + Spread() * breathShare;
        }

        private void Roughen()
        {
            jitter = 1 + Spread() * JitterShare;
            isTheWeakPulse = !isTheWeakPulse;
            var alternation = isTheWeakPulse ? WeakPulseStrength : FullStrength;
            strength = alternation * (1 + Spread() * ShimmerShare);
        }

        private double Pulse()
        {
            var sinceThePulse = phase - Math.Floor(phase);
            return Math.Exp(-sinceThePulse * PulseSharpness) - PulseMean;
        }

        private double Spread()
        {
            return random.NextDouble() * 2 - 1;
        }
    }
}
