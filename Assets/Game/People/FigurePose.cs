using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public sealed class FigurePose
    {
        private const double Whole = 1;
        private static readonly IReadOnlyDictionary<FigurePart, JointAngles> NoAngles = new Dictionary<FigurePart, JointAngles>();
        public static readonly FigurePose Upright = new FigurePose(NoAngles, new Offset(0, 0, 0));

        private readonly IReadOnlyDictionary<FigurePart, JointAngles> angles;

        public FigurePose(IReadOnlyDictionary<FigurePart, JointAngles> angles, Offset hipsOffset)
        {
            this.angles = angles;
            HipsOffset = hipsOffset;
        }

        public Offset HipsOffset { get; }
        public IEnumerable<FigurePart> Parts => angles.Keys;

        public JointAngles Of(FigurePart part)
        {
            return angles.TryGetValue(part, out var joint) ? joint : JointAngles.Still;
        }

        public FigurePose With(FigurePart part, JointAngles joint)
        {
            var changed = new Dictionary<FigurePart, JointAngles>(angles) { [part] = joint };
            return new FigurePose(changed, HipsOffset);
        }

        public FigurePose WithHips(Offset hipsOffset)
        {
            return new FigurePose(angles, hipsOffset);
        }

        public FigurePose Blend(FigurePose towards, double share)
        {
            return Mixed((this, Whole - share), (towards, share));
        }

        public static FigurePose Mixed(params (FigurePose pose, double weight)[] parts)
        {
            var mixed = new Dictionary<FigurePart, JointAngles>();
            var hips = new Offset(0, 0, 0);
            foreach (var (pose, weight) in parts)
            {
                foreach (var part in pose.Parts) mixed[part] = MixedAngles(mixed, part) + pose.Of(part) * weight;
                hips += pose.HipsOffset * weight;
            }
            return new FigurePose(mixed, hips);
        }

        public double LargestAngleDifferenceFrom(FigurePose other)
        {
            var parts = Parts.Union(other.Parts);
            return parts.Select(part => Of(part).LargestDifferenceFrom(other.Of(part))).DefaultIfEmpty(0).Max();
        }

        private static JointAngles MixedAngles(IReadOnlyDictionary<FigurePart, JointAngles> mixed, FigurePart part)
        {
            return mixed.TryGetValue(part, out var sum) ? sum : JointAngles.Still;
        }
    }
}
