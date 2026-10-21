using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class Sight
    {
        private const string RowClass = "hud-sight-row";
        private const string RingClass = "hud-sight";
        private const string HiddenClass = "hud-sight--hidden";

        private readonly VisualElement ring = OverlayElements.Block(RingClass, HiddenClass);

        public Sight(VisualElement screen)
        {
            var row = OverlayElements.Block(RowClass);
            row.pickingMode = PickingMode.Ignore;
            ring.pickingMode = PickingMode.Ignore;
            row.Add(ring);
            screen.Add(row);
        }

        public void Show(bool isAiming)
        {
            ring.EnableInClassList(HiddenClass, !isAiming);
        }
    }
}
