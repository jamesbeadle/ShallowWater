using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class ControlsKey : MonoBehaviour
    {
        private const int FontShare = 44;
        private const float LineShare = 0.04f;
        private const float RowStepShare = 1.45f;
        private const float BackdropWidthShare = 0.5f;
        private const float CapsColumnShare = 0.11f;
        private const float RuleShare = 0.0025f;
        private const float TitleRowsDown = 1.5f;
        private const float RuleColumns = 3f;
        private const float Half = 0.5f;
        private const string Title = "CONTROLS";

        private ShoreLeave shore;
        private GUIStyle wordStyle;
        private GUIStyle titleStyle;
        private GUIStyle capStyle;
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
            wordStyle = wordStyle ?? HudStyle.Made(fontSize, TextAnchor.MiddleLeft);
            titleStyle = titleStyle ?? HudStyle.Caps(fontSize, TextAnchor.MiddleLeft);
            capStyle = capStyle ?? HudStyle.KeyCap(fontSize);
            var line = Screen.height * LineShare;
            var corner = new Vector2(HudLayout.Margin, HudLayout.Margin);
            var toggle = new Rect(corner.x, corner.y, Screen.width * CapsColumnShare, line);
            var isClicked = GUI.Button(toggle, GUIContent.none, GUIStyle.none);
            if (isClicked) isOpen = !isOpen;
            if (!isOpen)
            {
                KeyCaps.Prompt(corner, line, ControlsRows.Open, capStyle, wordStyle, HudInk.Hint);
                return;
            }
            ShowThePanel(corner, line);
        }

        private void ShowThePanel(Vector2 corner, float line)
        {
            GUI.color = HudInk.Backdrop;
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width * BackdropWidthShare, Screen.height), HudTextures.Fade);
            GUI.color = HudInk.Unchanged;
            KeyCaps.Prompt(corner, line, ControlsRows.Close, capStyle, wordStyle, HudInk.Unchanged);
            var step = line * RowStepShare;
            var titlePlace = new Rect(corner.x, corner.y + step * TitleRowsDown, Screen.width, line);
            HudText.Shadowed(titlePlace, Title, titleStyle, HudInk.Faint);
            GUI.color = HudInk.Faint;
            GUI.DrawTexture(new Rect(corner.x, titlePlace.yMax, Screen.width * CapsColumnShare * RuleColumns, Screen.height * RuleShare), Texture2D.whiteTexture);
            GUI.color = HudInk.Unchanged;
            var rows = shore.IsAshore ? ControlsRows.Ashore : ControlsRows.AtTheHelm;
            for (var index = 0; index < rows.Length; index++) Row(new Vector2(corner.x, titlePlace.yMax + step * (index + Half)), line, rows[index]);
        }

        private void Row(Vector2 corner, float line, HudPrompt row)
        {
            KeyCaps.Draw(corner, line, row.Keys, capStyle);
            var words = new Rect(corner.x + Screen.width * CapsColumnShare, corner.y, Screen.width, line);
            HudText.Shadowed(words, row.Words, wordStyle);
        }
    }
}
