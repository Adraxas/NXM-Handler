using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace NXM_Handler
{
    
    internal static partial class Relay
    {
        [GeneratedRegex("nxm://(.*?)/mods/[0-9]*/files/[0-9]*")]
        private static partial Regex NXMParse();
        internal static void RelayURL(string url) {
            var game = NXMParse().Matches(url);
            if (game[0] is null)
            {
                //TODDO: log instead of throw?
                throw new ArgumentNullException($"{url} is not a valid NXM URL");
            }
            else if (Storage.Store!.MMAssociations.TryGetValue(game[0].Value, out MMAssociation mmassoc))
            {
                var MMArgs = $"{Storage.Store.ModManagers[mmassoc.MMName].Path} {mmassoc.SpecialArgs.pre} {url} {mmassoc.SpecialArgs.post}";
                CallModManager(MMArgs);
            }
            else
            {
                //TODO: Create dialogue box to ask user to associate mod manager and for extra args if needed
                //Storage.AddNewGameAssoc(new MMAssociation(game[0].Value, ref Storage.Store.ModManagers[modManager], (pre, post)));
            }
        }

        internal static bool RegisterNXM()
        {
            if (!NXMProtocol.Validate() || !NXMProtocol.Register())
            {
                //TODO: Log error and inform user
                return false;
            }
            return true;
        }
        private static void CallModManager(string MMArgs)
        {

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
                    //TODO:Replace with own logging
                    //Program.helper.Log($"Attempted to modify registery keys for NXM protocol on a non-Windows system!");
                    return false;
                }
                if (applicationPath is null)
                    throw new NullReferenceException("unable to get NXM Handler executible path");

                var keyTest = Registry.CurrentUser?.OpenSubKey("Software", true)?.OpenSubKey("Classes", true);
                RegistryKey key = keyTest is not null ? keyTest.CreateSubKey("nxm") : throw new NullReferenceException("Unable to access registry");
                key.SetValue("URL Protocol", "nxm");
                key.CreateSubKey(@"shell\open\command").SetValue("", "\"" + applicationPath + "\" \"%1\"");
            }
            catch (Exception)
            {
                //TODO: Replace with own logging
                //Program.helper.Log($"Failed to associate NXM Handler with the NXM protocol: {ex}");
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
                    //TODO: Replace with own logging
                    //Program.helper.Log($"Attempted to modify registery keys for NXM protocol on a non-Windows system!");
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
            catch (Exception)
            {
                //TODO: Replace with own logging
                //Program.helper.Log($"Failed to modify registry keys to associate NXM Handler with the NXM protocol: {ex}");
                return false;
            }

            return true;
        }
    }
}
