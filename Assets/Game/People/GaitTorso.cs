using System;

namespace ShallowWater.Game.People
{
    public static class GaitTorso
    {
        private const double ShouldersTwistRadians = 0.06;
        private const double CounterRollShare = 0.8;
        private const double WalkLeanRadians = 0.03;
        private const double RunLeanRadians = 0.12;
        private const double StepPitchRadians = 0.015;
        private const double HeadSteadyShare = 0.7;
        private const double HeadFacesForwardShare = 0.8;
        private const double TwiceACycle = 2;
        private const double FullTurnRadians = 2 * Math.PI;

        public static FigurePose On(FigurePose pose, double cycleShare, GaitRhythm rhythm)
        {
            var turn = cycleShare * FullTurnRadians;
            var hipsTwist = GaitHips.TwistAt(cycleShare, rhythm);
            var hipsRoll = GaitHips.RollAt(cycleShare, rhythm);
            var shouldersTwist = -ShouldersTwistRadians * Math.Cos(turn) * rhythm.Going;
            var chestRoll = -hipsRoll * CounterRollShare;
            var lean = rhythm.Between(WalkLeanRadians, RunLeanRadians) * rhythm.Going;
            var chestSwing = lean + StepPitchRadians * Math.Cos(TwiceACycle * turn) * rhythm.Going;
            var chest = new JointAngles(chestSwing, chestRoll, shouldersTwist - hipsTwist);
            var head = new JointAngles(-chestSwing * HeadSteadyShare, -(hipsRoll + chestRoll), -shouldersTwist * HeadFacesForwardShare);
            return pose.With(FigurePart.Chest, chest).With(FigurePart.Head, head);
        }
    }
}
