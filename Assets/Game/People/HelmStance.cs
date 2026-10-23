using System;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class HelmStance
    {
        private const double StandingToPortMetres = -0.1;
        private const double StandingForwardOfTheHandleMetres = 0.2;
        private const double FeetApartMetres = 0.13;
        private const double LeftFootAheadMetres = 0.05;
        private const double ToeOutRadians = 0.18;
        private const double StepOutRadians = 0.35;
        private const double StepOutMetres = 0.3;
        private const double StepAcrossMetres = 0.28;
        private const double FarFootFollowsShare = 0.4;
        private const double DipMetres = 0.04;
        private const double TiltRadians = 0.12;
        private const double HipsTwistRadians = 0.4;
        private const double ChestTwistRadians = 0.35;
        private const double HeadLooksAheadShare = 0.8;
        private const double HandSwapRudder = 0.05;
        private const double RestingArmRadians = 0.34;
        private const double RestingForearmRadians = 1.01;
        private const double Level = 0;
        private const double Opposite = -1;

        public static readonly WorldPoint Feet = new WorldPoint(StandingToPortMetres, SparrowForm.HatchFloorMetres, SternGear.HandleEndAlong + StandingForwardOfTheHandleMetres);

        public static FigurePart HandFor(double rudder)
        {
            var isTheTillerToPort = rudder > HandSwapRudder;
            return isTheTillerToPort ? FigurePart.LeftHand : FigurePart.RightHand;
        }

        public static FigurePose At(double rudder, double seconds)
        {
            var towardsTheTiller = -rudder;
            var body = Stance.Body(seconds);
            var hips = body.HipsOffset;
            var stepped = new Offset(hips.Eastward + towardsTheTiller * StepAcrossMetres, hips.Upward - Math.Abs(rudder) * DipMetres, hips.Northward);
            var turned = body.WithHips(stepped)
                .With(FigurePart.Hips, body.Of(FigurePart.Hips) + new JointAngles(Level, towardsTheTiller * TiltRadians, towardsTheTiller * HipsTwistRadians))
                .With(FigurePart.Chest, body.Of(FigurePart.Chest) + new JointAngles(Level, Level, towardsTheTiller * ChestTwistRadians))
                .With(FigurePart.Head, body.Of(FigurePart.Head) + new JointAngles(Level, Level, -towardsTheTiller * (HipsTwistRadians + ChestTwistRadians) * HeadLooksAheadShare));
            return Legs.Planted(Resting(turned, rudder), LeftFoot(towardsTheTiller), RightFoot(towardsTheTiller));
        }

        private static FigurePose Resting(FigurePose pose, double rudder)
        {
            var holding = HandFor(rudder);
            var resting = FigureJoints.Opposites[holding];
            var forearm = FigureJoints.Parents[resting];
            var upperArm = FigureJoints.Parents[forearm];
            return pose.With(upperArm, JointAngles.Swung(RestingArmRadians)).With(forearm, JointAngles.Swung(RestingForearmRadians));
        }

        private static FootStep LeftFoot(double towardsTheTiller)
        {
            var stepping = Math.Max(Level, towardsTheTiller * Opposite);
            var following = Math.Max(Level, towardsTheTiller) * StepAcrossMetres * FarFootFollowsShare;
            return FootStep.Flat(-FeetApartMetres - stepping * StepOutMetres + following, LeftFootAheadMetres, ToeOutRadians + stepping * StepOutRadians);
        }

        private static FootStep RightFoot(double towardsTheTiller)
        {
            var stepping = Math.Max(Level, towardsTheTiller);
            var following = Math.Max(Level, towardsTheTiller * Opposite) * StepAcrossMetres * FarFootFollowsShare;
            return FootStep.Flat(FeetApartMetres + stepping * StepOutMetres - following, Level, ToeOutRadians + stepping * StepOutRadians);
        }
    }
}
