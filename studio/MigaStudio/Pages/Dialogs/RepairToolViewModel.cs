using System.Diagnostics;
using System.IO;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Pages.Dialogs;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class RepairToolViewModel : DialogViewModel
    {
        public RepairToolViewModel()
        {
            BugReportCommand   = Command(BugReportImpl);
            KillProcessCommand = AsyncCommand(KillProcess);
            FixModuleCommand   = Command(FixModuleImpl);
            FixAvatarCommand   = AsyncCommand(ImageUtilities.CropAllAvatar);
        }

        private void BugReportImpl()
        {
            var pendingQueue = Xaml.Get<IPendingQueue>();
            var verbose = pendingQueue.OfType<BugReportVerbose>()
                                      .FirstOrDefault();

            if (verbose is null)
            {
                verbose = new BugReportVerbose
                {
                    Database = string.Empty,
                    Logs = Xaml.Get<ApplicationModel>()
                               .Logs,
                    Bug = BugLevel.Bug
                };
                pendingQueue.Add(verbose);
            }
            else
            {
                verbose.Database = string.Empty;
                verbose.Bug      = BugLevel.Bug;
            }
            
            this.Info(Language.GetText("text.applyWhenRestart"));
        }

        public static Task KillProcess()
        {
            return Task.Run(ProcessUtilities.Kill);
        }

        public static void CreateShortcut()
        {
            var p = Process.GetCurrentProcess();
            if (p.MainModule is null)
            {
                return;
            }

            var dir         = AppDomain.CurrentDomain.BaseDirectory;
            var fileName    = p.MainModule.FileName;
            var lnkFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "橘小柚.lnk");
            FileIO.CreateShortcut(lnkFileName, fileName, dir);
        }

        public static void FixModuleImpl()
        {
            var te = Studio.DatabaseManager()
                         .GetEngine<TemplateEngine>();


            var inside = te.TemplateDB
                           .FindAll()
                           .ToArray();

            foreach (var module in inside)
            {
            }

            te.TemplateDB.DeleteAll();
            te.TemplateCacheDB.DeleteAll();
            te.MetadataCacheDB.DeleteAll();
            inside.ForEach(x => te.AddModule(x));
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand FixAvatarCommand { get; }

        /// <summary>
        /// 修复模组
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand FixModuleCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand BugReportCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand KillProcessCommand { get; }
    }
}