using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class SavedNotice
    {
        private const string RowClass = "hud-saved-row";
        private const string NoticeClass = "hud-saved";
        private const string HiddenClass = "hud-saved--hidden";
        private const string SavedWords = "Game saved";
        private const long ShownMilliseconds = 2400;

        private readonly VisualElement notice = OverlayElements.Glass(NoticeClass, HiddenClass);
        private IVisualElementScheduledItem hiding;

        public SavedNotice(VisualElement screen)
        {
            var row = OverlayElements.Block(RowClass);
            row.pickingMode = PickingMode.Ignore;
            notice.Add(OverlayElements.Eyebrow(SavedWords));
            row.Add(notice);
            screen.Add(row);
        }

        public void Show()
        {
            notice.RemoveFromClassList(HiddenClass);
            hiding?.Pause();
            hiding = notice.schedule.Execute(Hide);
            hiding.ExecuteLater(ShownMilliseconds);
        }

        private void Hide()
        {
            notice.AddToClassList(HiddenClass);
        }
    }
}
