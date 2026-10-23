using System.Collections.Generic;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Mooring;
using ShallowWater.Unity.Helm;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class BoatEntry
    {
        private const string Heading = "SPARROW";
        private const string BoatName = "Sparrow";
        private const string BoatNote = "1912 Josher motor, single-cylinder Bolinder";
        private const string AtTheHelm = "Askew at the tiller";
        private const string MadeFast = "made fast";
        private const string LeftAtTheBank = "left at the bank";
        private const string EngineName = "Engine";
        private const string EngineNote = "Where the lever stands";
        private const string CentreLineName = "Centre line";
        private const string BowLineName = "Bow line";
        private const string SternLineName = "Stern line";
        private const string LineNote = "Eleven metres of hemp";
        private const string Made = "on its post";
        private const string Loose = "loose";

        private static readonly Dictionary<CentreLineHold, string> CentreLineWords = new Dictionary<CentreLineHold, string>
        {
            { CentreLineHold.OnTheRoof, "coiled on the roof" },
            { CentreLineHold.InHand, "in his hand" },
            { CentreLineHold.OnThePost, "on its post" }
        };

        private readonly PageEntry boat = new PageEntry(Sketches.Boat());
        private readonly PageEntry engine = new PageEntry(Sketches.Flywheel());
        private readonly PageEntry centreLine = new PageEntry(Sketches.Rope());
        private readonly PageEntry bowLine = new PageEntry(Sketches.Rope());
        private readonly PageEntry sternLine = new PageEntry(Sketches.Rope());

        public BoatEntry(VisualElement page)
        {
            var section = PageElements.Section(page, Heading);
            section.Add(boat.Row);
            section.Add(engine.Row);
            section.Add(centreLine.Row);
            section.Add(bowLine.Row);
            section.Add(sternLine.Row);
        }

        public void Show(ThrottleNotch lever, Mooring mooring, bool isAshore)
        {
            boat.Write(BoatName, BoatNote, WhereSheLies(mooring, isAshore));
            engine.Write(EngineName, EngineNote, NotchNames.Of(lever));
            centreLine.Write(CentreLineName, LineNote, CentreLineWords[mooring.Hold]);
            bowLine.Write(BowLineName, LineNote, mooring.IsMade(MooringLine.Bow) ? Made : Loose);
            sternLine.Write(SternLineName, LineNote, mooring.IsMade(MooringLine.Stern) ? Made : Loose);
        }

        private static string WhereSheLies(Mooring mooring, bool isAshore)
        {
            if (!isAshore) return AtTheHelm;
            return mooring.IsFast ? MadeFast : LeftAtTheBank;
        }
    }
}
