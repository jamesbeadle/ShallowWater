using System.Collections.Generic;
using ShallowWater.Game.Boat;

namespace ShallowWater.Game.People
{
    public static class TillerHold
    {
        private const double SlideArm = 0.34;
        private const double SlideForearm = 1.01;
        private const double Upright = 0;
        private const double HandReachMetres = AskewForm.ForearmMetres + Hand.PalmBelowTheWristMetres;

        public static GaitPose At(double rudder)
        {
            var stance = HelmStance.For(rudder);
            var steering = SteeringArm(rudder, stance);
            var thigh = stance.Thigh + stance.Lean;
            var shin = stance.Shin - Stride.StandingKnee;
            var swings = new Dictionary<FigurePart, double>
            {
                { FigurePart.Body, Upright }, { FigurePart.Head, Upright },
                { FigurePart.LeftThigh, thigh }, { FigurePart.RightThigh, thigh },
                { FigurePart.LeftShin, shin }, { FigurePart.RightShin, shin },
                { FigurePart.LeftUpperArm, SlideArm }, { FigurePart.RightUpperArm, steering.SwingRadians },
                { FigurePart.LeftForearm, SlideForearm }, { FigurePart.RightForearm, steering.ElbowRadians }
            };
            var reaches = new Dictionary<FigurePart, double>
            {
                { FigurePart.LeftThigh, stance.Sway }, { FigurePart.RightThigh, stance.Sway }, { FigurePart.RightUpperArm, steering.ReachRadians }
            };
            return new GaitPose(swings, stance.Bob, stance.Lean, reaches, stance.Sway, stance.StepAcross);
        }

        private static ArmAngles SteeringArm(double rudder, HelmStance stance)
        {
            var shoulder = stance.RightShoulderFromTheFeet();
            var grip = TillerGrip.FromTheFeet(Tiller.TurnedRadians(rudder), shoulder);
            var hand = stance.InTheBodyFrame(grip - shoulder);
            return ArmReach.Towards(hand, AskewForm.UpperArmMetres, HandReachMetres);
        }
    }
}
