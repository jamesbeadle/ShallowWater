using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Mooring
{
    public sealed class Mooring
    {
        private readonly Dictionary<MooringLine, LaidLine> laid = new Dictionary<MooringLine, LaidLine>();
        private LineWork work;

        public CentreLineHold Hold { get; private set; } = CentreLineHold.OnTheRoof;
        public HandOnTheLine Hand { get; } = new HandOnTheLine();
        public LineWork Work => work;
        public bool IsBusy => work != null;
        public bool IsLineInHand => Hold == CentreLineHold.InHand && !Hand.IsLaidDown;
        public bool IsLineLaidDown => Hold == CentreLineHold.InHand && Hand.IsLaidDown;
        public bool IsCentreLineOnThePost => Hold == CentreLineHold.OnThePost;
        public bool IsFast => IsCentreLineOnThePost && IsMade(MooringLine.Bow) && IsMade(MooringLine.Stern);
        public bool AreTheEndsLoose => !IsMade(MooringLine.Bow) && !IsMade(MooringLine.Stern);
        public IEnumerable<MooringLine> LinesLaid => laid.Keys;

        public bool IsMade(MooringLine line)
        {
            return laid.ContainsKey(line) && !IsWorkingOn(line);
        }

        public bool IsLaid(MooringLine line)
        {
            return IsWorkingOn(line) ? work.IsLineMade : laid.ContainsKey(line);
        }

        public LaidLine LaidAt(MooringLine line)
        {
            return laid[line];
        }

        public bool CanMakeFast(MooringLine line)
        {
            if (IsBusy) return false;
            var isTheCentreLine = line == MooringLine.Centre;
            if (isTheCentreLine) return IsLineInHand;
            return IsCentreLineOnThePost && !laid.ContainsKey(line);
        }

        public bool CanLetGo(MooringLine line)
        {
            if (IsBusy || !laid.ContainsKey(line)) return false;
            var isTheCentreLine = line == MooringLine.Centre;
            return !isTheCentreLine || AreTheEndsLoose;
        }

        public void TakeAshore(GroundPoint hand)
        {
            var isOnTheRoof = Hold == CentreLineHold.OnTheRoof;
            if (!isOnTheRoof) return;
            Hold = CentreLineHold.InHand;
            Hand.Rest(hand);
        }

        public void TakeAboard()
        {
            if (!IsLineInHand || IsBusy) return;
            Hold = CentreLineHold.OnTheRoof;
        }

        public void MakeFast(MooringLine line, LaidLine onThePost)
        {
            if (!CanMakeFast(line)) return;
            laid[line] = onThePost;
            work = LineWork.MakingFast(line);
        }

        public void LetGo(MooringLine line)
        {
            if (!CanLetGo(line)) return;
            work = LineWork.LettingGo(line);
        }

        public void WorkOn(double seconds)
        {
            if (!IsBusy) return;
            work.Work(seconds);
            if (!work.IsDone) return;
            if (work.IsLettingGo) laid.Remove(work.Line);
            var isTheCentreLine = work.Line == MooringLine.Centre;
            if (isTheCentreLine) Hold = work.IsMakingFast ? CentreLineHold.OnThePost : CentreLineHold.InHand;
            work = null;
        }

        private bool IsWorkingOn(MooringLine line)
        {
            return IsBusy && work.Line == line;
        }
    }
}
