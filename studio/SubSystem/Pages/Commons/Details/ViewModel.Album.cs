using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class AlbumPartViewModel : AsyncDetailViewModel<PartOfAlbum, Album>
    {
        public AlbumPartViewModel()
        {
            Collection    = new ObservableCollection<Album>();
            ImageEngine = Studio.DatabaseManager()
                                .GetEngine<ImageEngine>();
            
            EditCommand = AsyncCommand<Album>(EditItem, HasItem);
            OpenCommand = Command<Album>(OpenAlbumImpl, HasItem);
        }

        protected override async Task AddItem()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter, true);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            using (var session = Xaml.Get<IBusyService>()
                                     .CreateSession())
            {
                await Task.Run(async () =>
                {
                    await session.Await(SubSystemString.Processing);

                    foreach (var fileName in opendlg.FileNames)
                    {
                        try
                        {
                            var r = await ImageUtilities.Thumbnail(ImageEngine, fileName);
                            if (r.IsFinished)
                            {
                                var thumbnail = r.Value;
                                Sync(thumbnail);
                            }
                            else
                            {
                                this.ErrorNotification(r.Reason);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ErrorNotification(ex.Message.SubString());
                        }
                    }

                    SaveOperation();
                    this.SuccessfulNotification(SubSystemString.OperationOfAddIsSuccessful);
                });
            }
        }

        public async Task EditItem(Album album)
        {
            if (album is null)
            {
                return;
            }

            var r = await SingleLineViewModel.String(SR.EditNameTitle, album.Name);

            if (!r.IsFinished)
            {
                return;
            }

            album.Name = r.Value;
            SaveOperation();
        }

        protected override async Task RemoveItem(Album part)
        {
            if (part is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Collection.Remove(part);
            Detail.Items.Remove(part);
            SaveOperation();
        }

        protected override void ShiftDownItem(Album album)
        {
            Collection.ShiftDown(album);
            SaveOperation();
        }

        protected override void ShiftUpItem(Album album)
        {
            Collection.ShiftUp(album);
            SaveOperation();
        }

        protected override void OnCollectionChanged(Album x)
        {
            if (Collection.Any(y => y.Source == x.Source))
            {
                this.WarningNotification(Language.ItemDuplicatedText);
                return;
            }

            Collection.Add(x);
            Detail!.Items.Add(x);
            Selected ??= Collection.FirstOrDefault();
        }

        private void OpenAlbumImpl(Album part)
        {
            if (part is null)
            {
                return;
            }

            var fileName = ImageEngine.GetSourceFileName(part.Source);
            SubSystem.ImageView(fileName);
        }


        protected override void OnInitialize(ICollection<Album> collection)
        {
            if (Detail.Items is null)
            {
                Xaml.Get<ILogger>()
                    .Warn("PartOfAlbum为空");
                return;
            }

            collection.AddMany(Detail.Items, true);

            var music = Owner.GetImageBlocks()
                             .Select(x => new Album
                             {
                                 Id     = x.Id,
                                 Source = x.TargetSource,
                             });
            collection.AddMany(music);
            UpdateCollectionState();
        }

        protected override void UpdateCommandState()
        {
            OpenCommand.NotifyCanExecuteChanged();
            EditCommand.NotifyCanExecuteChanged();
        }

        public ImageEngine ImageEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Album> OpenCommand { get; }
        public AsyncRelayCommand<Album> EditCommand { get; }
    }
}