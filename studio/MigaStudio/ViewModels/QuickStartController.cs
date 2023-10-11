using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Core.Migrations;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Resources.Updaters;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class QuickStartController : TabController
    {
        private string _name;
        private string _foreignName;
        private string _author;
        private string _cover;
        private string _icon;
        private string _intro;

        public QuickStartController()
        {
            CreateCommand  = AsyncCommand(CreateImpl);
            OpenCommand    = AsyncCommand(OpenImpl);
            UpgradeCommand = AsyncCommand(UpgradeImpl);
            RepairCommand  = AsyncCommand(RepairImpl);
        }

        private static async Task RepairImpl()
        {
            await DialogService().Dialog(new RepairToolViewModel());
        }

        private async Task CreateImpl()
        {
            var opendlg = new VistaFolderBrowserDialog();

            if (string.IsNullOrEmpty(Author))
            {
                 await this.Error(SubSystemString.EmptyAuthor);
                 return;
            }
            
            
            if (string.IsNullOrEmpty(Name))
            {
                await this.Error(SubSystemString.EmptyName);
                return;
            }
            
            if (string.IsNullOrEmpty(ForeignName))
            {
                await this.Error(SubSystemString.EmptyName);
                return;
            }

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var folder = opendlg.SelectedPath;
            var dbMgr = Studio.DatabaseManager();
            var property = new DatabaseProperty
            {
                Id          = ID.Get(),
                Name        = Name,
                ForeignName = ForeignName,
                Author      = Author,
                Icon        = Icon,
                Cover       = Cover,
            };
            
            var result = await dbMgr.CreateAsync(folder, property);

            if (result.IsFinished)
            {
                var cache = new RepositoryCache
                {
                    Id     = property.Id,
                    Name   = Name,
                    Author = Author,
                    Intro = property.Intro,
                    Path = folder,
                    OpenCount = 1
                };

                await Xaml.Get<ISystemSetting>()
                          .AddRepository(cache);

                Context.SwitchController(Context.MainController);                
            }
            else
            {
                var reason = result.Reason;
                await this.WarningNotification(SubSystemString.GetDatabaseResult(reason));
            }
        }

        private async Task OpenImpl()
        {
            var opendlg = new VistaFolderBrowserDialog();

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var folder = opendlg.SelectedPath;
            var dbMgr  = Studio.DatabaseManager();
            
            var result = await dbMgr.LoadAsync(folder, Xaml.Get<SystemSetting>()
                                                           .AdvancedSetting
                                                           .DebugMode);

            if (result.IsFinished)
            {
                var property = dbMgr.Property
                                    .CurrentValue;
                
                var cache = new RepositoryCache
                {
                    Id        = property.Id,
                    Name      = Name,
                    Author    = Author,
                    Intro     = property.Intro,
                    Path      = folder,
                    OpenCount = 1
                };

                await Xaml.Get<ISystemSetting>()
                          .AddRepository(cache);

                Context.SwitchController(Context.MainController);
            }
            else
            {
                var reason = result.Reason;
                await this.Warning(SubSystemString.GetDatabaseResult(reason));
            }
        }

        private async Task UpgradeImpl()
        {
            var opendlg = new VistaFolderBrowserDialog
            {
                Description = "选择旧版世界观的位置",
                UseDescriptionForTitle = true
            };

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var databaseFilePath = opendlg.SelectedPath;
            
            
            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            
            var targetFilePath = opendlg.SelectedPath;
            var r              = await V209DatabaseUpdater.Update(Studio.DatabaseManager(), databaseFilePath, targetFilePath);

            if (r.IsFinished)
            {
                Context.SwitchController(Context.MainController);
            }
            else
            {
                this.ErrorNotification(Language.GetText("text.Updating.Failed"));
            }
        }


        /// <summary>
        /// 获取或设置 <see cref="Cover"/> 属性。
        /// </summary>
        public string Cover
        {
            get => _cover;
            set => SetValue(ref _cover, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Author"/> 属性。
        /// </summary>
        public string Author
        {
            get => _author;
            set
            {
                SetValue(ref _author, value);
                CreateCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="ForeignName"/> 属性。
        /// </summary>
        public string ForeignName
        {
            get => _foreignName;
            set
            {
                SetValue(ref _foreignName, value);
                CreateCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                SetValue(ref _name, value);
                CreateCommand.NotifyCanExecuteChanged();
            }
        }


        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Icon"/> 属性。
        /// </summary>
        public string Icon
        {
            get => _icon;
            set => SetValue(ref _icon, value);
        }

        public AsyncRelayCommand CreateCommand { get; }
        public AsyncRelayCommand OpenCommand { get; }
        public AsyncRelayCommand RepairCommand { get; }
        public AsyncRelayCommand UpgradeCommand { get; }
        public sealed override string Id => AppViewModel.IdOfQuickStartController;
    }
}