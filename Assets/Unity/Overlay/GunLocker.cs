using ShallowWater.Game.Guns;
using ShallowWater.Game.Kit;
using ShallowWater.Game.Missions;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class GunLocker
    {
        private const string Heading = "GUNS";
        private const string RookRifle = "Rook rifle";
        private const string RookRifleNote = "Single shot, " + Cartridges.Calibre;
        private const string EmptyLocker = "Nothing";
        private const string EmptyLockerNote = "Three hooks and nothing on them";
        private const string InHand = "in hand";
        private const string StowedBelow = "stowed below";
        private const string Loaded = "loaded";
        private const string BoltOpen = "bolt open";
        private const string LentByTheFarmer = "lent by " + FarmerLines.Name;
        private const string HisOwn = "his own";
        private const string Comma = ", ";
        private const int SpareHooks = 2;

        private readonly PageEntry rifle = new PageEntry(Sketches.Rifle());

        public GunLocker(VisualElement page)
        {
            var section = PageElements.Section(page, Heading);
            section.Add(rifle.Row);
            for (var hook = 0; hook < SpareHooks; hook++) section.Add(SpareHook());
        }

        public void Show(FarmRifleMission mission, RookRifle gun, bool isAshore)
        {
            var isInTheLocker = mission.IsRifleInHand;
            rifle.Fade(!isInTheLocker);
            if (!isInTheLocker) rifle.Write(EmptyLocker, EmptyLockerNote, string.Empty);
            if (!isInTheLocker) return;
            var where = isAshore ? InHand : StowedBelow;
            var state = gun.IsLoaded ? Loaded : BoltOpen;
            var whose = mission.IsRifleKept ? HisOwn : LentByTheFarmer;
            rifle.Write(RookRifle, RookRifleNote, where + Comma + state + Comma + whose);
        }

        private static VisualElement SpareHook()
        {
            var hook = new PageEntry(Sketches.Hook());
            hook.Fade(true);
            return hook.Row;
        }
    }
}
