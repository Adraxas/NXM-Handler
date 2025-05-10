using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                //Add logging and/or alert user
            }
            string json = JsonSerializer.Serialize(Store);
            using StreamWriter w = new($"{_appdir}\\store\\data.json");
            w.Write(json);
        }
        internal static void AddNewGameAssoc(MMAssociation MMAssoc)
        {
            if (Store!.MMAssociations.TryAdd(MMAssoc.NXString, MMAssoc))
            {
                Store.MMAssociations.Remove(MMAssoc.NXString);
                Store.MMAssociations.Add(MMAssoc.NXString, MMAssoc);
                //Add logging and/or alert user
                //Shouldn't happen
            }
            string json = JsonSerializer.Serialize(Store);
            using StreamWriter w = new($"{_appdir}\\store\\data.json");
            w.Write(json);
        }
    }
    internal readonly record struct ModManager(string Name, string Path);
    internal readonly struct MMAssociation(string NXString, ref ModManager MM, (string pre, string post) SpecialArgs = default)
    {
        [JsonInclude]
        public readonly string NXString = NXString;
        [JsonInclude]
        public readonly string MM = MM.Path;
        [JsonInclude]
        public readonly (string pre, string post) SpecialArgs = SpecialArgs;
    }
    internal sealed record DataStore(Dictionary<string, ModManager> ModManagers, Dictionary<string, MMAssociation> MMAssociations);
    internal static class Settings
    {
        private readonly static string _appdir;
        static Settings() {
            _appdir = Environment.CurrentDirectory;
        }
    }
}