using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace NXM_Handler
{
    
    internal static partial class Relay
    {
        [GeneratedRegex("nxm://(.*?)/mods/[0-9]*/files/[0-9]*")]
        private static partial Regex NXMParse();
        internal static void RelayURL(string url) {
            var game = NXMParse().Matches(url)[0].Groups[1].Value;
            if (game is null)
            {
                Logger.Error.Log($"ERROR: {url} is not a valid NXM URL");
                //throw new ArgumentNullException($"{url} is not a valid NXM URL");
            }
            else if (Storage.Store!.MMAssociations.TryGetValue(game, out MMAssociation mmassoc))
            {
                var MMArgs = $"{Storage.Store.ModManagers[mmassoc.MMName].Path} {mmassoc.SpecialArgs.pre} {url} {mmassoc.SpecialArgs.post}";
                CallModManager(game, MMArgs);
            }
            else
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    var _ = new MMAssocWindow(game);
                });
                Relay.RelayURL(url);
            }
        }
        internal static void RegisterNXM()
        {
            if (!NXMProtocol.Register() || !NXMProtocol.Validate())
            {
                Logger.Error.Log("CRITICAL: Failed to register or validate NXM protocol association. This program will now exit");
                App.Current.Shutdown();
                //TODO: inform user
            }
        }
        private static void CallModManager(string game, string MMArgs)
        {
            var fullPath = Storage.Store!.ModManagers[Storage.Store!.MMAssociations[game].MMName].Path;
            string path = Path.GetDirectoryName(fullPath) ?? string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                Logger.Error.Log($"ERROR: {fullPath} is invalid somehow");
            }
            else
            {
                using var cmd = new Process();
                cmd.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    WorkingDirectory = path,
                    CreateNoWindow = true,
                    ArgumentList = { "/C", MMArgs }
                };
                cmd.Start();
                cmd.Close();
            }
        }
    }
    //Modified from Stardrop Mod Manager https://github.com/Floogen/Stardrop/blob/development/Stardrop/Utilities/NXMProtocol.cs
    //Full repo https://github.com/Floogen/Stardrop/tree/development
    internal static class NXMProtocol
    {
        private static readonly string? applicationPath = Environment.ProcessPath;
        public static bool Register()
        {
            try
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Logger.Debug.Log($"WARN: Attempted to modify registery keys for NXM protocol on a non-Windows system!");
                    return false;
                }
                if (applicationPath is null)
                    throw new NullReferenceException("unable to get NXM Handler executible path");

                var keyTest = Registry.CurrentUser?.OpenSubKey("Software", true)?.OpenSubKey("Classes", true);
                RegistryKey key = keyTest is not null ? keyTest.CreateSubKey("nxm") : throw new NullReferenceException("Unable to access registry");
                key.SetValue("URL Protocol", "nxm");
                key.CreateSubKey(@"shell\open\command").SetValue("", "\"" + applicationPath + "\" \"%1\"");
            }
            catch (Exception ex)
            {
                Logger.Error.Log($"ERROR: Failed to associate NXM Handler with the NXM protocol: {ex}");
                return false;
            }

            return true;
        }

        public static bool Validate()
        {
            try
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Logger.Debug.Log($"WARN: Attempted to modify registery keys for NXM protocol on a non-Windows system!");
                    return false;
                }

                var baseKeyTest = Registry.CurrentUser?.OpenSubKey("Software", true)?.OpenSubKey("Classes", true)?.OpenSubKey("nxm", true);
                if (baseKeyTest is null || baseKeyTest?.GetValue("URL Protocol")?.ToString() != "nxm")
                {
                    return false;
                }

                var actualKeyTest = Registry.CurrentUser?.OpenSubKey("Software", true)?.OpenSubKey("Classes", true)?.OpenSubKey(@"nxm\shell\open\command", true);
                if (actualKeyTest is null || actualKeyTest?.GetValue(string.Empty)?.ToString() != "\"" + applicationPath + "\" \"%1\"")
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error.Log($"ERROR: Failed to modify registry keys to associate NXM Handler with the NXM protocol: {ex}");
                return false;
            }

            return true;
        }
    }
}
