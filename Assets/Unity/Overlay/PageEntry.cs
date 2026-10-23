using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class PageEntry
    {
        private const string EntryClass = "page-entry";
        private const string FaintClass = "page-entry--faint";
        private const string SketchClass = "page-entry__sketch";
        private const string WordsClass = "page-entry__words";
        private const string NameClass = "page-entry__name";
        private const string NoteClass = "page-entry__note";
        private const string PencilClass = "page-entry__pencil";

        private readonly Label name = OverlayElements.Words(string.Empty, NameClass);
        private readonly Label note = PageElements.Note(string.Empty, NoteClass);
        private readonly Label pencil = PageElements.Pencil(string.Empty, PencilClass);

        public PageEntry(VisualElement sketch)
        {
            Row = OverlayElements.Block(EntryClass);
            var frame = OverlayElements.Block(SketchClass);
            frame.Add(sketch);
            Row.Add(frame);
            var words = OverlayElements.Block(WordsClass);
            words.Add(name);
            words.Add(note);
            Row.Add(words);
            Row.Add(pencil);
        }

        public VisualElement Row { get; }

        public void Write(string nameWords, string noteWords, string pencilWords)
        {
            name.text = nameWords;
            note.text = noteWords;
            pencil.text = pencilWords;
        }

        public void Fade(bool isFaint)
        {
            Row.EnableInClassList(FaintClass, isFaint);
        }
    }
}
