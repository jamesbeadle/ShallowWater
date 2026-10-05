using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public static class OverlayScreen
    {
        private const string HolderName = "Overlay";
        private const string ThemePath = "Hud/HudTheme";
        private const string SheetPath = "Hud/Hud";
        private const string ScreenClass = "hud-screen";
        private const string SafeAreaClass = "hud-safe-area";

        public static VisualElement Raise()
        {
            var holder = new GameObject(HolderName);
            var document = holder.AddComponent<UIDocument>();
            document.panelSettings = ScaledToTheScreen();
            var root = document.rootVisualElement;
            var sheets = root.styleSheets;
            sheets.Add(Resources.Load<StyleSheet>(SheetPath));
            root.AddToClassList(ScreenClass);
            var safeArea = OverlayElements.Block(SafeAreaClass);
            InsetByTheMargin(safeArea.style);
            root.Add(safeArea);
            return safeArea;
        }

        private static PanelSettings ScaledToTheScreen()
        {
            var settings = ScriptableObject.CreateInstance<PanelSettings>();
            settings.themeStyleSheet = Resources.Load<ThemeStyleSheet>(ThemePath);
            settings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            settings.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            settings.referenceResolution = HudLayout.ReferenceScreen;
            settings.match = HudLayout.MatchTheHeight;
            return settings;
        }

        private static void InsetByTheMargin(IStyle style)
        {
            var margin = HudLayout.MarginUnits;
            style.left = margin;
            style.top = margin;
            style.right = margin;
            style.bottom = margin;
        }
    }
}
