using System.Collections.Generic;
using ShallowWater.Unity.Overlay;

namespace ShallowWater.Unity.Player
{
    public static class FarmPrompts
    {
        private const string Space = "Space";
        private static readonly HudPrompt Talk = new HudPrompt("E", "Talk to the farmer");
        private static readonly HudPrompt Next = new HudPrompt("E", "Next");
        private static readonly HudPrompt Done = new HudPrompt("E", "Done");
        private static readonly HudPrompt Shoulder = new HudPrompt("Z", "Shoulder the rifle");
        private static readonly HudPrompt Lower = new HudPrompt("Z", "Lower the rifle");
        private static readonly HudPrompt Fire = new HudPrompt(KeyGlyphs.Either(Space, KeyGlyphs.LeftMouse), "Fire");

        public static IEnumerable<HudPrompt> Of(Talker talker, Marksman marksman)
        {
            var shown = new List<HudPrompt>();
            if (talker.CanTalk) shown.Add(Talk);
            var nextOrDone = NextOrDone(talker);
            if (talker.IsTalking) shown.Add(nextOrDone);
            var isReadyToShoulder = marksman.CanAim && !marksman.IsAiming;
            if (isReadyToShoulder) shown.Add(Shoulder);
            if (marksman.IsAiming) shown.Add(Fire);
            if (marksman.IsAiming) shown.Add(Lower);
            return shown;
        }

        private static HudPrompt NextOrDone(Talker talker)
        {
            var conversation = talker.Conversation;
            return conversation.IsOnTheLastLine ? Done : Next;
        }
    }
}
