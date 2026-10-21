using ShallowWater.Game.Farmyard;
using ShallowWater.Game.Missions;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Talker : MonoBehaviour
    {
        private ShoreLeave shore;
        private Marksman marksman;
        private FarmRifleMission mission;
        private Farmyard yard;
        private WalkerController walking;

        public Conversation Conversation { get; } = new Conversation();
        public bool CanTalk { get; private set; }
        public bool IsTalking => Conversation.IsTalking;

        public void Ready(FarmRifleMission job, Farmyard farm, WalkerController walker)
        {
            shore = GetComponent<ShoreLeave>();
            marksman = GetComponent<Marksman>();
            mission = job;
            yard = farm;
            walking = walker;
        }

        private void Update()
        {
            if (mission == null) return;
            CanTalk = shore.IsAshore && !IsTalking && !marksman.IsAiming && yard.IsWithinHail(walking.Position);
            var isAskedFor = ShoreInput.IsTalking();
            if (!isAskedFor) return;
            if (CanTalk) Begin();
            if (CanTalk) return;
            if (IsTalking) CarryOn();
        }

        private void Begin()
        {
            Conversation.Begin(FarmerLines.Name, mission.LinesNow());
            walking.enabled = false;
            walking.Face(yard.FarmerStands);
            walking.Stand();
        }

        private void CarryOn()
        {
            Conversation.Next();
            if (IsTalking) return;
            mission.Spoken();
            walking.enabled = true;
        }
    }
}
