using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Mooring
{
    public sealed class LeadSamples
    {
        private const double SampleMetres = 0.15;
        private const double StepMetres = 0.03;
        private const double FinestMetres = 0.01;
        private const double Half = 0.5;
        private const int Fewest = 1;

        private readonly Func<double, LeadPoint> pointAt;
        private readonly List<LeadPoint> points;

        public LeadSamples(double lengthMetres, Func<double, LeadPoint> pointAt)
        {
            this.pointAt = pointAt;
            points = Sampled(lengthMetres);
            LookCloserAtEachStep();
        }

        public IReadOnlyList<LeadPoint> Points => points;

        private List<LeadPoint> Sampled(double lengthMetres)
        {
            var count = Math.Max(Fewest, (int)Math.Ceiling(lengthMetres / SampleMetres));
            return Enumerable.Range(0, count + 1).Select(sample => pointAt(lengthMetres * sample / count)).ToList();
        }

        private void LookCloserAtEachStep()
        {
            var index = 0;
            while (index < points.Count - 1)
            {
                var needsACloserLook = IsACoarseStepAfter(index);
                if (!needsACloserLook) index++;
                if (needsACloserLook) LookCloserAfter(index);
            }
        }

        private bool IsACoarseStepAfter(int index)
        {
            var before = points[index];
            var after = points[index + 1];
            var isAStep = Math.Abs(after.RestingHeight - before.RestingHeight) > StepMetres;
            var isCoarse = after.Along - before.Along > FinestMetres;
            return isAStep && isCoarse;
        }

        private void LookCloserAfter(int index)
        {
            var before = points[index];
            var after = points[index + 1];
            points.Insert(index + 1, pointAt((before.Along + after.Along) * Half));
        }
    }
}
