using ShallowWater.Game.Boat;
using ShallowWater.Unity.Helm;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class TelegraphCard
    {
        private const string Eyebrow = "ENGINE";
        private const string Astern = "ASTERN";
        private const string Ahead = "AHEAD";
        private const string CardClass = "hud-telegraph";
        private const string HiddenClass = "hud-telegraph--hidden";
        private const string OrderClass = "hud-order";
        private const string OrderAheadClass = "hud-order--ahead";
        private const string OrderAsternClass = "hud-order--astern";
        private const string EndsClass = "hud-ends";
        private const string EndWordClass = "hud-ends__word";

        private readonly VisualElement card = OverlayElements.Glass(CardClass);
        private readonly Label order = OverlayElements.Heading(string.Empty, OrderClass);
        private readonly TelegraphMeter meter = new TelegraphMeter();

        public TelegraphCard(VisualElement screen)
        {
            var place = card.style;
            place.left = HudLayout.BesideTheRadarUnits;
            card.Add(OverlayElements.Eyebrow(Eyebrow));
            card.Add(order);
            card.Add(meter.Meter);
            card.Add(Ends());
            screen.Add(card);
        }

        public void Show(ThrottleNotch lever)
        {
            order.text = NotchNames.Of(lever);
            order.EnableInClassList(OrderAheadClass, lever > ThrottleNotch.Stop);
            order.EnableInClassList(OrderAsternClass, lever < ThrottleNotch.Stop);
            meter.Show(lever);
        }

        public void Reveal(bool isAtTheHelm)
        {
            card.EnableInClassList(HiddenClass, !isAtTheHelm);
        }

        private static VisualElement Ends()
        {
            var ends = OverlayElements.Block(EndsClass);
            ends.Add(OverlayElements.Heading(Astern, EndWordClass));
            ends.Add(OverlayElements.Heading(Ahead, EndWordClass));
            return ends;
        }
    }
}
