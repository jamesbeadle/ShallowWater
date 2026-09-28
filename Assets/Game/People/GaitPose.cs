using System.Collections.Generic;

namespace ShallowWater.Game.People
{
    public sealed class GaitPose
    {
        public GaitPose(IReadOnlyDictionary<FigurePart, double> swingRadians, double bobMetres, double leanRadians)
        {
            SwingRadians = swingRadians;
            BobMetres = bobMetres;
            LeanRadians = leanRadians;
        }

        public IReadOnlyDictionary<FigurePart, double> SwingRadians { get; }
        public double BobMetres { get; }
        public double LeanRadians { get; }
    }
}
