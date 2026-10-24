using ShallowWater.Game.Day;
using ShallowWater.Game.Kit;
using ShallowWater.Game.Missions;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class KitPage
    {
        private const string Heading = "KIT";
        private const string Owner = "F. Askew, Sparrow";

        private readonly GunLocker locker;
        private readonly CartridgeBoxes cartridges;
        private readonly Pockets pockets;
        private readonly PaperCard papers;

        public KitPage(VisualElement page, Kit kit)
        {
            page.Add(PageElements.Heading(Heading, Owner));
            locker = new GunLocker(page, kit.Rifles);
            cartridges = new CartridgeBoxes(page, kit.Rifles);
            pockets = new Pockets(page);
            papers = new PaperCard(page);
        }

        public void Show(FarmJob job, Kit kit, bool isAshore, TimeOfDay time)
        {
            locker.Show(kit.Rifles, isAshore);
            cartridges.Show(kit.Rifles);
            pockets.Show(kit.Purse, time);
            papers.Show(job);
        }
    }
}
