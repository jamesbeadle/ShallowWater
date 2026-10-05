using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class SternLine
    {
        public const double LengthMetres = 9;
        private const double SagShare = 0.08;
        private const double RadiusMetres = 0.012;
        private const int Spans = 14;
        private const int RopeSides = 6;
        private const double SagShape = 4;
        private static readonly double[] Sides = { BoatSides.Port, BoatSides.Starboard };

        public static WorldPoint DollyNearest(BoatMotion boat, GroundPoint feet)
        {
            var dollies = Sides.Select(side => boat.PointOf(Dollies.On(side)));
            var nearest = dollies.OrderBy(dolly => dolly.DistanceTo(feet)).First();
            return new WorldPoint(nearest.East, Heights.WaterMetres + Dollies.TieMetres, nearest.North);
        }

        public static bool CanReach(BoatMotion boat, GroundPoint feet)
        {
            var dolly = DollyNearest(boat, feet);
            return new GroundPoint(dolly.East, dolly.North).DistanceTo(feet) < LengthMetres;
        }

        public static SurfaceShapes Between(WorldPoint dolly, WorldPoint post)
        {
            var reach = Offset.Between(dolly, post);
            var sag = SagShare * reach.Length;
            var hanging = Enumerable.Range(0, Spans + 1).Select(span =>
            {
                var share = (double)span / Spans;
                var drop = SagShape * sag * share * (1 - share);
                return (reach * share + Offset.Up * -drop).From(dolly);
            });
            var line = new SurfaceShapes();
            line.Add(Surface.Rope, Tube.Along(hanging.ToList(), RadiusMetres, RopeSides));
            return line;
        }
    }
}
