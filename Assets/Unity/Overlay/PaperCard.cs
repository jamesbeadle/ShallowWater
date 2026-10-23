using ShallowWater.Game.Missions;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class PaperCard
    {
        private const string Heading = "PAPERS";
        private const string NothingInWriting = "Nothing in writing.";
        private const string SentBy = "Say " + FarmerLines.Name + " sent me.";
        private const string Comma = ", ";
        private const string CardClass = "paper-card";
        private const string HiddenClass = "paper-card--hidden";
        private const string LineClass = "paper-card__line";
        private const string NothingClass = "paper-nothing";
        private const string NothingHiddenClass = "paper-nothing--hidden";

        private readonly VisualElement card = OverlayElements.Block(CardClass, HiddenClass);
        private readonly Label nothing = PageElements.Note(NothingInWriting, NothingClass);
        private readonly Label who = PageElements.Pencil(string.Empty, LineClass);
        private readonly Label telephone = PageElements.Pencil(string.Empty, LineClass);
        private readonly Label sentBy = PageElements.Pencil(SentBy, LineClass);

        public PaperCard(VisualElement page)
        {
            var section = PageElements.Section(page, Heading);
            section.Add(nothing);
            card.Add(who);
            card.Add(telephone);
            card.Add(sentBy);
            section.Add(card);
        }

        public void Show(FarmRifleMission mission)
        {
            var hasTheLead = mission.HasTheLead;
            card.EnableInClassList(HiddenClass, !hasTheLead);
            nothing.EnableInClassList(NothingHiddenClass, hasTheLead);
            if (!hasTheLead) return;
            var lead = mission.Lead;
            who.text = lead.Name + Comma + lead.Pub + Comma + lead.Place;
            telephone.text = lead.Telephone;
        }
    }
}
