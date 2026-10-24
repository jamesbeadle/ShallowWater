using System;

namespace ShallowWater.Game.Sound
{
    public sealed class Throat
    {
        private const double FullTurnRadians = 2 * Math.PI;

        private readonly double ring;
        private readonly double pull;
        private double last;
        private double beforeLast;

        public Throat(double resonanceHertz, double widthHertz, int samplesPerSecond)
        {
            var decay = Math.Exp(-Math.PI * widthHertz / samplesPerSecond);
            ring = 2 * decay * Math.Cos(FullTurnRadians * resonanceHertz / samplesPerSecond);
            pull = decay * decay;
        }

        public double Resonated(double sample)
        {
            var resonated = sample + ring * last - pull * beforeLast;
            beforeLast = last;
            last = resonated;
            return resonated;
        }
    }
}
