using System.Linq;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents.Share;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class CharacterDocumentViewModel : DocumentEditorVMBase
    {
        public CharacterDocumentViewModel() : base()
        {
            SetAsMainPictureOfSquareCommand       = AsyncCommand(SetAsMainPictureOfSquareImpl);
            SetAsMainPictureOfHorizontalCommand   = AsyncCommand(SetAsMainPictureOfHorizontalImpl);
            SetAsMainPictureOfVerticalCommand     = AsyncCommand(SetAsMainPictureOfVerticalImpl);
            ResetAsMainPictureOfHorizontalCommand = Command(ResetAsMainPictureOfHorizontalImpl);
            ResetAsMainPictureOfVerticalCommand = Command(ResetAsMainPictureOfVerticalImpl);
            ResetAsMainPictureOfSquareCommand = Command(ResetAsMainPictureOfSquareImpl);
        }

        // TODO: 人物关系中的血缘关系
        protected override void CreateSubViews(ICollection<HeaderedSubView> collection)
        {
            AddBasicView<CharacterBasicView>(collection);
            AddDetailView(collection);
            AddPartView(collection);
            AddShareView(collection);
            Preshapes.Clear();
            // Preshapes.Add(new CharacterPergamynStyleView
            // {
            //     Tag = Language.GetText("preshape.character.Pergamyn")
            // });
        }

        protected override void IsDataPartExistence(Document document)
        {
            HasDataPart<PartOfRel>(CreateCharacterRelatives);
            HasDataPart<PartOfSentence>(CreateSentence);
            HasDataPart<PartOfAppraise>(CreateAppraise);
            base.IsDataPartExistence(document);
        }

        protected override IEnumerable<object> CreateDetailPartList()
        {
            return new object[]
            {
                CreateAlbum(),
                CreatePlaylist(),
                CreateAppraise(),
                CreateSentence(),
                CreateStickyNote(),
                CreatePrototype(),
                CreateCharacterRelatives(),
                CreateSurvey(),
            };
        }

        protected override void OnCreateDocument(Document document)
        {
            AddDataPartToDocument(CreateStickyNote());
            AddDataPartToDocument(CreateAlbum());
            AddDataPartToDocument(CreatePlaylist());
            AddDataPartToDocument(CreateCharacterRelatives());
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

        #region MainPicture

        
        
        private async Task SetAsMainPictureOfVerticalImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            if (!string.IsNullOrEmpty(MainPictureOfVertical))
            {
                //
                // delete
                RemoveAlbumFromDetailPart(MainPictureOfVertical);
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, ImageUtilities.ImageScale.Vertical, AddAlbumToDetailPart);
            if (r.IsFinished)
            {
                MainPictureOfVertical = r.Value;
            }
        }

        private void ResetAsMainPictureOfHorizontalImpl()
        {
            
            if (!string.IsNullOrEmpty(MainPictureOfHorizontal))
            {
                //
                // delete
                RemoveAlbumFromDetailPart(MainPictureOfHorizontal);
            }

            MainPictureOfHorizontal = null;
        }
        
        private void ResetAsMainPictureOfVerticalImpl()
        {
            
            if (!string.IsNullOrEmpty(MainPictureOfVertical))
            {
                //
                // delete
                RemoveAlbumFromDetailPart(MainPictureOfVertical);
            }

            MainPictureOfVertical = null;
        }
        
        private void ResetAsMainPictureOfSquareImpl()
        {
            
            if (!string.IsNullOrEmpty(MainPictureOfSquare))
            {
                //
                // delete
                RemoveAlbumFromDetailPart(MainPictureOfSquare);
            }

            MainPictureOfSquare = null;
        }
        
        private async Task SetAsMainPictureOfHorizontalImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }
            
            if (!string.IsNullOrEmpty(MainPictureOfHorizontal))
            {
                //
                // delete
                RemoveAlbumFromDetailPart(MainPictureOfHorizontal);
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, ImageUtilities.ImageScale.Horizontal, AddAlbumToDetailPart);
            if (r.IsFinished)
            {
                MainPictureOfHorizontal = r.Value;
            }
        }

        private async Task SetAsMainPictureOfSquareImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }
            if (!string.IsNullOrEmpty(MainPictureOfSquare))
            {
                //
                // delete
                RemoveAlbumFromDetailPart(MainPictureOfSquare);
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, ImageUtilities.ImageScale.Square, AddAlbumToDetailPart);
            if (r.IsFinished)
            {
                MainPictureOfSquare = r.Value;
            }
        }

        #endregion

        public AsyncRelayCommand SetAsMainPictureOfVerticalCommand { get; }
        public AsyncRelayCommand SetAsMainPictureOfHorizontalCommand { get; }
        public AsyncRelayCommand SetAsMainPictureOfSquareCommand { get; }
        public RelayCommand ResetAsMainPictureOfVerticalCommand { get; }
        public RelayCommand ResetAsMainPictureOfHorizontalCommand { get; }
        public RelayCommand ResetAsMainPictureOfSquareCommand { get; }
    }
}