using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages.Relatives;
using Acorisoft.FutureGL.MigaStudio.Pages.Tools;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ToolsViewModelProxy : BindingProxy<ToolsViewModel>
    {
    }

    public class ToolsViewModel : InTabViewModel
    {
        protected override void Initialize()
        {
            // CreatePageFeature<CharacterRelationshipViewModel>(string.Empty, "__CharacterRelationship", null);
            // CreateDialogFeature<DirectoryManagerViewModel>(string.Empty, "__DirectoryStatistic", null);
            CreateDialogFeature<RepairToolViewModel>(ToolsGroup, "global.repair", null);
            CreatePageFeature<KeywordViewModel>(FeatureGroup, "__Keywords", null);
            CreatePageFeature<BookmarkViewModel>(FeatureGroup, "__Bookmark", null);
            CreatePageFeature<MessageBoardViewModel>(FeatureGroup, "__MessageBoard", null);
            CreatePageFeature<DocumentRecycleBinViewModel>(FeatureGroup, "__RecycleBin", null);
        }
    }
}