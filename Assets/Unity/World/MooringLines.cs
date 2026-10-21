using System;
using System.Collections.Generic;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Mooring;
using ShallowWater.Game.Shapes;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class MooringLines
    {
        private const string DrapedName = "Centre line on the roof";
        private const int RopeSides = 6;
        private readonly RopeMesh draped;
        private readonly Dictionary<MooringLine, RopeMesh> ropes = new Dictionary<MooringLine, RopeMesh>();
        private readonly MooringPins pins = new MooringPins();

        public MooringLines(Transform boatModel)
        {
            draped = new RopeMesh(DrapedName, boatModel);
            draped.Lay(CentreLineDrape.Laid());
            foreach (MooringLine line in Enum.GetValues(typeof(MooringLine))) ropes[line] = new RopeMesh(line.ToString(), null);
        }

        public void Show(Mooring mooring, BoatMotion boat, WorldPoint hand)
        {
            var isOnTheRoof = mooring.Hold == CentreLineHold.OnTheRoof;
            draped.Show(isOnTheRoof);
            ShowTheCentreLine(mooring, boat, hand);
            ShowTheEndLine(mooring, boat, MooringLine.Bow);
            ShowTheEndLine(mooring, boat, MooringLine.Stern);
        }

        private void ShowTheCentreLine(Mooring mooring, BoatMotion boat, WorldPoint hand)
        {
            var rope = ropes[MooringLine.Centre];
            pins.Show(mooring, MooringLine.Centre);
            var isOnTheRoof = mooring.Hold == CentreLineHold.OnTheRoof;
            if (isOnTheRoof) rope.TakeUp();
            if (isOnTheRoof) return;
            var isHeldAtThePost = mooring.IsLaid(MooringLine.Centre) || IsWorkingOn(mooring, MooringLine.Centre);
            var end = isHeldAtThePost ? TieOf(mooring, MooringLine.Centre) : hand;
            rope.Lay(CentreLineShape(boat, end, isHeldAtThePost));
        }

        private static Shape CentreLineShape(BoatMotion boat, WorldPoint end, bool isHeldAtThePost)
        {
            var stud = CentreLine.StudOn(boat);
            var edge = CentreLine.OverTheSideTowards(boat, new GroundPoint(end.East, end.North));
            var onTheBoat = Tube.Along(new[] { stud, edge }, HangingLine.RadiusMetres, RopeSides);
            var slack = CentreLine.SlackBeyond(stud, edge, end);
            onTheBoat.Append(isHeldAtThePost ? HangingLine.TakenUp(edge, end) : HangingLine.WithSlack(edge, end, slack));
            return onTheBoat;
        }

        private void ShowTheEndLine(Mooring mooring, BoatMotion boat, MooringLine line)
        {
            var rope = ropes[line];
            pins.Show(mooring, line);
            var isLaid = mooring.IsLaid(line);
            if (!isLaid) rope.TakeUp();
            if (!isLaid) return;
            var laid = mooring.LaidAt(line);
            var onTheBoat = LineEnds.OnTheBoat(line, boat, laid.Anchor);
            rope.Lay(HangingLine.TakenUp(onTheBoat, laid.Tie));
        }

        private static WorldPoint TieOf(Mooring mooring, MooringLine line)
        {
            var laid = mooring.LaidAt(line);
            return laid.Tie;
        }

        private static bool IsWorkingOn(Mooring mooring, MooringLine line)
        {
            var work = mooring.Work;
            return work != null && work.Line == line;
        }
    }
}
