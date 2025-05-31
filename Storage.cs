using System.IO;
using System.Text.Json;

namespace NXM_Handler
{
    internal static class Storage
    {
        //DO NOT ACCESS BEFORE INIT
        internal static readonly DataStore? Store;
        private readonly static string _appdir;
        static Storage()
        {
            _appdir = Environment.CurrentDirectory;
            Directory.CreateDirectory($"{_appdir}\\store");
            if (File.Exists($"{_appdir}\\store\\data.json"))
            {
                using StreamReader r = new($"{_appdir}\\store\\data.json");
                string json = r.ReadToEnd();
                Store = JsonSerializer.Deserialize<DataStore>(json) ?? new([], []);
            }
            else
            {
                Store = new([], []);
            }
        }
        internal static void AddNewModManager(ModManager modManager)
        {
            if (Store!.ModManagers.TryAdd(modManager.Name, modManager))
            {
                Store.ModManagers.Remove(modManager.Name);
                Store.ModManagers.Add(modManager.Name, modManager);
                Logger.Debug.Log($"WARN: ModManager {modManager.Name} already exists, overwriting with new settings");
            }
            string json = JsonSerializer.Serialize(Store);
            using StreamWriter w = new($"{_appdir}\\store\\data.json");
            w.Write(json);
        }
        internal static void AddNewGameAssoc(MMAssociation MMAssoc)
        {
            if (Store!.MMAssociations.TryAdd(MMAssoc.NXString, MMAssoc))
            {
                Logger.Error.Log($"ERROR: Association {MMAssoc.NXString} already exists, this should not be possible");
                //Throw maybe?
            }
            string json = JsonSerializer.Serialize(Store);
            using StreamWriter w = new($"{_appdir}\\store\\data.json");
            w.Write(json);
        }
    }
    internal readonly record struct ModManager(string Name, string Path, string MMArgs = "");
    internal readonly record struct MMAssociation(string NXString, string MMName, (string? pre , string? post) SpecialArgs = default)
    {
        public readonly (string pre, string post) SpecialArgs = (SpecialArgs.pre ?? "", SpecialArgs.post ?? "");
    }
    internal sealed record DataStore(Dictionary<string, ModManager> ModManagers, Dictionary<string, MMAssociation> MMAssociations);
    internal static class Settings
    {
        internal readonly static string _appdir;
        static Settings() {
            _appdir = Environment.CurrentDirectory;
        }
    }
}