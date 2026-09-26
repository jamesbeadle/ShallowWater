using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public sealed class SectionWalk
    {
        private const double LeftSide = -1;
        private const double Smooth = 0;

        private readonly double side;
        private readonly List<Band> bands = new List<Band>();

        public SectionWalk(double side, double startFromCentreMetres, double startHeightMetres)
        {
            this.side = side;
            DistanceFromCentre = startFromCentreMetres;
            Height = startHeightMetres;
        }

        public IReadOnlyList<Band> Bands => bands;
        public double DistanceFromCentre { get; private set; }
        public double Height { get; private set; }
        private double RoughnessMetres { get; set; }
        private bool IsOnTheLeft => side == LeftSide;

        public void Step(Surface surface, double widthMetres, double toHeightMetres)
        {
            StepOutTo(surface, DistanceFromCentre + widthMetres, toHeightMetres);
        }

        public void StepToRough(Surface surface, double widthMetres, double toHeightMetres, double roughnessMetres)
        {
            Walk(surface, DistanceFromCentre + widthMetres, toHeightMetres, roughnessMetres);
        }

        public void StepOutTo(Surface surface, double toDistanceFromCentre, double toHeightMetres)
        {
            Walk(surface, toDistanceFromCentre, toHeightMetres, Smooth);
        }

        private void Walk(Surface surface, double toDistanceFromCentre, double toHeightMetres, double toRoughnessMetres)
        {
            var inner = new BandEdge(side * DistanceFromCentre, Height, RoughnessMetres);
            var outer = new BandEdge(side * toDistanceFromCentre, toHeightMetres, toRoughnessMetres);
            bands.Add(IsOnTheLeft ? new Band(surface, outer, inner) : new Band(surface, inner, outer));
            DistanceFromCentre = toDistanceFromCentre;
            Height = toHeightMetres;
            RoughnessMetres = toRoughnessMetres;
        }
    }
}
