using System.Diagnostics;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Core;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class FileIO
    {
        public static void OpenLink(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var link = Language.GetText(key);

            if (string.IsNullOrEmpty(link))
            {
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName        = "explorer.exe",
                Arguments       = link
            });
        }

        /// <summary>
        /// 创建一个快捷方式
        /// </summary>
        /// <param name="lnkFilePath">快捷方式的完全限定路径。</param>
        /// <param name="workDir"></param>
        /// <param name="args">快捷方式启动程序时需要使用的参数。</param>
        /// <param name="targetPath"></param>
        public static void CreateShortcut(string lnkFilePath, string targetPath, string workDir, string args = "")
        {
            var     shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell     = Activator.CreateInstance(shellType);
            var     shortcut  = shell.CreateShortcut(lnkFilePath);
            shortcut.TargetPath       = targetPath;
            shortcut.Arguments        = args;
            shortcut.WorkingDirectory = workDir;
            shortcut.Save();
        }

        /// <summary>
        /// 读取一个快捷方式的信息
        /// </summary>
        /// <param name="lnkFilePath"></param>
        /// <returns></returns>
        public static ShortcutDescription ReadShortcut(string lnkFilePath)
        {
            var     shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell     = Activator.CreateInstance(shellType);
            var     shortcut  = shell.CreateShortcut(lnkFilePath);
            return new ShortcutDescription()
            {
                TargetPath       = shortcut.TargetPath,
                Arguments        = shortcut.Arguments,
                WorkingDirection = shortcut.WorkingDirectory,
            };
        }

        public static RelayCommand<string> OpenLinkCommand { get; } = new RelayCommand<string>(OpenLink);
    }

    public class ShortcutDescription
    {
        public object TargetPath { get; set; }
        public object Arguments { get; set; }
        public object WorkingDirection { get; set; }
    }
}