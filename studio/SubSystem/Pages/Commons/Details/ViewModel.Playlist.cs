using System.IO;
using System.Linq;
using System.Windows.Media;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Services;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using NLog;

// ReSharper disable ConvertToUsingDeclaration

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class PlaylistPartViewModel : AsyncDetailViewModel<PartOfPlaylist, Music>
    {
        private static readonly DrawingImage MusicDrawing;

        static PlaylistPartViewModel()
        {
            var music = Geometry.Parse("F1 M24,24z M0,0z M22,2L6,2 6,18 22,18 22,2z M18,7L15,7 15,12.5A2.5,2.5,0,0,1,10,12.5A2.5,2.5,0,0,1,12.5,10C13.07,10,13.58,10.19,14,10.51L14,5 18,5 18,7z M4,6L2,6 2,22 18,22 18,20 4,20 4,6z");
            var drawing = new GeometryDrawing
            {
                Geometry = music,
                Brush    = new SolidColorBrush(Colors.Gray)
            };
            MusicDrawing = new DrawingImage
            {
                Drawing = drawing,
            };
        }

        public PlaylistPartViewModel()
        {
            Collection = new ObservableCollection<Music>();
            MusicEngine = Studio.DatabaseManager()
                              .GetEngine<MusicEngine>();
            PlayCommand = Command<Music>(PlayItem, HasItem);
            PauseCommand = Command(PauseItem, () => Xaml.Get<MusicService>()
                                                        .IsPlaying
                                                        .CurrentValue);
        }

        protected override void OnInitialize(ICollection<Music> collection)
        {
            if (Detail.Items is null)
            {
                Xaml.Get<ILogger>()
                    .Warn("PartOfAlbum为空");
                return;
            }

            collection.AddMany(Detail.Items, true);
            var music = Owner.GetMusicBlocks()
                             .Where(x => !string.IsNullOrEmpty(x.TargetSource) &&
                                         File.Exists(x.TargetSource))
                             .Select(x => new Music
                             {
                                 Id   = x.Id,
                                 Name = x.TargetName,
                                 Path = x.TargetSource,
                                 Cover = x.TargetThumbnail
                             });
            collection.AddMany(music);
            UpdateCollectionState();
        }

        protected override void OnCollectionChanged(Music x)
        {
            if (Collection.Any(y => y.Id == x.Id))
            {
                this.WarningNotification(Language.ItemDuplicatedText);
                return;
            }

            Collection.Add(x);
            Detail!.Items.Add(x);

            //
            //
            Selected ??= Collection.FirstOrDefault();
            this.SuccessfulNotification(SubSystemString.OperationOfAddIsSuccessful);
            SaveOperation();
        }

        protected override async Task AddItem()
        {
            var opendlg = Studio.Open(SubSystemString.MusicFilter, true);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            using (var session = Xaml.Get<IBusyService>()
                                     .CreateSession())
            {
                session.Update(SubSystemString.Processing);

                await Task.Run(async () =>
                {
                    await session.Await();
                    await MusicUtilities.AddMusic(opendlg.FileNames, MusicEngine, Sync);
                });
            }
        }


        protected override async Task RemoveItem(Music part)
        {
            if (part is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }
            
            RemoveImpl(part);
        }

        private void RemoveImpl(Music item)
        {
            var ms = Xaml.Get<MusicService>();
            if (ms.IsPlaying
                  .CurrentValue)
            {
                ms.Playlist
                  .CurrentValue
                  .Items
                  .Remove(item);
            }

            Collection.Remove(item);
            SaveOperation();
        }

        private void PlayItem(Music part)
        {
            if (part is null)
            {
                return;
            }

            try
            {
                if (!File.Exists(part.Path))
                {
                    this.Obsoleted(string.Format(Language.GetText("text.FileNotFound"), part?.Name));
                    RemoveImpl(part);
                    return;
                }
                
                var ms = Xaml.Get<MusicService>();

                if (ms.IsPlaying
                      .CurrentValue)
                {
                    if (ms.Music.CurrentValue == part)
                    {
                        return;
                    }

                    var hash = ms.Playlist
                                 .CurrentValue
                                 .Items
                                 .Select(x => x.Id)
                                 .ToHashSet();

                    var noAddedValue = Collection.Where(x => !hash.Contains(x.Id));

                    ms.Playlist
                      .CurrentValue
                      .Items
                      .AddMany(noAddedValue);

                    ms.Play(part);
                }
                else
                {
                    ms.SetPlaylist(new Playlist
                    {
                        Name  = Owner.Name,
                        Items = new ObservableCollection<Music>(Collection)
                    }, true);
                }
            }
            catch (NAudio.MmException)
            {

            }
        }


        private void PauseItem()
        {
            var ms = Xaml.Get<MusicService>();

            if (ms.IsPlaying
                  .CurrentValue)
            {
                ms.Pause();
            }
        }

        protected override void ShiftDownItem(Music album)
        {
            Collection.ShiftDown(album);
            Save();
        }

        protected override void ShiftUpItem(Music album)
        {
            Collection.ShiftUp(album);
            SaveOperation();
        }

        protected override void UpdateCommandState()
        {
            PlayCommand.NotifyCanExecuteChanged();
            PauseCommand.NotifyCanExecuteChanged();
        }

        public bool AutoPlay
        {
            get => Detail.AutoPlay;
            set
            {
                Detail.AutoPlay = value;
                RaiseUpdated();
            }
        }

        public MusicEngine MusicEngine { get; }

        /// <summary>
        /// 获取或设置 <see cref="DefaultAlbum"/> 属性。
        /// </summary>
        public ImageSource DefaultAlbum
        {
            get => MusicDrawing;
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }


        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Music> PlayCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand PauseCommand { get; }
    }
}