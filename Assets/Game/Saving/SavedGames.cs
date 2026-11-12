using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Saving
{
    public sealed class SavedGames
    {
        public const int MostKeptByHand = 8;

        public SavedGames(IEnumerable<SavedGame> games)
        {
            NewestFirst = games.OrderByDescending(game => game.SavedAt).ToList();
        }

        public IReadOnlyList<SavedGame> NewestFirst { get; }
        public int Count => NewestFirst.Count;
        public bool HasAny => Count > 0;
        public SavedGame Latest => NewestFirst.FirstOrDefault();

        public IEnumerable<SavedGame> ToForget()
        {
            var keptByHand = NewestFirst.Where(game => !game.IsAutosave);
            return keptByHand.Skip(MostKeptByHand);
        }
    }
}
