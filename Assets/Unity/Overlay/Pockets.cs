using ShallowWater.Game.Day;
using ShallowWater.Game.Kit;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class Pockets
    {
        private const string Heading = "POCKETS";
        private const string PurseName = "Purse";
        private const string PurseNote = "What the pension left";
        private const string WatchName = "Pocket watch";
        private const string WatchNote = "Wound this morning";

        private readonly PageEntry purse = new PageEntry(Sketches.Coins());
        private readonly PocketWatch watch = new PocketWatch();
        private readonly PageEntry watchEntry;

        public Pockets(VisualElement page)
        {
            watchEntry = new PageEntry(watch.Face);
            var section = PageElements.Section(page, Heading);
            section.Add(purse.Row);
            section.Add(watchEntry.Row);
        }

        public void Show(Purse money, TimeOfDay time)
        {
            purse.Write(PurseName, PurseNote, money.Words);
            watch.Show(time);
            watchEntry.Write(WatchName, WatchNote, time.ToString());
        }
    }
}
