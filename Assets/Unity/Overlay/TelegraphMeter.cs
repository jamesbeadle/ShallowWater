using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Boat;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class TelegraphMeter
    {
        private const string MeterClass = "hud-meter";
        private const string SegmentClass = "hud-segment";
        private const string AheadClass = "hud-segment--ahead";
        private const string AsternClass = "hud-segment--astern";
        private const string LitClass = "hud-segment--lit";
        private const string StopClass = "hud-stop";
        private const string StopLitClass = "hud-stop--lit";

        private static readonly ThrottleNotch[] Notches = Enum.GetValues(typeof(ThrottleNotch)).Cast<ThrottleNotch>().OrderBy(notch => (int)notch).ToArray();

        private readonly Dictionary<ThrottleNotch, VisualElement> marks = new Dictionary<ThrottleNotch, VisualElement>();

        public TelegraphMeter()
        {
            foreach (var notch in Notches)
            {
                var mark = Mark(notch);
                marks[notch] = mark;
                Meter.Add(mark);
            }
        }

        public VisualElement Meter { get; } = OverlayElements.Block(MeterClass);

        public void Show(ThrottleNotch lever)
        {
            foreach (var notch in Notches) marks[notch].EnableInClassList(LitClassOf(notch), IsLit(notch, lever));
        }

        private static VisualElement Mark(ThrottleNotch notch)
        {
            if (notch == ThrottleNotch.Stop) return OverlayElements.Block(StopClass);
            var direction = notch > ThrottleNotch.Stop ? AheadClass : AsternClass;
            return OverlayElements.Block(SegmentClass, direction);
        }

        private static string LitClassOf(ThrottleNotch notch)
        {
            return notch == ThrottleNotch.Stop ? StopLitClass : LitClass;
        }

        private static bool IsLit(ThrottleNotch notch, ThrottleNotch lever)
        {
            if (notch == ThrottleNotch.Stop) return lever == ThrottleNotch.Stop;
            var isAhead = notch > ThrottleNotch.Stop;
            return isAhead ? lever >= notch : lever <= notch;
        }
    }
}
