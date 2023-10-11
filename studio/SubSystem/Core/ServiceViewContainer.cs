using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public static partial class ServiceViewContainer
    {
        private static readonly IViewFactoryService _service = new ViewFactoryService();


        public static void Initialize()
        {
            Manual<PartOfRel>(GetCharacterRel);
            MappingDataPart<PartOfAlbum, AlbumPartViewModel, AlbumPartView>();
            MappingDataPart<PartOfPlaylist, PlaylistPartViewModel, PlaylistPartView>();
            MappingDataPart<PartOfSurvey, SurveyPartViewModel, SurveyPartView>();
            MappingDataPart<PartOfSentence, SentencePartViewModel, SentencePartView>();
            MappingDataPart<PartOfPrototype, PrototypePartViewModel, PrototypePartView>();
            MappingDataPart<PartOfAppraise, AppraisePartViewModel, AppraisePartView>();
            MappingDataPart<PartOfStickyNote, StickyNotePartViewModel, StickyNotePartView>();
            UseDetailSetting();

            // MappingSubViewModel<UniversalIntroduction, UniversalIntroductionViewModel, UniversalIntroductionView>();
            // MappingSubViewModel<SpaceConceptOverview, SpaceConceptOverviewViewModel, SpaceConceptOverviewView>();
            // MappingSubViewModel<PropertyOverview, PropertyOverviewViewModel, PropertyOverviewView>();
            // MappingSubViewModel<OtherIntroduction, OtherIntroductionViewModel, OtherIntroductionView>();
            // MappingSubViewModel<SpaceConcept, SpaceConceptViewModel, SpaceConceptView>();
            // MappingSubViewModel<BrowsableProperty, BrowsablePropertyViewModel, BrowsablePropertyView>();
            // MappingSubViewModel<DeclarationConcept, DeclarationConceptViewModel, DeclarationConceptView>();
            // MappingSubViewModel<RarityConcept, RarityConceptViewModel, RarityConceptView>();

            Prepare<PartOfAlbum>();
            Prepare<PartOfAppraise>();
            Prepare<PartOfPrototype>(false);
            Prepare<PartOfPlaylist>();
            Prepare<PartOfRel>();
            Prepare<PartOfSentence>();
            Prepare<PartOfStickyNote>();
            Prepare<PartOfSurvey>();
        }


        private static FrameworkElement GetCharacterRel(object owner, object instance)
        {
            var d = (DocumentEditorVMBase)owner;
            return new CharacterRelshipPartView
            {
                DataContext = new CharacterRelPartViewModel(d, (PartOfRel)instance)
            };
        }

        private static void Manual<TPart>(ViewFactory factory)
        {
            _service.Manual<TPart>(factory);
        }

        public static FrameworkElement Build(object parent, object data)
        {
            return _service.GetView(parent, data);
        }

        /// <summary>
        /// 映射数据部件
        /// </summary>
        /// <typeparam name="TPart"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TView"></typeparam>
        private static void MappingDataPart<TPart, TViewModel, TView>()
            where TPart : DataPart
            where TViewModel : IsolatedViewModel<DocumentEditorBase, TPart>, new()
            where TView : FrameworkElement, new()
        {
            _service.Isolate<DocumentEditorBase, TPart, TView, TViewModel>();
        }

        // private static void MappingSubViewModel<TPart, TViewModel, TView>()
        //     where TPart : class
        //     where TViewModel : IsolatedViewModel<UniverseEditorViewModel, TPart>, new()
        //     where TView : FrameworkElement, new()
        // {
        //     _service.Isolate<UniverseEditorViewModel, TPart, TView, TViewModel>();
        // }

        private static void UseDetailSetting()
        {
            _service.Direct<DocumentEditorBase, DetailPartSettingPlaceHolder, DetailPartSettingView>();
        }
    }
}