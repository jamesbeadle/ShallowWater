using System;

namespace ShallowWater.Game.People
{
    public static class Skirts
    {
        private const double FollowShare = 0.55;
        private const double FlareShare = 0.06;
        private const double Level = 0;

        public static FigurePose Following(FigurePose pose)
        {
            var left = Hung(pose, FigurePart.LeftSkirt, FigurePart.LeftThigh);
            return Hung(left, FigurePart.RightSkirt, FigurePart.RightThigh);
        }

        private static FigurePose Hung(FigurePose pose, FigurePart skirt, FigurePart thigh)
        {
            var leg = pose.Of(thigh);
            var swing = leg.SwingRadians * FollowShare;
            var flare = Math.Abs(leg.SwingRadians) * FlareShare;
            return pose.With(skirt, new JointAngles(swing, flare, Level));
        }
    }
}
