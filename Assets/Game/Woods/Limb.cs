using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public sealed class Limb
    {
        private readonly List<double> distances = new List<double>();

        public Limb(IReadOnlyList<WorldPoint> path, IReadOnlyList<double> radiiMetres, int order)
        {
            Path = path;
            RadiiMetres = radiiMetres;
            Order = order;
            var travelled = 0.0;
            for (var index = 0; index < path.Count; index++)
            {
                travelled += index == 0 ? 0 : Offset.Between(path[index - 1], path[index]).Length;
                distances.Add(travelled);
            }
        }

        public IReadOnlyList<WorldPoint> Path { get; }
        public IReadOnlyList<double> RadiiMetres { get; }
        public int Order { get; }
        public double LengthMetres => distances.Last();
        public int Segments => Path.Count - 1;

        public Limb Coarsened(int everyNthRing)
        {
            var kept = Enumerable.Range(0, Path.Count).Where(ring => ring % everyNthRing == 0 || ring == Segments).ToList();
            return new Limb(kept.Select(ring => Path[ring]).ToList(), kept.Select(ring => RadiiMetres[ring]).ToList(), Order);
        }

        public LimbPoint At(double share)
        {
            var wanted = share * LengthMetres;
            var segment = SegmentHolding(wanted);
            var from = Path[segment];
            var run = Offset.Between(from, Path[segment + 1]);
            var segmentLength = distances[segment + 1] - distances[segment];
            var within = (wanted - distances[segment]) / segmentLength;
            var radius = RadiiMetres[segment] + (RadiiMetres[segment + 1] - RadiiMetres[segment]) * within;
            return new LimbPoint((run * within).From(from), run.Normalised, radius);
        }

        private int SegmentHolding(double distance)
        {
            var lastSegment = Segments - 1;
            for (var segment = 0; segment < lastSegment; segment++)
            {
                var isWithin = distance <= distances[segment + 1];
                if (isWithin) return segment;
            }
            return lastSegment;
        }
    }
}
