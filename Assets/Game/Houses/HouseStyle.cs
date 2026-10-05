using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public abstract class HouseStyle
    {
        private static readonly IReadOnlyList<Joinery> NoJoinery = new List<Joinery>();

        public abstract double DepthMetres { get; }
        public abstract double RoofRisePerMetre { get; }
        public abstract double VergeMetres { get; }
        public abstract double StackAboveTheRidgeMetres { get; }
        public abstract Lintel Lintel { get; }
        public virtual bool HasBargeboards => false;
        public virtual bool CanBeHipped => false;
        public virtual bool StandsAlone => false;
        public virtual double OutshutDepthMetres => 0;

        public abstract double FrontageMetres(Random random);
        public abstract int HousesInARun(Random random);
        public abstract double EavesMetres(Random random);
        public abstract Surface Walls(Random random);
        public abstract Surface Roof(Random random);
        public abstract Extras ExtrasFor(Random random);
        public abstract IReadOnlyList<Joinery> FrontOf(House house, double eavesMetres);
        public abstract IReadOnlyList<Joinery> BackOf(House house, double eavesMetres);

        public virtual double FrontSetBackMetres(Random random)
        {
            return 0;
        }

        public virtual Outshut OutshutOf(House house)
        {
            return Outshut.None;
        }

        public virtual IReadOnlyList<Joinery> DormersOf(House house, double eavesMetres)
        {
            return NoJoinery;
        }

        public double TotalDepthMetres => DepthMetres + OutshutDepthMetres;

        public bool CanStandIn(double depthMetres)
        {
            return TotalDepthMetres <= depthMetres;
        }
    }
}
