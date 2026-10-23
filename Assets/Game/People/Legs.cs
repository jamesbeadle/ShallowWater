using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Legs
    {
        private const double Level = 0;
        private const double TowardsTheRight = 1;
        private const double TowardsTheLeft = -1;
        private static readonly Offset AtTheJoint = new Offset(Level, Level, Level);

        public static FigurePose Planted(FigurePose pose, FootStep left, FootStep right)
        {
            return Planted(Planted(pose, FigurePart.LeftFoot, left), FigurePart.RightFoot, right);
        }

        public static FigurePose Planted(FigurePose pose, FigurePart foot, FootStep step)
        {
            var reaching = Reaching(pose, foot, step);
            return Laid(reaching, foot, step);
        }

        private static FigurePose Reaching(FigurePose pose, FigurePart foot, FootStep step)
        {
            var shin = FigureJoints.Parents[foot];
            var thigh = FigureJoints.Parents[shin];
            var frames = FigurePlacement.Of(pose);
            var hips = frames[FigurePart.Hips];
            var hipJoint = hips.Place(FigureJoints.FromTheParent[thigh]);
            var toTheAnkle = hips.Local(Offset.Between(hipJoint, step.Ankle));
            var angles = LimbReach.Leg(toTheAnkle, AskewForm.ThighMetres, AskewForm.ShinMetres, Offset.North);
            return pose.With(thigh, angles.UpperOf(thigh)).With(shin, angles.Lower);
        }

        private static FigurePose Laid(FigurePose pose, FigurePart foot, FootStep step)
        {
            var isOnTheRight = JointAxes.Sides[foot] == FigureSide.Right;
            var turnedOut = step.ToeOutRadians * (isOnTheRight ? TowardsTheRight : TowardsTheLeft);
            var wanted = Frame.AtTheFeet.Child(AtTheJoint, new JointAngles(step.ToesDownRadians, Level, turnedOut));
            var frames = FigurePlacement.Of(pose);
            var shin = FigurePlacement.ParentFrameOf(foot, frames);
            return pose.With(foot, JointAxes.Signed(foot, shin.SignedAnglesOf(wanted)));
        }
    }
}
