using System.Diagnostics;
using System.Linq;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Exceptions;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class TabShell : ShellCore
    {
        public TabShell()
        {
            SelectInactiveWorkspaceCommand = AsyncCommand(SelectInactiveWorkspaceImpl);
        }

        protected override void OnAddViewModel(ITabViewModel viewModel)
        {
            SelectInactiveWorkspaceCommand.NotifyCanExecuteChanged();
            base.OnAddViewModel(viewModel);
        }

        protected override void OnRemoveViewModel(ITabViewModel viewModel)
        {
            SelectInactiveWorkspaceCommand.NotifyCanExecuteChanged();
            base.OnRemoveViewModel(viewModel);
        }

        protected override void OnCurrentViewModelChanged(ITabViewModel oldViewModel, ITabViewModel newViewModel)
        {
            SelectInactiveWorkspaceCommand.NotifyCanExecuteChanged();
            base.OnCurrentViewModelChanged(oldViewModel, newViewModel);
        }

        private async Task SelectInactiveWorkspaceImpl()
        {
            if (InactiveWorkspace.Count == 0)
            {
                await this.WarningNotification(Language.GetText("text.noInactiveWorkspace"));
                return;
            }

            var r = await SubSystem.Selection<TabViewModel>(
                SubSystemString.SelectTitle,
                InactiveWorkspace.FirstOrDefault(),
                InactiveWorkspace);

            if (!r.IsFinished)
            {
                return;
            }

            if (Workspace.Count <= 0)
            {
                return;
            }

            var lastActiveWorkspace     = Workspace[^1];
            var selectInactiveWorkspace = r.Value;

            var index = InactiveWorkspace.IndexOf(selectInactiveWorkspace);
            InactiveWorkspace[index] = lastActiveWorkspace;
            Workspace[^1]            = selectInactiveWorkspace;
            CurrentViewModel         = selectInactiveWorkspace;
        }

        protected override async void StartOverride()
        {
            base.StartOverride();
            
#if DEBUG
            // New<WeaponEditorViewModel>();
            // New<PlanetEditorViewModel>();
            // New<MaterialEditorViewModel>();
            // New<UniverseEditorViewModel>();
            // New<KeywordViewModel>();
#endif

            var dbMgr = Xaml.Get<IDatabaseManager>();
            
            if (dbMgr.NeedUpdate
                     .CurrentValue)
            {
                await PerformanceUpgrade(dbMgr);
            }
        }
        
        private async Task PerformanceUpgrade(IDatabaseManager dbMgr)
        {
            var bs       = Xaml.Get<IBusyService>();
            var logger   = Xaml.Get<ILogger>();
            var database = dbMgr.Database
                                .CurrentValue;
            var version = database.Version;
                    
            using (var session = bs.CreateSession())
            {
                session.Update(SubSystemString.Updating);
                await session.Await();
                logger.Warn($"正在升级数据库，当前版本为:{version}");

                foreach (var updater in dbMgr.Updaters)
                {
                    try
                    {
                        if (updater.TargetVersion >= database.Version)
                        {
                            if (!updater.Update(database))
                            {
                                logger.Warn($"数据库升级失败，升级器:{updater.GetType().FullName}");
                                continue;
                            }

                            if (updater.ResultVersion > database.Version)
                            {
                                database.UpdateVersion(updater.ResultVersion);
                                logger.Warn($"数据库升级完成，当前版本为:{updater.ResultVersion}");
                            }
                            else
                            {
                                logger.Error($"升级数据库异常，疑似升级器未手动提升数据库版本，内存版本:{version}，本地版本:{database.Version}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"升级数据库失败，当前版本为:{version}");
                        throw new UpdaterException(ex.Message, ex);
                    }
                }
            }
        }

        protected override void RequireStartupTabViewModel()
        {
            New<HomeViewModel>();
            
        }

        public sealed override string Id => AppViewModel.IdOfTabShellController;

        /// <summary>
        /// 选择
        /// </summary>
        public AsyncRelayCommand SelectInactiveWorkspaceCommand { get; }
    }
}