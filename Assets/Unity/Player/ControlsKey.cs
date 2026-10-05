using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class ControlsKey : MonoBehaviour
    {
        private const float ButtonWidthShare = 0.16f;
        private const float PanelWidthShare = 0.36f;
        private const float LineHeightShare = 0.045f;
        private const int FontShare = 40;
        private const string Closed = "Controls  (H)";
        private const string Open = "Hide controls  (H)";
        private static readonly string[] AtTheHelm =
        {
            "W / S   lever ahead and astern", "A / D   tiller", "Space   stop", "E   step ashore, by the bank",
            "M   the 1900 map", "Right mouse   look round", "Scroll   zoom",
        };
        private static readonly string[] Ashore =
        {
            "W A S D   walk", "Shift   run", "E   step aboard, by the stern", "T   tie up or cast off",
            "M   the 1900 map", "Right mouse   look round", "Scroll   zoom",
        };

        private ShoreLeave shore;
        private GUIStyle buttonStyle;
        private GUIStyle panelStyle;
        private bool isOpen;

        private void Start()
        {
            shore = GetComponent<ShoreLeave>();
        }

        private void Update()
        {
            if (KeyInput.IsTogglingTheKey()) isOpen = !isOpen;
        }

        private void OnGUI()
        {
            if (shore == null) return;
            var fontSize = Screen.height / FontShare;
            buttonStyle = buttonStyle ?? HudStyle.Made(fontSize, TextAnchor.MiddleCenter);
            panelStyle = panelStyle ?? HudStyle.Made(fontSize, TextAnchor.UpperLeft);
            var margin = HudLayout.Margin;
            var lineHeight = Screen.height * LineHeightShare;
            var buttonWidth = Screen.width * ButtonWidthShare;
            var button = new Rect(margin, margin, buttonWidth, lineHeight);
            var isClicked = GUI.Button(button, isOpen ? Open : Closed, buttonStyle);
            if (isClicked) isOpen = !isOpen;
            if (!isOpen) return;
            ShowThePanel(margin, lineHeight, button.yMax);
        }

        private void ShowThePanel(float margin, float lineHeight, float top)
        {
            var lines = shore.IsAshore ? Ashore : AtTheHelm;
            var width = Screen.width * PanelWidthShare;
            var panel = new Rect(margin, top + margin, width, lineHeight * lines.Length);
            GUI.Label(panel, string.Join("\n", lines), panelStyle);
        }
    }
}
