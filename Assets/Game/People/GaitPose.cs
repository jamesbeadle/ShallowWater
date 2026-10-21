using System.Collections.Generic;

namespace ShallowWater.Game.People
{
    public sealed class GaitPose
    {
        private const double Upright = 0;
        private const double OnTheCentreline = 0;
        private static readonly IReadOnlyDictionary<FigurePart, double> NoReach = new Dictionary<FigurePart, double>();

        public GaitPose(IReadOnlyDictionary<FigurePart, double> swingRadians, double bobMetres, double leanRadians)
            : this(swingRadians, bobMetres, leanRadians, NoReach, Upright, OnTheCentreline)
        {
        }

        public GaitPose(
            IReadOnlyDictionary<FigurePart, double> swingRadians, double bobMetres, double leanRadians,
            IReadOnlyDictionary<FigurePart, double> reachRadians, double swayRadians, double stepAcrossMetres)
        {
            SwingRadians = swingRadians;
            BobMetres = bobMetres;
            LeanRadians = leanRadians;
            ReachRadians = reachRadians;
            SwayRadians = swayRadians;
            StepAcrossMetres = stepAcrossMetres;
        }

        public IReadOnlyDictionary<FigurePart, double> SwingRadians { get; }
        public double BobMetres { get; }
        public double LeanRadians { get; }
        public IReadOnlyDictionary<FigurePart, double> ReachRadians { get; }
        public double SwayRadians { get; }
        public double StepAcrossMetres { get; }

        public double ReachOf(FigurePart part)
        {
            return ReachRadians.TryGetValue(part, out var reach) ? reach : Upright;
        }

        public GaitPose Holding(FigurePart upperArm, FigurePart forearm, ArmAngles arm)
        {
            var swings = new Dictionary<FigurePart, double>(SwingRadians) { [upperArm] = arm.SwingRadians, [forearm] = arm.ElbowRadians };
            var reaches = new Dictionary<FigurePart, double>(ReachRadians) { [upperArm] = arm.ReachRadians };
            return new GaitPose(swings, BobMetres, LeanRadians, reaches, SwayRadians, StepAcrossMetres);
        }
    }
}
