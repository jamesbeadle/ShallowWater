using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Arms
    {
        public static FigurePose Reaching(FigurePose pose, FigurePart hand, WorldPoint palm, Offset elbowTowards)
        {
            var forearm = FigureJoints.Parents[hand];
            var upperArm = FigureJoints.Parents[forearm];
            var frames = FigurePlacement.Of(pose);
            var chest = frames[FigurePart.Chest];
            var shoulder = chest.Place(FigureJoints.FromTheParent[upperArm]);
            var toThePalm = chest.Local(Offset.Between(shoulder, palm));
            var angles = LimbReach.Arm(toThePalm, AskewForm.UpperArmMetres, AskewForm.HandReachMetres, elbowTowards);
            return pose.With(upperArm, angles.UpperOf(upperArm)).With(forearm, angles.Lower).With(hand, JointAngles.Still);
        }

        public static WorldPoint PalmOf(FigurePose pose, FigurePart hand)
        {
            var frames = FigurePlacement.Of(pose);
            var wrist = frames[hand];
            return wrist.Place(new Offset(0, -AskewForm.PalmBelowTheWristMetres, 0));
        }
    }
}
