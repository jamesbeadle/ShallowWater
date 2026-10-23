using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Walking;
using CanalPound = ShallowWater.Game.Pound.Pound;

namespace ShallowWater.Game.Mooring
{
    public sealed class SurfaceBeneath
    {
        private readonly CanalPound pound;
        private readonly Shore shore;
        private readonly BoatMotion boat;

        public SurfaceBeneath(CanalPound pound, BoatMotion boat)
        {
            this.pound = pound;
            this.boat = boat;
            shore = Shore.Along(pound);
        }

        public WaterPosition WaterPositionAt(GroundPoint place)
        {
            return pound.WaterPositionAt(place);
        }

        public WaterPosition WaterPositionWithin(GroundPoint place, Span span)
        {
            return pound.WaterPositionWithin(place, span);
        }

        public WaterPosition WaterPositionBeside(WaterPosition water, GroundPoint step)
        {
            var heading = GroundPoint.Facing(pound.BearingAt(water.Along));
            return new WaterPosition(water.Along + step.Dot(heading), water.Across + step.Dot(heading.RightAngleClockwise));
        }

        public double RestingHeightAt(GroundPoint place, WaterPosition water)
        {
            var onTheHull = HullPoint.On(boat, place);
            var isOverTheBoat = SparrowTop.IsOver(onTheHull);
            if (isOverTheBoat) return OnTheBoat(onTheHull);
            var isAfloat = shore.IsWaterAt(water);
            if (isAfloat) return Rope.Afloat;
            return Rope.RestingOn(shore.HeightAt(water));
        }

        private static double OnTheBoat(HullPoint onTheHull)
        {
            var top = SparrowTop.HeightAt(onTheHull);
            return Rope.RestingOn(top);
        }
    }
}
