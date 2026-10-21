using ShallowWater.Game.Ground;
using ShallowWater.Game.Mooring;
using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class LineHolder
    {
        private static readonly Vector3 InTheHand = new Vector3(0, -(float)(AskewForm.ForearmMetres + Hand.PalmBelowTheWristMetres), 0);

        private readonly FigureModel figure;
        private readonly WalkerController walking;
        private readonly BoatController boat;

        public LineHolder(FigureModel model, WalkerController walker, BoatController helm)
        {
            figure = model;
            walking = walker;
            boat = helm;
        }

        public WorldPoint Hand { get; private set; }

        public void Hold(Mooring mooring)
        {
            var heldBack = CentreLine.HeldBack(walking.Position, boat.Motion);
            var isHeldBack = !heldBack.IsSamePlaceAs(walking.Position);
            if (isHeldBack) walking.SetOff(heldBack, walking.Bearing);
            var root = figure.Root;
            var feet = WorldVectors.PointOf(root.position);
            var studFromTheFeet = FeetFrame.Of(feet, walking.Bearing, CentreLine.StudOn(boat.Motion));
            figure.Pose(LineInHand.Holding(walking.Stride, studFromTheFeet));
            var wrist = figure.JointOf(LineInHand.ForearmFor(studFromTheFeet));
            Hand = WorldVectors.PointOf(wrist.TransformPoint(InTheHand));
            var held = mooring.Hand;
            held.MoveTo(new GroundPoint(Hand.East, Hand.North), Time.deltaTime);
        }
    }
}
