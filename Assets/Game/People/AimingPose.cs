using ShallowWater.Game.Guns;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AimingPose
    {
        private const double HipsTwistRadians = 0.35;
        private const double ChestTwistRadians = -0.15;
        private const double ShouldersTwistRadians = HipsTwistRadians + ChestTwistRadians;
        private const double TorsoLeanRadians = 0.25;
        private const double ChestTiltRadians = 0.15;
        private const double CheekDownRadians = 0.55;
        private const double CheekTiltRadians = 0.55;
        private const double StepAcrossMetres = 0.02;
        private const double StepForwardMetres = 0.03;
        private const double SoftKneesMetres = 0.012;
        private const double Level = 0;
        private static readonly FootStep LeftFoot = FootStep.Flat(-0.15, 0.22, 0.12);
        private static readonly FootStep RightFoot = FootStep.Flat(0.17, -0.1, 0.5);
        private static readonly Offset GripFromTheButt = new Offset(0, -0.045, 0.33);
        private static readonly Offset ForeEndFromTheButt = new Offset(0, -0.035, 0.52);
        private static readonly Offset RightElbowOutAndUp = new Offset(1, 0.5, -0.3);
        private static readonly Offset LeftElbowDown = new Offset(-0.3, -1, 0);
        private static readonly FigurePose Standing = Legs.Planted(Body(), LeftFoot, RightFoot);

        public static readonly WorldPoint Eye = EyeOf(Standing);

        public static FigurePose At(double muzzleDownRadians)
        {
            var holding = Arms.Reaching(Standing, FigurePart.RightHand, OnTheRifle(GripFromTheButt, muzzleDownRadians), RightElbowOutAndUp);
            var supporting = Arms.Reaching(holding, FigurePart.LeftHand, OnTheRifle(ForeEndFromTheButt, muzzleDownRadians), LeftElbowDown);
            return Skirts.Following(supporting);
        }

        public static WorldPoint OnTheRifle(Offset fromTheButt, double muzzleDownRadians)
        {
            var fromTheEye = (fromTheButt - RookRifle.EyeFromTheButt).TurnedAbout(Offset.East, muzzleDownRadians);
            return fromTheEye.From(Eye);
        }

        private static FigurePose Body()
        {
            return FigurePose.Upright.WithHips(new Offset(StepAcrossMetres, -SoftKneesMetres, StepForwardMetres))
                .With(FigurePart.Hips, new JointAngles(Level, Level, HipsTwistRadians))
                .With(FigurePart.Chest, new JointAngles(TorsoLeanRadians, ChestTiltRadians, ChestTwistRadians))
                .With(FigurePart.Head, new JointAngles(CheekDownRadians, CheekTiltRadians, -ShouldersTwistRadians));
        }

        private static WorldPoint EyeOf(FigurePose pose)
        {
            var frames = FigurePlacement.Of(pose);
            var head = frames[FigurePart.Head];
            return head.Place(AskewFace.RightEyeFromTheNeck);
        }
    }
}
