using System;
using System.Collections.Generic;
using ShallowWater.Game.Saving;
using ShallowWater.Unity.Overlay;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class MenuPage
    {
        private const string MenuClass = "title-menu";
        private const string NewGame = "New game";
        private const string NewGameDetail = "Moored at Hopwas bridge, half past eight";
        private const string Continuing = "Continue";
        private const string Loading = "Load";
        private const string Crediting = "Credits";
        private const string Quitting = "Quit";
        private const string OneSavedGame = "One saved game";
        private const int One = 1;

        private static bool CanQuit => Application.platform != RuntimePlatform.WebGLPlayer;

        public static VisualElement Of(SavedGames saves, Action<GameStart> begin, TitlePages pages)
        {
            var menu = OverlayElements.Block(MenuClass);
            foreach (var choice in Choices(saves, begin, pages)) menu.Add(choice);
            return TitlePage.Front(menu);
        }

        private static IEnumerable<Button> Choices(SavedGames saves, Action<GameStart> begin, TitlePages pages)
        {
            if (saves.HasAny) yield return Continue(saves.Latest, begin);
            yield return TitleButton.Of(NewGame, NewGameDetail, () => begin(GameStart.Fresh));
            if (saves.HasAny) yield return Load(saves, begin, pages);
            yield return Credits(pages);
            if (CanQuit) yield return TitleButton.Of(Quitting, Application.Quit);
        }

        private static Button Continue(SavedGame latest, Action<GameStart> begin)
        {
            var detail = $"{latest.Whereabouts} · {SaveAge.Words(latest.SavedAt, DateTime.UtcNow)}";
            return TitleButton.Of(Continuing, detail, () => begin(GameStart.From(latest)));
        }

        private static Button Credits(TitlePages pages)
        {
            return TitleButton.Of(Crediting, () => pages.Open(CreditsPage.Of(pages.Back)));
        }

        private static Button Load(SavedGames saves, Action<GameStart> begin, TitlePages pages)
        {
            var count = saves.Count;
            var isOne = count == One;
            var detail = isOne ? OneSavedGame : $"{count} saved games";
            return TitleButton.Of(Loading, detail, () => pages.Open(SavedGamesPage.Of(saves, begin, pages.Back)));
        }
    }
}
