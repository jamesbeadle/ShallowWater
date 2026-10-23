using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public static class HintsRow
    {
        private const string RowClass = "hud-hints";

        public static VisualElement On(VisualElement screen)
        {
            var row = screen.Q(className: RowClass);
            var isLaid = row != null;
            if (isLaid) return row;
            row = OverlayElements.Block(RowClass);
            screen.Add(row);
            return row;
        }
    }
}
