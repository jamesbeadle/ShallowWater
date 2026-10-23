using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AimingPose
    {
        private const double HipsTwistRadians = 0.35;
        private const double ChestTwistRadians = -0.15;
        private const double TorsoLeanRadians = 0.08;
        private const double ChestTiltRadians = 0.05;
        private const double CheekDownRadians = 0.18;
        private const double CheekTiltRadians = 0.22;
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

        public const double ShouldersTwistRadians = HipsTwistRadians + ChestTwistRadians;
        public static readonly Offset ButtFromTheChest = new Offset(0.145, 0.27, 0.08);

        public static double RifleDownFromTheChest(double muzzleDownRadians)
        {
            return muzzleDownRadians - TorsoLeanRadians;
        }

        public static Offset FromTheChest(Offset onTheRifle, double rifleDownRadians)
        {
            var pitched = onTheRifle.TurnedAbout(Offset.East, rifleDownRadians);
            return ButtFromTheChest + pitched.TurnedAbout(Offset.Up, -ShouldersTwistRadians);
        }

        public static FigurePose At(double muzzleDownRadians)
        {
            var body = FigurePose.Upright.WithHips(new Offset(StepAcrossMetres, -SoftKneesMetres, StepForwardMetres))
                .With(FigurePart.Hips, new JointAngles(Level, Level, HipsTwistRadians))
                .With(FigurePart.Chest, new JointAngles(TorsoLeanRadians, ChestTiltRadians, ChestTwistRadians))
                .With(FigurePart.Head, new JointAngles(CheekDownRadians, CheekTiltRadians, -ShouldersTwistRadians));
            var standing = Legs.Planted(body, LeftFoot, RightFoot);
            var rifleDown = RifleDownFromTheChest(muzzleDownRadians);
            var holding = Arms.Reaching(standing, FigurePart.RightHand, OnTheRifle(standing, GripFromTheButt, rifleDown), RightElbowOutAndUp);
            var supporting = Arms.Reaching(holding, FigurePart.LeftHand, OnTheRifle(holding, ForeEndFromTheButt, rifleDown), LeftElbowDown);
            return Skirts.Following(supporting);
        }

        private static WorldPoint OnTheRifle(FigurePose pose, Offset onTheRifle, double rifleDownRadians)
        {
            var frames = FigurePlacement.Of(pose);
            var chest = frames[FigurePart.Chest];
            return chest.Place(FromTheChest(onTheRifle, rifleDownRadians));
        }
    }
}
