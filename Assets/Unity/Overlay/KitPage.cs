using ShallowWater.Game.Day;
using ShallowWater.Game.Guns;
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
        private readonly CartridgeBox cartridges;
        private readonly Pockets pockets;
        private readonly PaperCard papers;

        public KitPage(VisualElement page)
        {
            page.Add(PageElements.Heading(Heading, Owner));
            locker = new GunLocker(page);
            cartridges = new CartridgeBox(page);
            pockets = new Pockets(page);
            papers = new PaperCard(page);
        }

        public void Show(FarmRifleMission mission, RookRifle rifle, bool isAshore, Kit kit, TimeOfDay time)
        {
            locker.Show(mission, rifle, isAshore);
            cartridges.Show(kit.Cartridges, mission.IsRifleInHand);
            pockets.Show(kit.Purse, time);
            papers.Show(mission);
        }
    }
}
