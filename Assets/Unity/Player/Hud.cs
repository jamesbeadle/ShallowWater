using System.Collections.Generic;
using ShallowWater.Unity.Helm;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Hud : MonoBehaviour
    {
        private const float HintSeconds = 20f;
        private const float WidthShare = 0.5f;
        private const float LineHeightShare = 0.045f;
        private const int FontShare = 40;
        private const string StepAshore = "E  step ashore";
        private const string StepAboard = "E  step aboard";
        private const string HelmHint = "W / S  lever ahead and astern     A / D  tiller     Space  stop     right mouse  look round";
        private const string WalkHint = "W A S D  walk     Shift  run     right mouse  look round";

        private BoatController boat;
        private ShoreLeave shore;
        private GUIStyle style;

        private void Start()
        {
            boat = GetComponent<BoatController>();
            shore = GetComponent<ShoreLeave>();
        }

        private void OnGUI()
        {
            if (boat == null) return;
            style = style ?? HudStyle.Made(Screen.height / FontShare, TextAnchor.LowerLeft);
            var lines = Lines();
            var margin = HudLayout.Margin;
            var height = Screen.height * LineHeightShare * lines.Count;
            var place = new Rect(HudLayout.BesideTheRadar, Screen.height - margin - height, Screen.width * WidthShare, height);
            GUI.Label(place, string.Join("\n", lines), style);
        }

        private List<string> Lines()
        {
            var lines = new List<string>();
            var lever = NotchNames.Of(boat.Notch);
            var isAtTheHelm = !shore.IsAshore;
            if (isAtTheHelm) lines.Add(lever);
            if (shore.CanStepAshore) lines.Add(StepAshore);
            if (shore.CanStepAboard) lines.Add(StepAboard);
            var isNew = Time.timeSinceLevelLoad < HintSeconds;
            if (isNew) lines.Add(shore.IsAshore ? WalkHint : HelmHint);
            return lines;
        }
    }
}
