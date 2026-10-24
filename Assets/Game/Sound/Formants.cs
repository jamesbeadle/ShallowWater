using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Sound
{
    public sealed class Formants
    {
        private readonly double directShare;
        private readonly IReadOnlyList<Throat> throats;
        private readonly IReadOnlyList<double> shares;

        public Formants(int samplesPerSecond, double directShare, IReadOnlyList<Formant> formants)
        {
            this.directShare = directShare;
            throats = formants.Select(formant => new Throat(formant.Hertz, formant.WidthHertz, samplesPerSecond)).ToList();
            shares = formants.Select(formant => formant.Share).ToList();
        }

        public double Shaped(double sample)
        {
            var shaped = directShare * sample;
            for (var index = 0; index < throats.Count; index++) shaped += shares[index] * throats[index].Resonated(sample);
            return shaped;
        }
    }
}
