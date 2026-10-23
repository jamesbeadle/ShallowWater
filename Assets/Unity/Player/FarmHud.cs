using ShallowWater.Game.Missions;
using ShallowWater.Unity.Overlay;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Player
{
    public sealed class FarmHud : MonoBehaviour
    {
        private FarmRifleMission mission;
        private Talker talker;
        private Marksman marksman;
        private SpeechCard speech;
        private JobCard job;
        private Sight sight;

        public FarmRifleMission Mission => mission;

        public void Shows(VisualElement screen, FarmRifleMission farmJob)
        {
            mission = farmJob;
            speech = new SpeechCard(screen);
            job = new JobCard(screen, FarmerLines.Farm);
            sight = new Sight(screen);
        }

        private void Start()
        {
            talker = GetComponent<Talker>();
            marksman = GetComponent<Marksman>();
        }

        private void LateUpdate()
        {
            var isShowing = mission != null && talker != null;
            if (!isShowing) return;
            speech.Show(talker.Conversation);
            job.Show(mission);
            sight.Show(marksman.IsAiming);
        }
    }
}
