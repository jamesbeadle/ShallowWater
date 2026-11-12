using System;
using ShallowWater.Game.Day;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Saving
{
    public sealed class SavedGame
    {
        public SavedGame(GroundPoint boatPosition, double boatBearing, TimeOfDay time, string place, DateTime savedAt, bool isAutosave)
        {
            BoatPosition = boatPosition;
            BoatBearing = boatBearing;
            Time = time;
            Place = place;
            SavedAt = savedAt;
            IsAutosave = isAutosave;
        }

        public GroundPoint BoatPosition { get; }
        public double BoatBearing { get; }
        public TimeOfDay Time { get; }
        public string Place { get; }
        public DateTime SavedAt { get; }
        public bool IsAutosave { get; }
        public string Whereabouts => $"{Time}, {Place}";
    }
}
