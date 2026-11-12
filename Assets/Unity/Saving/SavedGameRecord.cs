using System;
using ShallowWater.Game.Day;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Saving;

namespace ShallowWater.Unity.Saving
{
    [Serializable]
    public sealed class SavedGameRecord
    {
        public double boatEast;
        public double boatNorth;
        public double boatBearing;
        public double minutesSinceMidnight;
        public string place;
        public long savedAtTicks;
        public bool isAutosave;

        public static SavedGameRecord Of(SavedGame game)
        {
            var position = game.BoatPosition;
            var time = game.Time;
            var savedAt = game.SavedAt;
            return new SavedGameRecord
            {
                boatEast = position.East,
                boatNorth = position.North,
                boatBearing = game.BoatBearing,
                minutesSinceMidnight = time.MinutesSinceMidnight,
                place = game.Place,
                savedAtTicks = savedAt.Ticks,
                isAutosave = game.IsAutosave,
            };
        }

        public SavedGame Game()
        {
            var position = new GroundPoint(boatEast, boatNorth);
            var time = new TimeOfDay(minutesSinceMidnight);
            var savedAt = new DateTime(savedAtTicks, DateTimeKind.Utc);
            return new SavedGame(position, boatBearing, time, place, savedAt, isAutosave);
        }
    }
}
