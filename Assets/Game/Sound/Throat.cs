using System;

namespace ShallowWater.Game.Sound
{
    public sealed class Throat
    {
        private const double FullTurnRadians = 2 * Math.PI;

        private readonly double ring;
        private readonly double pull;
        private readonly double peakGain;
        private double last;
        private double beforeLast;

        public Throat(double resonanceHertz, double widthHertz, int samplesPerSecond)
        {
            var decay = Math.Exp(-Math.PI * widthHertz / samplesPerSecond);
            var turn = FullTurnRadians * resonanceHertz / samplesPerSecond;
            ring = 2 * decay * Math.Cos(turn);
            pull = decay * decay;
            peakGain = 1 / ((1 - decay) * Math.Sqrt(1 - 2 * decay * Math.Cos(2 * turn) + pull));
        }

        public double Resonated(double sample)
        {
            var resonated = sample + ring * last - pull * beforeLast;
            beforeLast = last;
            last = resonated;
            return resonated / peakGain;
        }
    }
}
