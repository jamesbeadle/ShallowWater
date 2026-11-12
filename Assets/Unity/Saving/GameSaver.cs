using System;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Saving;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.Sky;
using UnityEngine;
using CanalPound = ShallowWater.Game.Pound.Pound;

namespace ShallowWater.Unity.Saving
{
    public sealed class GameSaver : MonoBehaviour
    {
        private const float PlayedSecondsBetweenAutosaves = 60f;
        private const bool ByHand = false;
        private const bool Automatically = true;

        private BoatController boat;
        private DayAndNight day;
        private CanalPound pound;
        private float playedSinceAutosave;

        public event Action Saved;

        public void Keeps(CanalPound water, DayAndNight dayAndNight)
        {
            pound = water;
            day = dayAndNight;
            boat = GetComponent<BoatController>();
            Application.quitting += SaveOnTheWayOut;
            Application.focusChanged += SaveOnLosingFocus;
        }

        private void Update()
        {
            if (boat == null) return;
            if (SaveInput.IsAskedFor()) SaveNow(ByHand);
            playedSinceAutosave += Time.deltaTime;
            var isDue = playedSinceAutosave >= PlayedSecondsBetweenAutosaves;
            if (isDue) SaveNow(Automatically);
        }

        private void OnDestroy()
        {
            Application.quitting -= SaveOnTheWayOut;
            Application.focusChanged -= SaveOnLosingFocus;
        }

        private void SaveOnTheWayOut()
        {
            SaveNow(Automatically);
        }

        private void SaveOnLosingFocus(bool hasFocus)
        {
            if (!hasFocus) SaveNow(Automatically);
        }

        private void SaveNow(bool isAutosave)
        {
            if (boat == null) return;
            playedSinceAutosave = 0;
            SaveFiles.Write(GameNow(isAutosave));
            if (!isAutosave) Saved?.Invoke();
        }

        private SavedGame GameNow(bool isAutosave)
        {
            var motion = boat.Motion;
            var place = Whereabouts.Of(pound, motion.Position);
            return new SavedGame(motion.Position, motion.Bearing, day.Now, place.Words, DateTime.UtcNow, isAutosave);
        }
    }
}
