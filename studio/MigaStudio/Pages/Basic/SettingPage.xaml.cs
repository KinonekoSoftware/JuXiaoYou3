using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;
using Languager = Acorisoft.FutureGL.Forest.Services.Language;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    [Connected(View = typeof(SettingPage), ViewModel = typeof(SettingViewModel))]
    public partial class SettingPage : ForestUserControl
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        private void Button_BugReport(object sender, System.Windows.RoutedEventArgs e)
        {
            var pendingQueue = Xaml.Get<IPendingQueue>();
            var verbose = pendingQueue.OfType<BugReportVerbose>()
                                      .FirstOrDefault();

            if (CannotUseBug.IsChecked == true)
            {
                if (verbose is null)
                {
                    verbose = new BugReportVerbose
                    {
                        Database = Studio.Database()
                                         .DatabaseDirectory,
                        Logs = Xaml.Get<ApplicationModel>()
                                   .Logs,
                        Bug = BugLevel.Bug
                    };
                    pendingQueue.Add(verbose);
                }
                else
                {
                    verbose.Database = Studio.Database()
                                             .DatabaseDirectory;
                    verbose.Bug = BugLevel.Bug;
                }

                
                this.ViewModel<SettingViewModel>()
                    .Info(Languager.GetText("text.applyWhenRestart"));
                return;
            }

            if (CrashBugButCanContinue.IsChecked == true)
            {
                if (verbose is null)
                {
                    verbose = new BugReportVerbose
                    {
                        Database = Studio.Database()
                                         .DatabaseDirectory,
                        Logs = Xaml.Get<ApplicationModel>()
                                   .Logs,
                        Bug = BugLevel.NotImplemented
                    };
                    pendingQueue.Add(verbose);
                }
                else
                {
                    verbose.Database = Studio.Database()
                                             .DatabaseDirectory;
                    verbose.Bug = BugLevel.NotImplemented;
                }

                this.ViewModel<SettingViewModel>()
                    .Info(Languager.GetText("text.applyWhenRestart"));
                return;
            }

            if (CrashBug.IsChecked == true)
            {
                if (verbose is null)
                {
                    verbose = new BugReportVerbose
                    {
                        Database = Studio.Database()
                                         .DatabaseDirectory,
                        Logs = Xaml.Get<ApplicationModel>()
                                   .Logs,
                        Bug = BugLevel.Crash
                    };
                    pendingQueue.Add(verbose);
                }
                else
                {
                    verbose.Database = Studio.Database()
                                             .DatabaseDirectory;
                    verbose.Bug = BugLevel.Crash;
                }

                this.ViewModel<SettingViewModel>()
                    .Info(Languager.GetText("text.applyWhenRestart"));
                return;
            }

            this.ViewModel<SettingViewModel>()
                .WarningNotification("请选择一个反馈类型！");
        }
    }
}