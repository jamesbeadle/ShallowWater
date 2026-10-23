using ShallowWater.Game.Kit;
using ShallowWater.Game.Missions;
using ShallowWater.Game.Pound;
using ShallowWater.Unity.Controls;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.Overlay;
using ShallowWater.Unity.Sky;
using ShallowWater.Unity.World;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Player
{
    public sealed class PocketBook : MonoBehaviour
    {
        private ShoreLeave shore;
        private Moorer moorer;
        private Marksman marksman;
        private BoatController boat;
        private FarmJob job;
        private DayAndNight day;
        private Pound pound;
        private string poundLength;
        private PocketBookScreen book;

        public Kit Kit { get; } = new Kit();
        public bool IsOpen { get; private set; }

        public void Opens(VisualElement screen, DayAndNight dayAndNight, Pound water)
        {
            day = dayAndNight;
            pound = water;
            poundLength = OldDistance.Words(water.LengthMetres);
            book = new PocketBookScreen(screen, ControlsRows.Book, ControlsRows.PutAway, TakeOut);
        }

        private void Start()
        {
            shore = GetComponent<ShoreLeave>();
            moorer = GetComponent<Moorer>();
            marksman = GetComponent<Marksman>();
            boat = GetComponent<BoatController>();
            var farm = GetComponent<FarmHud>();
            job = farm.Job;
        }

        private void Update()
        {
            var isReady = book != null && job != null;
            if (!isReady) return;
            var isPutAway = IsOpen && BookInput.IsPutAway();
            var isTurned = BookInput.IsAskedFor() || isPutAway;
            if (isTurned) Turn(!IsOpen);
        }

        private void LateUpdate()
        {
            if (!IsOpen) return;
            var isAshore = shore.IsAshore;
            var kitPage = book.Kit;
            kitPage.Show(job, marksman.Rifle, isAshore, Kit, day.Now);
            var dayPage = book.Day;
            dayPage.Show(job, boat.Notch, moorer.Mooring, isAshore);
        }

        private void TakeOut()
        {
            if (!IsOpen) Turn(true);
        }

        private void Turn(bool isOpening)
        {
            IsOpen = isOpening;
            if (isOpening) Attention.TurnToTheBook();
            if (isOpening) WorldPause.Begin();
            if (isOpening) PlaceSparrow();
            if (!isOpening) Attention.TurnToTheWorld();
            if (!isOpening) WorldPause.End();
            book.Open(isOpening);
        }

        private void PlaceSparrow()
        {
            var motion = boat.Motion;
            var dayPage = book.Day;
            dayPage.Place(Whereabouts.Of(pound, motion.Position), poundLength);
        }
    }
}
