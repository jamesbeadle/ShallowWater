using System.Collections.Generic;
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
        private static readonly HudPrompt TieUp = new HudPrompt("T", "Tie her up");
        private static readonly HudPrompt CastOff = new HudPrompt("T", "Cast off");
        private static readonly HudPrompt MadeFast = new HudPrompt("T", "Cast off at the post before stepping aboard");

        private BoatController boat;
        private ShoreLeave shore;
        private Moorer moorer;
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
            if (moorer.CanTieUp) shown.Add(TieUp);
            if (moorer.CanCastOff) shown.Add(CastOff);
            var isWaitingToCastOff = shore.IsHeldAshoreByTheLine && !moorer.CanCastOff;
            if (isWaitingToCastOff) shown.Add(MadeFast);
            return shown;
        }
    }
}
