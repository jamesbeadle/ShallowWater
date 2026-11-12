using System;
using ShallowWater.Game.Saving;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class SavedGamesPage
    {
        private const string ListClass = "title-saves";
        private const string HeadingWords = "Saved games";
        private const string Autosaved = "Autosave";
        private const string SavedByHand = "Saved";

        public static VisualElement Of(SavedGames saves, Action<GameStart> begin, Action back)
        {
            var list = OverlayElements.Block(ListClass);
            foreach (var saved in saves.NewestFirst) list.Add(Entry(saved, begin));
            return TitlePage.Inside(HeadingWords, list, back);
        }

        private static Button Entry(SavedGame saved, Action<GameStart> begin)
        {
            var kind = saved.IsAutosave ? Autosaved : SavedByHand;
            var detail = $"{kind} · {SaveAge.Words(saved.SavedAt, DateTime.UtcNow)}";
            return TitleButton.Of(saved.Whereabouts, detail, () => begin(GameStart.From(saved)));
        }
    }
}
