using System;
using ShallowWater.Game.Saving;
using ShallowWater.Unity.Overlay;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public static class TitleMenu
    {
        private const string MenuClass = "title-menu";
        private const string NewGame = "New game";
        private const string NewGameDetail = "Moored at Hopwas bridge, half past eight";
        private const string Continuing = "Continue";
        private const string Loading = "Load";
        private const string Quitting = "Quit";
        private const string NoSavesYet = "Nothing saved yet";
        private const string OneSavedGame = "One saved game";
        private const int One = 1;

        private static bool CanQuit => Application.platform != RuntimePlatform.WebGLPlayer;

        public static VisualElement Of(SavedGames saves, Action<GameStart> begin, Action showTheSaves)
        {
            var menu = OverlayElements.Block(MenuClass);
            menu.Add(TitleButton.Of(NewGame, NewGameDetail, () => begin(GameStart.Fresh)));
            menu.Add(Continue(saves, begin));
            menu.Add(Load(saves, showTheSaves));
            if (!CanQuit) return menu;
            var quit = TitleButton.Of(Quitting, Application.Quit);
            menu.Add(quit);
            return menu;
        }

        private static Button Continue(SavedGames saves, Action<GameStart> begin)
        {
            if (!saves.HasAny) return TitleButton.Unavailable(Continuing, NoSavesYet);
            var latest = saves.Latest;
            var detail = $"{latest.Whereabouts} · {SaveAge.Words(latest.SavedAt, DateTime.UtcNow)}";
            return TitleButton.Of(Continuing, detail, () => begin(GameStart.From(latest)));
        }

        private static Button Load(SavedGames saves, Action showTheSaves)
        {
            if (!saves.HasAny) return TitleButton.Unavailable(Loading, NoSavesYet);
            var count = saves.Count;
            var isOne = count == One;
            var detail = isOne ? OneSavedGame : $"{count} saved games";
            return TitleButton.Of(Loading, detail, showTheSaves);
        }
    }
}
