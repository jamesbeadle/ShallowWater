using System;
using ShallowWater.Game.Saving;
using ShallowWater.Unity.Overlay;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public sealed class TitleScreen
    {
        private const string ScreenClass = "title-screen";
        private const string ArrivingClass = "title-screen--arriving";
        private const string LeavingClass = "title-screen--leaving";
        private const string CurtainClass = "title__curtain";
        private const string DrawnClass = "title__curtain--drawn";
        private const string CastingOffClass = "title__casting-off";
        private const string CastingOffWords = "Casting off…";
        private const long DrawTheCurtainMilliseconds = 260;
        private const long FadeMilliseconds = 450;

        private readonly VisualElement title = OverlayElements.Block(ScreenClass, ArrivingClass);
        private readonly VisualElement curtain = OverlayElements.Block(CurtainClass);
        private readonly Action<GameStart> begin;
        private bool isCastingOff;

        public TitleScreen(VisualElement screen, SavedGames saves, Action<GameStart> beginTheGame)
        {
            begin = beginTheGame;
            var pages = new TitlePages(title);
            title.Add(TitleFooter.Of());
            curtain.pickingMode = PickingMode.Ignore;
            title.Add(curtain);
            title.RegisterCallback<GeometryChangedEvent>(Arrive);
            var table = screen.parent;
            table.Add(title);
            pages.Open(MenuPage.Of(saves, Begin, pages));
        }

        private void Arrive(GeometryChangedEvent laidOut)
        {
            title.UnregisterCallback<GeometryChangedEvent>(Arrive);
            title.RemoveFromClassList(ArrivingClass);
        }

        private void Begin(GameStart start)
        {
            if (isCastingOff) return;
            isCastingOff = true;
            curtain.Add(OverlayElements.Words(CastingOffWords, CastingOffClass));
            curtain.AddToClassList(DrawnClass);
            title.schedule.Execute(() => CastOff(start)).ExecuteLater(DrawTheCurtainMilliseconds);
        }

        private void CastOff(GameStart start)
        {
            begin(start);
            title.AddToClassList(LeavingClass);
            title.schedule.Execute(title.RemoveFromHierarchy).ExecuteLater(FadeMilliseconds);
        }
    }
}
