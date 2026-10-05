using System;
using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Unity.Helm;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class Telegraph
    {
        private const float PipShare = 0.32f;
        private const float StopPipShare = 0.42f;
        private const float SpacingToPip = 1.7f;
        private const float GapToPip = 0.6f;
        private const float Half = 0.5f;
        private const float ShadowPixels = 1f;
        private static readonly ThrottleNotch[] Notches = Enum.GetValues(typeof(ThrottleNotch)).Cast<ThrottleNotch>().OrderBy(notch => (int)notch).ToArray();

        public static void Draw(Rect area, ThrottleNotch current, GUIStyle labelStyle)
        {
            var pip = area.height * PipShare;
            var spacing = pip * SpacingToPip;
            for (var index = 0; index < Notches.Length; index++)
            {
                var notch = Notches[index];
                var centre = new Vector2(area.x + pip * Half + spacing * index, area.y + pip * Half);
                Pip(centre, pip, notch, notch == current);
            }
            var label = new Rect(area.x, area.y + pip + pip * GapToPip, area.width, area.height - pip);
            var name = NotchNames.Of(current);
            HudText.Shadowed(label, name.ToUpperInvariant(), labelStyle, LitColourOf(current));
        }

        private static void Pip(Vector2 centre, float pip, ThrottleNotch notch, bool isLit)
        {
            var isStop = notch == ThrottleNotch.Stop;
            var size = isStop ? pip * StopPipShare / PipShare : pip;
            var place = new Rect(centre.x - size * Half, centre.y - size * Half, size, size);
            GUI.color = HudInk.Shadow;
            GUI.DrawTexture(new Rect(place.x + ShadowPixels, place.y + ShadowPixels, size, size), HudTextures.Ring);
            GUI.color = isLit ? LitColourOf(notch) : HudInk.Faint;
            GUI.DrawTexture(place, isLit ? HudTextures.Disc : HudTextures.Ring);
            GUI.color = HudInk.Unchanged;
        }

        private static Color LitColourOf(ThrottleNotch notch)
        {
            if (notch < ThrottleNotch.Stop) return HudInk.Astern;
            if (notch > ThrottleNotch.Stop) return HudInk.Ahead;
            return HudInk.Ink;
        }
    }
}
