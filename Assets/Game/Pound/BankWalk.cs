using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Pound
{
    public sealed class BankWalk
    {
        private readonly double side;
        private readonly List<Band> bands = new List<Band>();
        private double distanceFromCentre = PoundLimits.ChannelHalfWidthMetres;
        private double height = Heights.WaterMetres;

        private BankWalk(double side)
        {
            this.side = side;
        }

        public IReadOnlyList<Band> Bands => bands;
        private bool IsOnTheTowpathSide => side == PoundLimits.TowpathSide;

        public static BankWalk OnTheTowpathSide()
        {
            return new BankWalk(PoundLimits.TowpathSide);
        }

        public static BankWalk OnTheOffside()
        {
            return new BankWalk(-PoundLimits.TowpathSide);
        }

        public void Step(Surface surface, double widthMetres, double toHeightMetres)
        {
            StepOutTo(surface, distanceFromCentre + widthMetres, toHeightMetres);
        }

        public void StepOutTo(Surface surface, double toDistanceFromCentre, double toHeightMetres)
        {
            var inner = new BandEdge(side * distanceFromCentre, height);
            var outer = new BandEdge(side * toDistanceFromCentre, toHeightMetres);
            bands.Add(IsOnTheTowpathSide ? new Band(surface, outer, inner) : new Band(surface, inner, outer));
            distanceFromCentre = toDistanceFromCentre;
            height = toHeightMetres;
        }
    }
}
