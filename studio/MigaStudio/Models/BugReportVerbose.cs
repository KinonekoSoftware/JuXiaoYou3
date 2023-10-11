using System.Diagnostics;
using System.IO;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public enum BugLevel : int
    {
        Bug,
        NotImplemented,
        Crash,
    }
    
    public class BugReportVerbose : PendingVerbose
    {
        public string Database { get; set; }
        public string Logs { get; init; }
        public BugLevel Bug { get; set; }
        
        public override void Run()
        {
            var db = string.IsNullOrEmpty(Database) ? "d" : Database;
            Crash($"{db} {(int)Bug} {Logs}");
        }

        public static void Crash(string args)
        {
            var thisAppDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var crashReporter    = Path.Combine(thisAppDirectory, "MigaStudio.BugReporter.exe");
            
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                Arguments       = args,
                FileName        = crashReporter
            });
        }
    }
}