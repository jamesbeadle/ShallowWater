using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Mooring
{
    public readonly struct LeadPoint
    {
        public LeadPoint(double along, GroundPoint place, WaterPosition water, double restingHeight)
        {
            Along = along;
            Place = place;
            Water = water;
            RestingHeight = restingHeight;
        }

        public double Along { get; }
        public GroundPoint Place { get; }
        public WaterPosition Water { get; }
        public double RestingHeight { get; }
    }
}
