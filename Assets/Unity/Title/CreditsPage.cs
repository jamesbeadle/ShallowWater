using System;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class CreditsPage
    {
        private const string CreditsClass = "title-credits";
        private const string CreditClass = "title-credit";
        private const string NameClass = "title-credit__name";
        private const string WordsClass = "title-credit__words";
        private const string HeadingWords = "Credits";

        private static readonly (string Name, string Words)[] Credits =
        {
            ("The ground", "Ordnance Survey one-inch map, revised 1898 to 1904, reproduced with the permission of the National Library of Scotland."),
            ("The survey", "Canal, roads, woods and buildings © OpenStreetMap contributors, under the Open Database Licence."),
            ("The lettering", "League Gothic, Stardos Stencil, Caveat, Cabin and Josefin Sans, each under the SIL Open Font Licence.")
        };

        public static VisualElement Of(Action back)
        {
            var credits = OverlayElements.Block(CreditsClass);
            foreach (var credit in Credits) credits.Add(Entry(credit.Name, credit.Words));
            return TitlePage.Inside(HeadingWords, credits, back);
        }

        private static VisualElement Entry(string name, string words)
        {
            var credit = OverlayElements.Block(CreditClass);
            credit.Add(OverlayElements.Words(name.ToUpperInvariant(), NameClass));
            credit.Add(OverlayElements.Words(words, WordsClass));
            return credit;
        }
    }
}
