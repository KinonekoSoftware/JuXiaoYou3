using System.Diagnostics;

namespace Acorisoft.FutureGL.MigaUtils
{
    public class ProcessUtilities
    {
        public static void Kill()
        {
            var current        = Process.GetCurrentProcess();
            var currentPID     = current.Id;
            var currentAppName = current.ProcessName;
            var sameNameApps    = Process.GetProcessesByName(currentAppName).Where(x => x.Id != currentPID);
            
            foreach (var app in sameNameApps)
            {
                Debug.WriteLine($"正在结束进程：{app.Id}，{app.ProcessName}");
                app.Kill();
            }
        }
    }
}