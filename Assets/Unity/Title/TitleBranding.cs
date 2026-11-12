using System.Linq;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class TitleBranding
    {
        private const string Ground = "Huddlesford to Fazeley · Autumn 1938";
        private const string SeriesName = "Shallow Water";
        private const string ChapterName = "Locked";
        private const string Tagline = "You can wade it. You cannot see the bottom.";
        private const char BetweenWords = ' ';
        private const string LockupClass = "title__lockup";
        private const string NameClass = "title__name";
        private const string SecondLineClass = "title__second-line";
        private const string ChapterClass = "title__chapter";
        private const string TaglineClass = "title__tagline";

        public static string RunningHead => $"{SeriesName} · {ChapterName}".ToUpperInvariant();

        public static VisualElement Lockup()
        {
            var words = SeriesName.ToUpperInvariant().Split(BetweenWords);
            var lockup = OverlayElements.Block(LockupClass);
            lockup.Add(OverlayElements.Eyebrow(Ground.ToUpperInvariant()));
            lockup.Add(OverlayElements.Words(words.First(), NameClass));
            lockup.Add(SecondLine(words.Last()));
            lockup.Add(OverlayElements.Words(Tagline, TaglineClass));
            return lockup;
        }

        private static VisualElement SecondLine(string word)
        {
            var line = OverlayElements.Block(SecondLineClass);
            line.Add(OverlayElements.Words(word, NameClass));
            line.Add(OverlayElements.Words(ChapterName, ChapterClass));
            return line;
        }
    }
}
