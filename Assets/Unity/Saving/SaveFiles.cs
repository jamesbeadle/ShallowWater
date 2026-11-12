using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShallowWater.Game.Saving;
using UnityEngine;

namespace ShallowWater.Unity.Saving
{
    public static class SaveFiles
    {
        private const string FolderName = "Saves";
        private const string Extension = ".json";
        private const string Pattern = "*" + Extension;
        private const string AutosaveName = "autosave";
        private const string KeptByHandPrefix = "saved-";

        private static string Folder => Path.Combine(Application.persistentDataPath, FolderName);

        public static SavedGames Read()
        {
            var hasFolder = Directory.Exists(Folder);
            if (!hasFolder) return new SavedGames(Enumerable.Empty<SavedGame>());
            var files = Directory.GetFiles(Folder, Pattern);
            return new SavedGames(files.Select(ReadOne).Where(IsReadable));
        }

        public static void Write(SavedGame game)
        {
            Directory.CreateDirectory(Folder);
            var record = SavedGameRecord.Of(game);
            File.WriteAllText(PathOf(game), JsonUtility.ToJson(record));
            if (game.IsAutosave) return;
            var saves = Read();
            Forget(saves.ToForget());
        }

        private static SavedGame ReadOne(string path)
        {
            try
            {
                var record = JsonUtility.FromJson<SavedGameRecord>(File.ReadAllText(path));
                return record?.Game();
            }
            catch (System.Exception unreadable)
            {
                Debug.LogWarning($"Skipped the saved game at {path}: {unreadable.Message}");
                return null;
            }
        }

        private static bool IsReadable(SavedGame game)
        {
            return game != null;
        }

        private static void Forget(IEnumerable<SavedGame> games)
        {
            foreach (var game in games.ToList()) File.Delete(PathOf(game));
        }

        private static string PathOf(SavedGame game)
        {
            var savedAt = game.SavedAt;
            var name = game.IsAutosave ? AutosaveName : KeptByHandPrefix + savedAt.Ticks;
            return Path.Combine(Folder, name + Extension);
        }
    }
}
