using System;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Roads
{
    public sealed class Road
    {
        private const double StackingMetres = 0.002;
        private const double SampleSpacingMetres = 5;
        private const double OnTheCentreline = 0;
        private const double StartMetres = 0;
        private const int FirstStep = 0;
        private const int FewestSteps = 1;

        private readonly RoadClass roadClass;

        public Road(GroundLine line, RoadClass roadClass, int index)
        {
            this.roadClass = roadClass;
            Centreline = new Centreline(line);
            Index = index;
            HeightMetres = roadClass.HeightMetres - StackingMetres * index;
        }

        public Centreline Centreline { get; }
        public int Index { get; }
        public double HeightMetres { get; }
        public GroundLine Line => Centreline.Line;
        public double LengthMetres => Centreline.LengthMetres;
        public Span Whole => new Span(StartMetres, LengthMetres);
        public Surface Surface => roadClass.Surface;
        public double WidthMetres => roadClass.WidthMetres;
        public double HalfWidthMetres => roadClass.HalfWidthMetres;
        public double VergeWidthMetres => roadClass.VergeWidthMetres;
        public bool HasCentreLine => roadClass.HasCentreLine;
        public bool HasTelegraphPoles => roadClass.HasTelegraphPoles;

        public GroundPoint At(double along, double across)
        {
            return Centreline.Offset(along, across);
        }

        public double BearingAt(double along)
        {
            return Centreline.BearingAt(along);
        }

        public GroundLine Between(Span span)
        {
            var steps = Math.Max(FewestSteps, (int)Math.Ceiling(span.Length / SampleSpacingMetres));
            var places = Enumerable.Range(FirstStep, steps + 1).Select(step => At(span.From + span.Length * step / steps, OnTheCentreline));
            return new GroundLine(places);
        }
    }
}
