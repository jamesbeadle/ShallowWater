using ShallowWater.Game.Pound;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class WaterEntry
    {
        private const string Heading = "THE WATER";
        private const string PoundName = "The pound";
        private const string PoundNote = "Huddlesford to Glascote, not a lock between";
        private const string WordsClass = "page-water__words";

        private readonly PageEntry pound = new PageEntry(Sketches.Reeds());
        private readonly Label words = PageElements.Pencil(string.Empty, WordsClass);

        public WaterEntry(VisualElement page)
        {
            var section = PageElements.Section(page, Heading);
            section.Add(pound.Row);
            section.Add(words);
        }

        public void Show(Whereabouts whereabouts, string poundLength)
        {
            pound.Write(PoundName, PoundNote, poundLength);
            words.text = whereabouts.Words;
        }
    }
}
