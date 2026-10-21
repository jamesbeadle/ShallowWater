using ShallowWater.Game.Missions;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class JobCard
    {
        private const string RowClass = "hud-job-row";
        private const string CardClass = "hud-job";
        private const string HiddenClass = "hud-job--hidden";
        private const string WordsClass = "hud-job__words";

        private readonly VisualElement card = OverlayElements.Glass(CardClass, HiddenClass);
        private readonly Label words = OverlayElements.Words(string.Empty, WordsClass);

        public JobCard(VisualElement screen, string heading)
        {
            var row = OverlayElements.Block(RowClass);
            row.pickingMode = PickingMode.Ignore;
            card.pickingMode = PickingMode.Ignore;
            card.Add(OverlayElements.Eyebrow(heading.ToUpperInvariant()));
            card.Add(words);
            row.Add(card);
            screen.Add(row);
        }

        public void Show(FarmRifleMission mission)
        {
            card.EnableInClassList(HiddenClass, !mission.HasBegun);
            words.text = mission.Words;
        }
    }
}
