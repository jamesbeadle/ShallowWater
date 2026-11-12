using System;
using ShallowWater.Game.Saving;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public sealed class TitleScreen
    {
        private const string ScreenClass = "title-screen";
        private const string LeavingClass = "title-screen--leaving";
        private const string WashClass = "title__wash";
        private const string ShadeClass = "title__shade";
        private const string StageClass = "title__stage";
        private const string CoverClass = "title__cover";
        private const string ColumnClass = "title__column";
        private const string PanelClass = "title__panel";
        private const string CastingOffClass = "title__casting-off";
        private const string CastingOffWords = "Casting off…";
        private const long PaintTheCastingOffMilliseconds = 80;
        private const long FadeMilliseconds = 450;

        private readonly VisualElement title = OverlayElements.Block(ScreenClass);
        private readonly VisualElement panel = OverlayElements.Block(PanelClass);
        private readonly SavedGames saves;
        private readonly Action<GameStart> begin;

        public TitleScreen(VisualElement screen, SavedGames savedGames, Action<GameStart> beginTheGame)
        {
            saves = savedGames;
            begin = beginTheGame;
            title.Add(OverlayElements.Block(WashClass));
            title.Add(OverlayElements.Block(ShadeClass));
            title.Add(Stage());
            var table = screen.parent;
            table.Add(title);
            ShowTheMenu();
        }

        private VisualElement Stage()
        {
            var stage = OverlayElements.Block(StageClass);
            stage.Add(OverlayElements.Block(CoverClass));
            var column = TitleBranding.Column(ColumnClass);
            column.Add(panel);
            stage.Add(column);
            return stage;
        }

        private void ShowTheMenu()
        {
            Show(TitleMenu.Of(saves, Begin, ShowTheSaves));
        }

        private void ShowTheSaves()
        {
            Show(LoadList.Of(saves, Begin, ShowTheMenu));
        }

        private void Show(VisualElement choices)
        {
            panel.Clear();
            panel.Add(choices);
            var first = choices.Query<Button>().Where(button => button.focusable).First();
            panel.schedule.Execute(() => first?.Focus());
        }

        private void Begin(GameStart start)
        {
            panel.Clear();
            panel.Add(OverlayElements.Words(CastingOffWords, CastingOffClass));
            title.schedule.Execute(() => CastOff(start)).ExecuteLater(PaintTheCastingOffMilliseconds);
        }

        private void CastOff(GameStart start)
        {
            begin(start);
            title.AddToClassList(LeavingClass);
            title.schedule.Execute(title.RemoveFromHierarchy).ExecuteLater(FadeMilliseconds);
        }
    }
}
