using System.Collections.Generic;
using ShallowWater.Game.Kit;
using ShallowWater.Game.Missions;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class CartridgeBox
    {
        private const string Heading = "CARTRIDGES";
        private const string BoxNote = "A box of fifty from the gate at " + FarmerLines.Farm;
        private const string NoBox = "None";
        private const string NoBoxNote = "Nothing to put in a gun";
        private const string StripClass = "rounds";
        private const string StripHiddenClass = "rounds--hidden";
        private const string RoundClass = "round";
        private const string SpentClass = "round--spent";

        private readonly PageEntry box = new PageEntry(Sketches.CartridgeBox());
        private readonly VisualElement strip = OverlayElements.Block(StripClass);
        private readonly List<VisualElement> rounds = new List<VisualElement>();

        public CartridgeBox(VisualElement page)
        {
            var section = PageElements.Section(page, Heading);
            section.Add(box.Row);
            for (var round = 0; round < Cartridges.BoxOf; round++) rounds.Add(Round());
            section.Add(strip);
        }

        public void Show(Cartridges cartridges, bool isTheBoxHis)
        {
            box.Fade(!isTheBoxHis);
            strip.EnableInClassList(StripHiddenClass, !isTheBoxHis);
            if (!isTheBoxHis) box.Write(NoBox, NoBoxNote, string.Empty);
            if (!isTheBoxHis) return;
            box.Write(Cartridges.Calibre, BoxNote, cartridges.Words);
            for (var place = 0; place < rounds.Count; place++) rounds[place].EnableInClassList(SpentClass, place >= cartridges.Count);
        }

        private VisualElement Round()
        {
            var round = OverlayElements.Block(RoundClass);
            strip.Add(round);
            return round;
        }
    }
}
