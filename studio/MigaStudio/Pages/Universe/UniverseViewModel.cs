using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Animation;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Controls;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;
using Acorisoft.FutureGL.MigaStudio.Resources.Converters;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using NLog;


namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class UniverseViewModelProxy : BindingProxy<UniverseViewModel>
    {
        
    }
    
    public partial class UniverseViewModel : TabViewModel
    {
        private readonly Subject<Album>   _threadSafeAdding;
        private readonly DatabaseProperty _databaseProperty;

        private Album _selectedAlbum;
        private bool  _exitOperation;

        public UniverseViewModel()
        {
            ColorService      = Xaml.Get<ColorService>();
            Keywords          = new ObservableCollection<string>();
            Mappings          = new ObservableCollection<ColorMapping>();
            PictureCollection = new ObservableCollection<Album>();
            Timelines         = new ObservableCollection<TimelineConcept>();
            IsEmpty              = Selected is null;
            DatabaseManager      = Studio.DatabaseManager();
            ImageEngine          = Studio.Engine<ImageEngine>();
            ProjectEngine        = Studio.Engine<ProjectEngine>();
            _databaseProperty    = Studio.Database()
                                         .Get<DatabaseProperty>();
            SystemSetting = Xaml.Get<SystemSetting>();
            RarityProperty = Studio.Database()
                             .Get<ColorServiceProperty>();
            
            _threadSafeAdding    = new Subject<Album>().DisposeWith(Collector);
            _threadSafeAdding.ObserveOn(Scheduler)
                             .Subscribe(x =>
                             {
                                 if (PictureCollection.Any(y => y.Source == x.Source))
                                 {
                                     this.WarningNotification(Language.ItemDuplicatedText);
                                     return;
                                 }

                                 PictureCollection.Add(x);
                                 _databaseProperty.Album.Add(x);
                                 SelectedAlbum ??= PictureCollection.FirstOrDefault();
                                 Save();
                             })
                             .DisposeWith(Collector);


            if (RarityProperty.Mappings
                        .Count > 0)
            {
                Mappings.AddMany(RarityProperty.Mappings, true);
            }


            AddMappingCommand = AsyncCommand(AddMappingImpl);
            RemoveMappingCommand = AsyncCommand<ColorMapping>(RemoveMappingImpl, HasItem);
            EditMappingCommand   = AsyncCommand<ColorMapping>(EditMappingImpl, HasItem);
            
            AddKeywordCommand    = AsyncCommand<ColorMapping>(AddKeywordImpl, HasItem);
            RemoveKeywordCommand = AsyncCommand<string>(RemoveKeywordImpl,x => Selected is not null && HasItem(x));
            EditKeywordCommand   = AsyncCommand<string>(EditKeywordImpl, x => Selected is not null && HasItem(x));


            CloseDatabaseCommand  = AsyncCommand(CloseDatabaseImpl);
            RemoveDatabaseCommand = AsyncCommand<RepositoryCache>(RemoveDatabaseImpl);
            SwitchDatabaseCommand = AsyncCommand<RepositoryCache>(SwitchDatabaseImpl);
            ChangeAvatarCommand   = AsyncCommand(ChangeAvatarImpl);
            EditCommand           = Command(EditImpl);
            AddAlbumCommand       = AsyncCommand(AddAlbumImpl);
            RemoveAlbumCommand    = AsyncCommand<Album>(RemoveAlbumImpl);
            EditAlbumCommand    = AsyncCommand<Album>(EditAlbumImpl);
            ShiftUpAlbumCommand   = Command<Album>(ShiftUpAlbumImpl);
            ShiftDownAlbumCommand = Command<Album>(ShiftDownAlbumImpl);
            OpenAlbumCommand      = Command<Album>(OpenAlbumImpl);
            
            AddTimelineAgeCommand         = AsyncCommand(AddTimelineAgeImpl);
            AddTimelineAgeBeforeCommand   = AsyncCommand<TimelineConcept>(AddTimelineAgeBeforeImpl);
            AddTimelineAgeAfterCommand    = AsyncCommand<TimelineConcept>(AddTimelineAgeAfterImpl);
            AddTimelineAgeCommand         = AsyncCommand(AddTimelineAgeImpl);
            AddTimelineEventBeforeCommand = AsyncCommand<TimelineConcept>(AddTimelineEventBeforeImpl);
            AddTimelineEventAfterCommand  = AsyncCommand<TimelineConcept>(AddTimelineEventAfterImpl);
            AddTimelineEventCommand       = AsyncCommand(AddTimelineEventImpl);
            RemoveTimelineCommand         = AsyncCommand<TimelineConcept>(RemoveTimelineImpl);
            EditTimelineCommand           = AsyncCommand<TimelineConcept>(EditTimelineImpl);
            ShiftUpTimelineCommand        = Command<TimelineConcept>(ShiftUpTimelineImpl);
            ShiftDownTimelineCommand      = Command<TimelineConcept>(ShiftDownTimelineImpl);
        }

        protected override void OnStart()
        {
            if (_databaseProperty.Album is null)
            {
                _databaseProperty.Album = new ObservableCollection<Album>();
                Save();
            }

            PictureCollection.AddMany(_databaseProperty.Album, true);
            SelectedAlbum = PictureCollection.FirstOrDefault();
            InitializeTimelines();
        }

        protected override void OnResume()
        {
            AlbumButtonUpdateHandler?.Invoke();
            base.OnResume();
        }

        protected override void OnStop()
        {
            if (_exitOperation)
            {
                return;
            }
            
            Save();
        }

        private void EditImpl()
        {
            // Controller.New<UniverseEditorViewModel>();
            this.Obsoleted(Language.GetText("feature.Universe"), 8);
        }

        private async Task CloseDatabaseImpl()
        {
            _exitOperation = true;
            
            var context    = Controller.Context;
            var controller = (TabController)context.MainController;
            controller.Reset();
            controller = (TabController)Controller.Context
                                                  .ControllerMaps[AppViewModel.IdOfQuickStartController];
            // 重置头像缓存
            AvatarConverter.Reset();
            
            // 清空最后一次打开的世界观
            SystemSetting.RepositorySetting
                         .LastRepository = null;
            
            // 保存设置更改
            await SystemSetting.SaveAsync()
                               .ConfigureAwait(true);
            
            
            //
            // 先关闭引擎
            await DatabaseManager.CloseAsync()
                                 .ConfigureAwait(true);
            
            // 切换控制器
            context.SwitchController(controller);
        }
        
        private async Task SwitchDatabaseImpl(RepositoryCache cache)
        {
            if (cache is null)
            {
                return;
            }
            
            _exitOperation = true;
            
            
            var context    = Controller.Context;
            var controller = (TabController)context.MainController;
                
            // 重置头像缓存
            AvatarConverter.Reset();
                
            //
            // 打开次数 +1
            cache.OpenCount++;
                
            //
            // 设置最后一次打开的世界观
            SystemSetting.RepositorySetting
                         .LastRepository = cache.Path;
                
            // 保存设置更改
            await SystemSetting.SaveAsync();

            // 先关闭
            await DatabaseManager.CloseAsync();
            
            // 再打开引擎
            var r  = await DatabaseManager.LoadAsync(cache.Path, Xaml.Get<SystemSetting>()
                                                                     .AdvancedSetting
                                                                     .DebugMode);
            
            // 重置所有内容
            foreach (var controllerMap in context.ControllerMaps)
            {
                if (controllerMap.Value is TabShell shell)
                {
                    shell.Reset();
                }
                else
                {
                    controllerMap.Value
                                 .Stop();
                }
            }
            
            if (r.IsFinished)
            {
                controller = (TabController)Controller.Context
                                                      .ControllerMaps[AppViewModel.IdOfTabShellController];
            }
            else
            {
                controller = (TabController)Controller.Context
                                                      .ControllerMaps[AppViewModel.IdOfQuickStartController];
            }
            
            // 切换控制器
            context.SwitchController(controller);
        }

        private async Task RemoveDatabaseImpl(RepositoryCache cache)
        {
            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            if (cache.Path == Database.DatabaseDirectory)
            {
                this.ErrorNotification(Language.GetText("text.cannotRemoveOpeningDatabase"));
                return;
            }

            //
            // 删除
            SystemSetting.RepositorySetting
                         .Repositories
                         .Remove(cache);

            Repositories.Remove(cache);

            //
            // 保存
            await SystemSetting.SaveAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        private async Task ChangeAvatarImpl()
        {
            await ImageUtilities.Avatar(ImageEngine, x => Icon = x);
        }

        private void Save()
        {
            var cache = SystemSetting.RepositorySetting
                                     .Repositories
                                     .FirstOrDefault(x => x.Path == Database.DatabaseDirectory);

            if (cache is not null)
            {
                cache.Name   = Name;
                cache.Author = Author;
                cache.Intro  = Intro;
            }
            
            Database.Upsert(RarityProperty);
            SetDirtyState(false);
            Database.Upsert(_databaseProperty);
            SystemSetting.Save();
            ColorService.InvalidateDataSource();
        }

        public override void Suspend()
        {
            var cache = SystemSetting.RepositorySetting
                                     .Repositories
                                     .FirstOrDefault(x => x.Path == Database.DatabaseDirectory);

            if (cache is not null)
            {
                cache.Name   = Name;
                cache.Author = Author;
                cache.Intro  = Intro;
            }
            SystemSetting.Save();
            base.Suspend();
        }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _databaseProperty.Intro;
            set
            { 
                var cache = SystemSetting.RepositorySetting
                                       .Repositories
                                       .FirstOrDefault(x => x.Path == Database.DatabaseDirectory);

                if (cache is not null)
                {
                    cache.Intro  = Intro;
                }
                _databaseProperty.Intro = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="ForeignName"/> 属性。
        /// </summary>
        public string ForeignName
        {
            get => _databaseProperty.ForeignName;
            set
            {
                _databaseProperty.ForeignName = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }

        public string Author
        {
            get => _databaseProperty.Author;
            set
            {
                var cache = SystemSetting.RepositorySetting
                                         .Repositories
                                         .FirstOrDefault(x => x.Path == Database.DatabaseDirectory);

                if (cache is not null)
                {
                    cache.Author = Author;
                }
                _databaseProperty.Author = value;
                SetDirtyState();
                RaiseUpdated();
            }
        }

        public string Name
        {
            get => _databaseProperty.Name;
            set
            { 
                var cache = SystemSetting.RepositorySetting
                                       .Repositories
                                       .FirstOrDefault(x => x.Path == Database.DatabaseDirectory);

                if (cache is not null)
                {
                    cache.Name   = Name;
                }
                _databaseProperty.Name = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Icon
        {
            get => _databaseProperty.Icon;
            set
            {
                _databaseProperty.Icon = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }


        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public bool HasRepositories
        {
            get => Repositories.Count > 0;
        }

        //---------------------------------------------
        //
        // Keywords
        //
        //---------------------------------------------


        public DocumentType Type => DocumentType.None;
        
        public AsyncRelayCommand CloseDatabaseCommand { get; }
        public AsyncRelayCommand<RepositoryCache> RemoveDatabaseCommand { get; }
        public AsyncRelayCommand<RepositoryCache> SwitchDatabaseCommand { get; }

        public ObservableCollection<RepositoryCache> Repositories => Xaml.Get<SystemSetting>()
                                                                         .RepositorySetting
                                                                         .Repositories;

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ChangeAvatarCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand EditCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Album> PictureCollection { get; init; }

        public SystemSetting SystemSetting { get; }
        public ProjectEngine ProjectEngine { get; }
        public IDatabase Database => DatabaseUtilities.Database;
        public IDatabaseManager DatabaseManager { get; }
        public ImageEngine ImageEngine { get; }

        public override bool Uniqueness => true;
    }
}