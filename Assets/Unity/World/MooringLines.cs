using System;
using System.Collections.Generic;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Mooring;
using ShallowWater.Game.Shapes;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class MooringLines
    {
        private const string DrapedName = "Centre line on the roof";
        private readonly RopeMesh draped;
        private readonly Dictionary<MooringLine, RopeMesh> ropes = new Dictionary<MooringLine, RopeMesh>();
        private readonly MooringPins pins = new MooringPins();
        private readonly SurfaceBeneath beneath;

        public MooringLines(Transform boatModel, SurfaceBeneath surfaceBeneath)
        {
            beneath = surfaceBeneath;
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
            var stud = LineEnds.OnTheBoat(MooringLine.Centre, boat, hand.Place);
            rope.Lay(CentreLineShape(mooring, stud, hand));
        }

        private Shape CentreLineShape(Mooring mooring, LineEnd stud, WorldPoint hand)
        {
            var isHeldAtThePost = mooring.IsLaid(MooringLine.Centre) || IsWorkingOn(mooring, MooringLine.Centre);
            if (!isHeldAtThePost) return MooringLineShape.InHand(stud, hand, beneath);
            var laid = mooring.LaidAt(MooringLine.Centre);
            return MooringLineShape.MadeFast(stud, laid, beneath);
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
            rope.Lay(MooringLineShape.MadeFast(onTheBoat, laid, beneath));
        }

        private static bool IsWorkingOn(Mooring mooring, MooringLine line)
        {
            var work = mooring.Work;
            return work != null && work.Line == line;
        }
    }
}
