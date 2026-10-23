using ShallowWater.Game.Missions;
using ShallowWater.Unity.Overlay;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Player
{
    public sealed class FarmHud : MonoBehaviour
    {
        private FarmJob job;
        private Talker talker;
        private Marksman marksman;
        private SpeechCard speech;
        private JobCard card;
        private Sight sight;

        public void Shows(VisualElement screen, FarmJob farmJob)
        {
            job = farmJob;
            speech = new SpeechCard(screen);
            card = new JobCard(screen, FarmerLines.Farm);
            sight = new Sight(screen);
        }

        private void Start()
        {
            talker = GetComponent<Talker>();
            marksman = GetComponent<Marksman>();
        }

        private void LateUpdate()
        {
            var isShowing = job != null && talker != null;
            if (!isShowing) return;
            speech.Show(talker.Conversation);
            card.Show(job);
            sight.Show(marksman.IsAiming);
        }
    }
}
