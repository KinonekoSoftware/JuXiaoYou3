using System.Linq;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents.Basic;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class GeographyDocumentViewModel : DocumentEditorVMBase
    {
        public GeographyDocumentViewModel(){
        
            SetAsMainPictureCommand   = AsyncCommand(SetAsMainPictureImpl);
            ResetAsMainPictureCommand = Command(ResetAsMainPictureImpl);
        }
        
        private async Task SetAsMainPictureImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }
            
            if (!string.IsNullOrEmpty(MainPicture))
            {
                //
                // delete
                RemoveAlbumFromDetailPart(MainPicture);
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, ImageUtilities.ImageScale.Horizontal, AddAlbumToDetailPart);
            if (r.IsFinished)
            {
                MainPicture = r.Value;
            }
        }
        
        private void ResetAsMainPictureImpl()
        {
            
            if (!string.IsNullOrEmpty(MainPicture))
            {
                //
                // delete
                RemoveAlbumFromDetailPart(MainPicture);
            }

            MainPicture = null;
        }
        
        private void ResumeDetailPart()
        {
            if (DetailPart is null)
            {
                return;
            }

            if (DetailPart.DataContext is ViewModelBase vm)
            {
                vm.Resume();
            }
        }
        
        private void AddAlbumToDetailPart(Album album)
        {
            if (album is null)
            {
                return;
            }

            var part = DetailParts.OfType<PartOfAlbum>()
                                  .FirstOrDefault();

            if (part is null)
            {
                return;
            }

            if (part.Items.Any(x => x.Source == album.Source))
            {
                return;
            }

            part.Items.Add(album);
            ResumeDetailPart();
        }

        private void RemoveAlbumFromDetailPart(string album)
        {
            if (album is null)
            {
                return;
            }

            var part = DetailParts.OfType<PartOfAlbum>()
                                  .FirstOrDefault();

            if (part is null)
            {
                return;
            }

            var item = part.Items.FirstOrDefault(x => x.Source == album);

            if (item is null)
            {
                return;
            }

            part.Items
                .Remove(item);
            ResumeDetailPart();
        }
        
        protected override void CreateSubViews(ICollection<HeaderedSubView> collection)
        {
            AddBasicView<GeographyBasicView>(collection);
            AddDetailView(collection);
            AddPartView(collection);
            AddShareView(collection);
        }
        
        protected override void OnCreateDocument(Document document)
        {
            AddDataPartToDocument(CreateAlbum());
            AddDataPartToDocument(CreateStickyNote());
            AddDataPartToDocument(CreatePlaylist());
        }

        public AsyncRelayCommand SetAsMainPictureCommand { get; }
        public RelayCommand ResetAsMainPictureCommand { get; }
    }
}