using System;
using ShallowWater.Game.Saving;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class LoadList
    {
        private const string ListClass = "title-saves";
        private const string ScrollClass = "title-saves__scroll";
        private const string HeadingWords = "Saved games";
        private const string Autosaved = "Autosave";
        private const string SavedByHand = "Saved";

        public static VisualElement Of(SavedGames saves, Action<GameStart> begin, Action back)
        {
            var list = OverlayElements.Block(ListClass);
            list.Add(OverlayElements.Eyebrow(HeadingWords));
            var scroll = new ScrollView(ScrollViewMode.Vertical);
            scroll.AddToClassList(ScrollClass);
            foreach (var saved in saves.NewestFirst) scroll.Add(Entry(saved, begin));
            list.Add(scroll);
            list.Add(TitleButton.Of("Back", back));
            list.RegisterCallback<NavigationCancelEvent>(_ => back());
            return list;
        }

        private static Button Entry(SavedGame saved, Action<GameStart> begin)
        {
            var kind = saved.IsAutosave ? Autosaved : SavedByHand;
            var detail = $"{kind} · {SaveAge.Words(saved.SavedAt, DateTime.UtcNow)}";
            return TitleButton.Of(saved.Whereabouts, detail, () => begin(GameStart.From(saved)));
        }
    }
}
