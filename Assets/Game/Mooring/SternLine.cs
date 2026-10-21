using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class SternLine
    {
        public const double LengthMetres = 9;
        private static readonly double[] Sides = { BoatSides.Port, BoatSides.Starboard };

        public static HullPoint DollyNearest(BoatMotion boat, GroundPoint feet)
        {
            var dollies = Sides.Select(Dollies.On);
            return dollies.OrderBy(dolly => boat.PointOf(dolly).DistanceTo(feet)).First();
        }

        public static WorldPoint TieOn(BoatMotion boat, HullPoint dolly)
        {
            var place = boat.PointOf(dolly);
            return new WorldPoint(place.East, Heights.WaterMetres + Dollies.TieMetres, place.North);
        }

        public static bool CanReach(BoatMotion boat, GroundPoint feet)
        {
            var dolly = DollyNearest(boat, feet);
            return boat.PointOf(dolly).DistanceTo(feet) < LengthMetres;
        }
    }
}
