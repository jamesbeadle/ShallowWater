using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class Scope
    {
        private const string RowClass = "hud-scope-row";
        private const string HiddenClass = "hud-scope-row--hidden";
        private const string FieldClass = "hud-scope";
        private const string WireClass = "hud-scope__wire";
        private const string UprightWireClass = "hud-scope__wire--upright";
        private const string LevelWireClass = "hud-scope__wire--level";

        private readonly VisualElement row = OverlayElements.Block(RowClass, HiddenClass);

        public Scope(VisualElement screen)
        {
            row.pickingMode = PickingMode.Ignore;
            row.Add(Glass(FieldClass));
            row.Add(Glass(WireClass, UprightWireClass));
            row.Add(Glass(WireClass, LevelWireClass));
            screen.Add(row);
        }

        public void Show(bool isLookingThrough)
        {
            row.EnableInClassList(HiddenClass, !isLookingThrough);
        }

        private static VisualElement Glass(params string[] classNames)
        {
            var part = OverlayElements.Block(classNames);
            part.pickingMode = PickingMode.Ignore;
            return part;
        }
    }
}
