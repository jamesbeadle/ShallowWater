using System.Collections.Generic;
using ShallowWater.Game.Guns;
using ShallowWater.Unity.Overlay;

namespace ShallowWater.Unity.Player
{
    public static class FarmPrompts
    {
        private const string Space = "Space";
        private const string ChangeKey = "X";
        private const string TakeThe = "Take the ";
        private static readonly HudPrompt Talk = new HudPrompt("E", "Talk to the farmer");
        private static readonly HudPrompt Next = new HudPrompt("E", "Next");
        private static readonly HudPrompt Done = new HudPrompt("E", "Done");
        private static readonly HudPrompt Shoulder = new HudPrompt("Z", "Shoulder the rifle");
        private static readonly HudPrompt Lower = new HudPrompt("Z", "Lower the rifle");
        private static readonly HudPrompt Fire = new HudPrompt(KeyGlyphs.Either(Space, KeyGlyphs.LeftMouse), "Fire");
        private static readonly Dictionary<Rifle, HudPrompt> TakeUp = new Dictionary<Rifle, HudPrompt>();

        public static IEnumerable<HudPrompt> Of(Talker talker, Marksman marksman)
        {
            var shown = new List<HudPrompt>();
            if (talker.CanTalk) shown.Add(Talk);
            var nextOrDone = NextOrDone(talker);
            if (talker.IsTalking) shown.Add(nextOrDone);
            var isReadyToShoulder = marksman.CanAim && !marksman.IsAiming;
            if (isReadyToShoulder) shown.Add(Shoulder);
            var takeUpTheOther = TakeUpThe(marksman.Slung);
            if (marksman.CanAim) shown.Add(takeUpTheOther);
            if (marksman.IsAiming) shown.Add(Fire);
            if (marksman.IsAiming) shown.Add(Lower);
            return shown;
        }

        private static HudPrompt NextOrDone(Talker talker)
        {
            var conversation = talker.Conversation;
            return conversation.IsOnTheLastLine ? Done : Next;
        }

        private static HudPrompt TakeUpThe(Rifle slung)
        {
            var isNamed = TakeUp.TryGetValue(slung, out var prompt);
            if (isNamed) return prompt;
            var name = slung.Name;
            TakeUp[slung] = new HudPrompt(ChangeKey, TakeThe + name.ToLowerInvariant());
            return TakeUp[slung];
        }
    }
}
