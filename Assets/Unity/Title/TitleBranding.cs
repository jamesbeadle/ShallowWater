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
        private const string NameClass = "title__name";
        private const string ChapterClass = "title__chapter";
        private const string TaglineClass = "title__tagline";
        private const string RuleClass = "title__rule";

        public static VisualElement Column(string columnClass)
        {
            var column = OverlayElements.Block(columnClass);
            column.Add(OverlayElements.Eyebrow(Ground));
            column.Add(OverlayElements.Heading(SeriesName, NameClass));
            column.Add(OverlayElements.Words(ChapterName, ChapterClass));
            column.Add(OverlayElements.Words(Tagline, TaglineClass));
            column.Add(OverlayElements.Block(RuleClass));
            return column;
        }
    }
}
