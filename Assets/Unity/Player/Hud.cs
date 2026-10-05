using System.Collections.Generic;
using ShallowWater.Unity.Helm;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Hud : MonoBehaviour
    {
        private const int FontShare = 42;
        private const float LineShare = 0.042f;
        private const float TelegraphShare = 0.075f;
        private const float RowGapShare = 0.35f;
        private static readonly HudPrompt StepAshore = new HudPrompt("E", "Step ashore");
        private static readonly HudPrompt StepAboard = new HudPrompt("E", "Step aboard");
        private static readonly HudPrompt TieUp = new HudPrompt("T", "Tie her up");
        private static readonly HudPrompt CastOff = new HudPrompt("T", "Cast off");
        private static readonly HudPrompt MadeFast = new HudPrompt("T", "Cast off at the post before stepping aboard");

        private BoatController boat;
        private ShoreLeave shore;
        private Moorer moorer;
        private GUIStyle wordStyle;
        private GUIStyle capStyle;

        private void Start()
        {
            boat = GetComponent<BoatController>();
            shore = GetComponent<ShoreLeave>();
            moorer = GetComponent<Moorer>();
        }

        private void OnGUI()
        {
            if (boat == null) return;
            var fontSize = Screen.height / FontShare;
            wordStyle = wordStyle ?? HudStyle.Made(fontSize, TextAnchor.MiddleLeft);
            capStyle = capStyle ?? HudStyle.KeyCap(fontSize);
            var left = HudLayout.BesideTheRadar;
            var bottom = Screen.height - HudLayout.Margin;
            var isAtTheHelm = !shore.IsAshore;
            if (isAtTheHelm) bottom = DrawTheTelegraph(left, bottom);
            DrawThePrompts(left, bottom);
        }

        private float DrawTheTelegraph(float left, float bottom)
        {
            var height = Screen.height * TelegraphShare;
            var area = new Rect(left, bottom - height, Screen.width, height);
            Telegraph.Draw(area, boat.Notch, wordStyle);
            return area.y - Screen.height * LineShare * RowGapShare;
        }

        private void DrawThePrompts(float left, float bottom)
        {
            var line = Screen.height * LineShare;
            var step = line * (1f + RowGapShare);
            var prompts = Prompts();
            for (var index = 0; index < prompts.Count; index++)
            {
                var corner = new Vector2(left, bottom - line - step * index);
                KeyCaps.Prompt(corner, line, prompts[index], capStyle, wordStyle, HudInk.Unchanged);
            }
        }

        private List<HudPrompt> Prompts()
        {
            var prompts = new List<HudPrompt>();
            if (shore.CanStepAshore) prompts.Add(StepAshore);
            if (shore.CanStepAboard) prompts.Add(StepAboard);
            if (moorer.CanTieUp) prompts.Add(TieUp);
            if (moorer.CanCastOff) prompts.Add(CastOff);
            var isWaitingToCastOff = shore.IsHeldAshoreByTheLine && !moorer.CanCastOff;
            if (isWaitingToCastOff) prompts.Add(MadeFast);
            return prompts;
        }
    }
}
