using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.Roads
{
    public sealed class RoadNetwork
    {
        private readonly RoadSurroundings surroundings;

        public RoadNetwork(IReadOnlyList<Road> roads, RoadSurroundings surroundings)
        {
            Roads = roads;
            this.surroundings = surroundings;
        }

        public IReadOnlyList<Road> Roads { get; }
        public SurfaceShapes Shapes => RoadShapes.Of(Roads, surroundings);
        public IEnumerable<Hedge> Hedges => RoadHedges.Along(Roads, surroundings);
        public IEnumerable<FloorStrip> Beds => RoadBeds.Along(Roads, surroundings);
    }
}
