using System.Collections.Generic;
using ShallowWater.Game.Guns;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class CartridgeBox
    {
        private const string ABoxOf = "A box of ";
        private const string KeptWithThe = ", kept with the ";
        private const string StripClass = "rounds";
        private const string RoundClass = "round";
        private const string SpentClass = "round--spent";

        private readonly PageEntry box = new PageEntry(Sketches.CartridgeBox());
        private readonly VisualElement strip = OverlayElements.Block(StripClass);
        private readonly List<VisualElement> rounds = new List<VisualElement>();
        private readonly string note;

        public CartridgeBox(VisualElement section, Rifle rifle)
        {
            var cartridges = rifle.Cartridges;
            var rifleName = rifle.Name;
            note = ABoxOf + cartridges.BoxOf + KeptWithThe + rifleName.ToLowerInvariant();
            section.Add(box.Row);
            for (var round = 0; round < cartridges.BoxOf; round++) rounds.Add(Round());
            section.Add(strip);
        }

        public void Show(Cartridges cartridges)
        {
            box.Write(cartridges.Calibre, note, cartridges.Words);
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
