using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.UI;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Services;
using Acorisoft.FutureGL.MigaStudio.Views;
using Acorisoft.FutureGL.MigaUtils.Collections;
using DryIoc;
using NLog;

// ReSharper disable UnusedParameter.Local

namespace Acorisoft.FutureGL.MigaStudio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string SettingDir                = "JuXiaoYou";
        private const string UserDataDir               = "UserData";
        public const  string FeedbackDir               = "Feedbacks";
        private const string LogDir                    = "Logs";
        private const string BasicSettingFileName      = "juxiaoyou-main.json";
        private const string RepositorySettingFileName = "repo.json";
        private const string AdvancedSettingFileName   = "advanced.json";

        private IDatabaseManager _databaseManager;

        public App() : base(BasicSettingFileName)
        {
            Current.DispatcherUnhandledException       += OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger.Error(e.Exception);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var thisAppDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var crashReporter    = Path.Combine(thisAppDirectory, "MigaStudio.BugReporter.exe");
            Process.Start(new ProcessStartInfo
            {
                Arguments = "dump 2 1",
                FileName  = crashReporter
            });
            //
            // 写入错误次数
            Logger.Error(e.ExceptionObject);
        }

        protected sealed override ApplicationModel ConfigureDirectory()
        {
            var domain = ApplicationModel.CheckDirectory(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    SettingDir));

            return new ApplicationModel
            {
                Logs      = Path.Combine(domain, LogDir),
                Settings  = Path.Combine(domain, UserDataDir),
                Feedbacks = Path.Combine(domain, FeedbackDir)
            }.Initialize();
        }

        protected override AppViewModel GetShell()
        {
            return new AppViewModel();
        }


        protected override void RegisterResourceDictionary(ResourceDictionary appResDict)
        {
            appResDict.MergedDictionaries.Add(ForestUI.UseToolKits());
            base.RegisterResourceDictionary(appResDict);
        }

        protected override void RegisterViews(ILogger logger, IContainer container)
        {
            //
            // TODO: 安装视图
            SubSystem.InstallViews();
            // TemplateSystem.InstallViews();
        }

        protected override void OnExitOverride(ExitEventArgs e)
        {
            Xaml.Get<AppViewModel>()
                .Stop();
            //
            // 移除所有对象
            Task.Run(async () =>
                {
                    Xaml.Get<IPendingQueue>()
                        .ForEach(x => x.Run());

                    var dir        = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crashes", "bug.txt");
                    var crashCount = GetCrashCount(dir);
                    if (crashCount > 0)
                    {
                        SetCrashCount(0, dir);
                    }

                    return await _databaseManager.CloseAsync();
                })
                .GetAwaiter()
                .GetResult();
        }

        private static int GetCrashCount(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    var lines     = File.ReadAllLines(fileName);
                    var firstLine = lines.FirstOrDefault() ?? "0";
                    return int.TryParse(firstLine, out var n) ? n : 0;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private static void SetCrashCount(int count, string fileName)
        {
            File.WriteAllText(fileName, count.ToString());
        }
    }
}