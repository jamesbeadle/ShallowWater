using System.Collections.Generic;
using ShallowWater.Game.Mooring;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.Overlay;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Player
{
    public sealed class Hud : MonoBehaviour
    {
        private static readonly HudPrompt StepAshore = new HudPrompt("E", "Step ashore");
        private static readonly HudPrompt StepAboard = new HudPrompt("E", "Step aboard");
        private static readonly HudPrompt MadeFast = new HudPrompt("R", "Let go the lines at their posts before stepping aboard");
        private static readonly Dictionary<MooringLine, HudPrompt> MakeFast = new Dictionary<MooringLine, HudPrompt>
        {
            { MooringLine.Centre, new HudPrompt("T", "Tie her up") },
            { MooringLine.Bow, new HudPrompt("T", "Tie the bow line") },
            { MooringLine.Stern, new HudPrompt("T", "Tie the stern line") }
        };
        private static readonly Dictionary<MooringLine, HudPrompt> LetGo = new Dictionary<MooringLine, HudPrompt>
        {
            { MooringLine.Centre, new HudPrompt("R", "Cast off") },
            { MooringLine.Bow, new HudPrompt("R", "Let go the bow line") },
            { MooringLine.Stern, new HudPrompt("R", "Let go the stern line") }
        };

        private BoatController boat;
        private ShoreLeave shore;
        private Moorer moorer;
        private Talker talker;
        private Marksman marksman;
        private TelegraphCard telegraph;
        private PromptColumn prompts;

        public void Shows(VisualElement screen)
        {
            telegraph = new TelegraphCard(screen);
            prompts = new PromptColumn(screen);
        }

        private void Start()
        {
            boat = GetComponent<BoatController>();
            shore = GetComponent<ShoreLeave>();
            moorer = GetComponent<Moorer>();
            talker = GetComponent<Talker>();
            marksman = GetComponent<Marksman>();
        }

        private void LateUpdate()
        {
            var isShowing = telegraph != null && boat != null;
            if (!isShowing) return;
            var isAtTheHelm = !shore.IsAshore;
            telegraph.Reveal(isAtTheHelm);
            telegraph.Show(boat.Notch);
            prompts.Show(Prompts());
        }

        private List<HudPrompt> Prompts()
        {
            var shown = new List<HudPrompt>();
            if (shore.CanStepAshore) shown.Add(StepAshore);
            if (shore.CanStepAboard) shown.Add(StepAboard);
            var toMakeFast = moorer.ToMakeFast;
            var toLetGo = moorer.ToLetGo;
            if (toMakeFast.HasValue) shown.Add(MakeFast[toMakeFast.Value]);
            if (toLetGo.HasValue) shown.Add(LetGo[toLetGo.Value]);
            var isWaitingToCastOff = shore.IsHeldAshoreByTheLine && !toLetGo.HasValue;
            if (isWaitingToCastOff) shown.Add(MadeFast);
            shown.AddRange(FarmPrompts.Of(talker, marksman));
            return shown;
        }
    }
}
