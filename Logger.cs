using System.IO;
using System.Text.RegularExpressions;

//Blame the ADHD medication shortage for this shitty code
namespace NXM_Handler
{
    internal sealed class Logger
    {
        private readonly string _logdir;
        private readonly StreamWriter _stream;
        private readonly bool _enabled = true;
        //KeepNum special values: 0 = overwrite latest every run, -1 = disable logging, -2 = append to latest
        private Logger(string FileName, int KeepNum)
        {
            _logdir = $"{Settings._appdir}\\logs";
            Directory.CreateDirectory(_logdir);
            var Latest = $"{_logdir}\\{FileName}.latest.log";
            var append = false;
            switch (KeepNum)
            {
                case 0:
                    File.Delete(Latest);
                    break;
                case -1:
                    _enabled = false;
                    break;
                case -2:
                    append = true;
                    break;
                case < -2 or > 9:
                    RenameLogs(FileName, 3);
                    break;
                default:
                    RenameLogs(FileName, KeepNum);
                    break;
            }
            _stream = new(Latest, append);
        }
        private void RenameLogs(string FileName, int KeepNum)
        {
            var regex = new Regex($"{FileName}.([a-zA-Z0-9]*).log");
            var files = from retrievedFile in Directory.EnumerateFiles(_logdir)
                        from fileName in regex.Matches(retrievedFile)
                        where fileName.Success
                        select retrievedFile;
            foreach (var file in files.Reverse())
            {
                var filename = Path.GetFileName(file);
                var parts = filename.Split('.');
                var age = parts[1];
                if (age == "latest")
                {
                    parts[1] = "1";
                    File.Move(file, $"{_logdir}\\{string.Join(".", parts)}");
                }
                else if (int.Parse(age) < KeepNum)
                {
                    parts[1] = (int.Parse(age) + 1).ToString();
                    File.Move(file, $"{_logdir}\\{string.Join(".", parts)}");
                }
                else
                {
                    File.Delete(file);
                }
            }
        }
        internal void Log(string StringToLog)
        {
            _stream.WriteLine(StringToLog);
        }
        internal static readonly Logger Debug = new("debug", 3);
        internal static readonly Logger Error = new("err", 3);
    }
}
