using System;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public readonly struct HelmStance
    {
        private const double StandingToPortMetres = -0.1;
        private const double StandingForwardOfTheHandleMetres = 0.2;
        private const double StepAcrossMetres = 0.12;
        private const double SwayRadians = 0.35;
        private const double LeanBackRadians = 0.2;
        private const double DipRadians = 0.45;
        private const double KneesTwiceTheDip = 2;
        private const double LegMetres = AskewForm.ThighMetres + AskewForm.ShinMetres;
        private const double Level = 0;
        private const double Whole = 1;
        private static readonly Offset ShoulderFromTheHips = new Offset(AskewForm.ShoulderHalfWidthMetres, AskewForm.ShoulderMetres - AskewForm.HipMetres, Level);
        private static readonly Offset Right = Offset.East;
        private static readonly Offset Forward = new Offset(Level, Level, 1);

        public static readonly WorldPoint Feet = new WorldPoint(StandingToPortMetres, SparrowForm.HatchFloorMetres, SternGear.HandleEndAlong + StandingForwardOfTheHandleMetres);

        private HelmStance(double rudder)
        {
            StepAcross = -rudder * StepAcrossMetres;
            Sway = -rudder * SwayRadians;
            Lean = -Math.Abs(rudder) * LeanBackRadians;
            Thigh = Math.Abs(rudder) * DipRadians;
            Bob = LegMetres * (Math.Cos(Thigh) - Whole);
        }

        public double StepAcross { get; }
        public double Sway { get; }
        public double Lean { get; }
        public double Thigh { get; }
        public double Shin => -Thigh * KneesTwiceTheDip;
        public double Bob { get; }

        public static HelmStance For(double rudder)
        {
            return new HelmStance(rudder);
        }

        public Offset RightShoulderFromTheFeet()
        {
            var hips = new Offset(StepAcross, AskewForm.HipMetres + Bob, Level);
            return hips + ShoulderFromTheHips.TurnedAbout(Right, Lean).TurnedAbout(Forward, -Sway);
        }

        public Offset InTheBodyFrame(Offset fromTheShoulder)
        {
            return fromTheShoulder.TurnedAbout(Forward, Sway).TurnedAbout(Right, -Lean);
        }
    }
}
