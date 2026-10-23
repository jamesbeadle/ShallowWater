using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public static class PageElements
    {
        private const string HeadingClass = "page-heading";
        private const string TitleClass = "page-title";
        private const string AsideClass = "page-aside";
        private const string SectionClass = "page-section";
        private const string SectionNameClass = "page-section__name";
        private const string RuleClass = "page-rule";
        private const string StencilClass = "page-stencil";
        private const string PencilClass = "page-pencil";
        private const string NoteClass = "page-note";

        public static VisualElement Heading(string title, string aside)
        {
            var heading = OverlayElements.Block(HeadingClass);
            heading.Add(Stencil(title, TitleClass));
            heading.Add(Pencil(aside, AsideClass));
            return heading;
        }

        public static VisualElement Section(VisualElement page, string name)
        {
            var section = OverlayElements.Block(SectionClass);
            section.Add(Stencil(name, SectionNameClass));
            section.Add(OverlayElements.Block(RuleClass));
            page.Add(section);
            return section;
        }

        public static Label Stencil(string text, params string[] classNames)
        {
            var label = OverlayElements.Words(text, classNames);
            label.AddToClassList(StencilClass);
            return label;
        }

        public static Label Pencil(string text, params string[] classNames)
        {
            var label = OverlayElements.Words(text, classNames);
            label.AddToClassList(PencilClass);
            return label;
        }

        public static Label Note(string text, params string[] classNames)
        {
            var label = OverlayElements.Words(text, classNames);
            label.AddToClassList(NoteClass);
            return label;
        }
    }
}
