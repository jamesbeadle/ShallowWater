using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public sealed class Whereabouts
    {
        private const double AtTheLandmarkMetres = 20;
        private const double Southward = 0;
        private const string At = "at ";
        private const string NorthOf = " north of ";
        private const string SouthOf = " south of ";

        private Whereabouts(Landmark landmark, double metresSouthOfIt)
        {
            Landmark = landmark;
            MetresSouthOfIt = metresSouthOfIt;
        }

        public Landmark Landmark { get; }
        public double MetresSouthOfIt { get; }
        public bool IsAtTheLandmark => Math.Abs(MetresSouthOfIt) < AtTheLandmarkMetres;

        public string Words
        {
            get
            {
                var name = LandmarkNames.Of(Landmark);
                if (IsAtTheLandmark) return At + name;
                var isSouth = MetresSouthOfIt > Southward;
                return OldDistance.Words(MetresSouthOfIt) + (isSouth ? SouthOf : NorthOf) + name;
            }
        }

        public static Whereabouts Of(Pound pound, GroundPoint place)
        {
            var position = pound.WaterPositionAt(place);
            var along = position.Along;
            var nearest = Landmark.HopwasBridge;
            var nearestMetres = double.MaxValue;
            foreach (Landmark landmark in Enum.GetValues(typeof(Landmark)))
            {
                var metresSouth = along - pound.AlongOf(landmark);
                var isNearer = Math.Abs(metresSouth) < Math.Abs(nearestMetres);
                if (!isNearer) continue;
                nearest = landmark;
                nearestMetres = metresSouth;
            }
            return new Whereabouts(nearest, nearestMetres);
        }
    }
}
