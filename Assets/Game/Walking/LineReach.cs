using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public readonly struct LineReach
    {
        private LineReach(GroundPoint southWest, GroundPoint northEast)
        {
            SouthWest = southWest;
            NorthEast = northEast;
        }

        public GroundPoint SouthWest { get; }
        public GroundPoint NorthEast { get; }

        public static LineReach Around(GroundLine line, double reachMetres)
        {
            var points = line.Points;
            var margin = new GroundPoint(reachMetres, reachMetres);
            var southWest = new GroundPoint(points.Min(point => point.East), points.Min(point => point.North));
            var northEast = new GroundPoint(points.Max(point => point.East), points.Max(point => point.North));
            return new LineReach(southWest - margin, northEast + margin);
        }
    }
}
