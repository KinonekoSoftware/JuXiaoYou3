using Acorisoft.FutureGL.MigaStudio.Pages.Relatives;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class RelationshipViewModel : InTabViewModel
    {
        protected override void Initialize()
        {
            CreatePageFeature<RelativePresetViewModel>(PresetGroup, "global.RelationshipDefinition");
            CreatePageFeature<CharacterRelationshipViewModel>(FeatureGroup, "global.CharacterRelationship");
        }
    }
}