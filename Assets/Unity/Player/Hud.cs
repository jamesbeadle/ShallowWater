using System.Collections.Generic;
using ShallowWater.Unity.Helm;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Hud : MonoBehaviour
    {
        private const float HintSeconds = 20f;
        private const float MarginShare = 0.025f;
        private const float WidthShare = 0.5f;
        private const float LineHeightShare = 0.045f;
        private const int FontShare = 40;
        private const string StepAshore = "E  step ashore";
        private const string StepAboard = "E  step aboard";
        private const string HelmHint = "W / S  lever ahead and astern     A / D  tiller     Space  stop     right mouse  look round";
        private const string TieUp = "T  tie her up";
        private const string CastOff = "T  cast off";
        private const string MadeFast = "She is made fast: cast off before stepping aboard";
        private const string WalkHint = "W A S D  walk     Shift  run     T  tie up     right mouse  look round";

        private BoatController boat;
        private ShoreLeave shore;
        private Moorer moorer;
        private GUIStyle style;

        private void Start()
        {
            boat = GetComponent<BoatController>();
            shore = GetComponent<ShoreLeave>();
            moorer = GetComponent<Moorer>();
        }

        private void OnGUI()
        {
            if (boat == null) return;
            style = style ?? HudStyle.Made(Screen.height / FontShare);
            var lines = Lines();
            var margin = Screen.height * MarginShare;
            var height = Screen.height * LineHeightShare * lines.Count;
            var place = new Rect(margin, Screen.height - margin - height, Screen.width * WidthShare, height);
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
            if (moorer.CanTieUp) lines.Add(TieUp);
            if (moorer.CanCastOff) lines.Add(CastOff);
            var isWaitingToCastOff = shore.IsHeldAshoreByTheLine && !moorer.CanCastOff;
            if (isWaitingToCastOff) lines.Add(MadeFast);
            var isNew = Time.timeSinceLevelLoad < HintSeconds;
            if (isNew) lines.Add(shore.IsAshore ? WalkHint : HelmHint);
            return lines;
        }
    }
}
