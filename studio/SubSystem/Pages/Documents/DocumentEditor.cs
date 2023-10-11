using System.Windows;
using System.Windows.Media;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents.Share;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class DocumentEditorViewModelProxy : BindingProxy<DocumentEditorVMBase>
    {
    }

    public abstract class DocumentEditorVMBase : DocumentEditorBase
    {
        protected static void AddSubView<TView>(ICollection<HeaderedSubView> collection, string id, string nc, string hc) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Name           = Language.GetText(id),
                Type           = typeof(TView),
                DefaultColor   = new SolidColorBrush(Xaml.FromHex(nc)),
                HighlightColor = new SolidColorBrush(Xaml.FromHex(hc))
            });
        }

        protected static void AddBasicView<TView>(ICollection<HeaderedSubView> collection) where TView : FrameworkElement
        {
            AddSubView<TView>(collection, "text.DocumentEditor.Basic", "#89a12b", "#89a12b");
        }

        protected static void AddPartView(ICollection<HeaderedSubView> collection)
        {
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart", "#89a12b", "#89a12b");
        }

        protected static void AddDetailView(ICollection<HeaderedSubView> collection)
        {
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail", "#89a12b", "#89a12b");
        }

        protected static void AddShareView(ICollection<HeaderedSubView> collection)
        {
            AddSubView<ShareView>(collection, "text.DocumentEditor.Presentation", "#89a12b", "#89a12b");
        }

        protected override IEnumerable<object> CreateDetailPartList()
        {
            return new object[]
            {
                CreateAlbum(),
                CreatePlaylist(),
                CreateStickyNote(),
                CreatePrototype(),
                CreateSurvey(),
            };
        }

        protected override void IsDataPartExistence(Document document)
        {
            HasDataPart<PartOfAlbum>(CreateAlbum);
            HasDataPart<PartOfPlaylist>(CreatePlaylist);
            HasDataPart<PartOfStickyNote>(CreateStickyNote);
        }

        protected sealed override void OnSubViewChanged(HeaderedSubView oldValue, HeaderedSubView newValue)
        {
            if (newValue is null)
            {
                return;
            }

            newValue.Create(this);
            SubView = newValue.SubView;

            if (newValue.Type == typeof(ShareView))
            {
                Preshapes.ForEach(x => (x.DataContext as PreshapeViewModelBase)?.Resume());
                RefreshPresentation();
            }
        }

        public static PartOfAlbum CreateAlbum() => new PartOfAlbum { Items                = new List<Album>() };
        public static PartOfPlaylist CreatePlaylist() => new PartOfPlaylist { Items       = new List<Music>() };
        public static PartOfStickyNote CreateStickyNote() => new PartOfStickyNote { Items = new List<StickyNote>() };
        public static PartOfPrototype CreatePrototype() => new PartOfPrototype();
        public static PartOfSurvey CreateSurvey() => new PartOfSurvey { Items = new List<SurveySet>() };
        public static PartOfAppraise CreateAppraise() => new PartOfAppraise();
        public static PartOfSentence CreateSentence() => new PartOfSentence();
        public static PartOfRel CreateCharacterRelatives() => new PartOfRel();
    }
}