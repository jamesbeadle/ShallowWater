using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public sealed class Wood
    {
        private const double EdgeBeltMetres = 45;
        private readonly Area area;
        private readonly WoodEdge edge;
        private readonly Rides rides;
        private readonly Compartments compartments;

        public Wood(Area area)
        {
            this.area = area;
            Outline = area.Outline;
            edge = new WoodEdge(Outline);
            rides = new Rides(Outline);
            compartments = new Compartments(area, edge);
        }

        public GroundRing Outline { get; }
        public IEnumerable<Compartment> Plantations => compartments.Plantations;

        public bool IsBroadleafAt(GroundPoint place)
        {
            var isOpen = IsOpenGround(place);
            if (!isOpen) return false;
            var compartment = compartments.Holding(place);
            return !compartment.IsPlantation || IsInTheEdgeBelt(place);
        }

        public bool IsPlantationAt(GroundPoint place, Compartment plantation)
        {
            var isOpen = IsOpenGround(place) && !IsInTheEdgeBelt(place);
            if (!isOpen) return false;
            return compartments.Holding(place) == plantation;
        }

        public double EdgeDistanceFrom(GroundPoint place)
        {
            return edge.DistanceFrom(place);
        }

        private bool IsInTheEdgeBelt(GroundPoint place)
        {
            return edge.DistanceFrom(place) < EdgeBeltMetres;
        }

        private bool IsOpenGround(GroundPoint place)
        {
            var isInTheWood = area.IsAround(place);
            return isInTheWood && !rides.IsRideAt(place);
        }
    }
}
