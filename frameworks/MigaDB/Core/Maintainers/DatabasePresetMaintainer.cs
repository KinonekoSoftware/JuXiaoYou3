using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Relatives;
using Acorisoft.FutureGL.MigaDB.Utils;

// ReSharper disable All

namespace Acorisoft.FutureGL.MigaDB.Core.Maintainers
{
    public class DatabasePresetMaintainer : DatabaseMaintainer<PresetProperty>
    {
        
        protected override PresetProperty OnCreateInstance()
        {
            var pp = new PresetProperty
            {
                RelativePresets           = new ObservableCollection<RelativePreset>(),
                ModulePresets             = new ObservableCollection<ModulePreset>(),
                DefaultModulePreset       = new Dictionary<DocumentType, string>(),
                DefaultPresentationPreset = new Dictionary<DocumentType, PartOfPresentation>()
            };

            InstallRelatives(pp);

            return pp;
        }


        private static void InstallRelatives(PresetProperty preset)
        {
            const string SonAndDaughter           = "儿子/ 女儿";
            const string GrandSonAndGrandDaughter = "孙子/ 孙女";
            const string Te = "侄子/ 侄女";
            
            InstallDirectRelative(preset, "父亲", "父亲", SonAndDaughter);
            InstallDirectRelative(preset, "继父", "继父", SonAndDaughter);
            InstallDirectRelative(preset, "养父", "养父", SonAndDaughter);
            InstallDirectRelative(preset, "母亲", "母亲", SonAndDaughter);
            InstallDirectRelative(preset, "继母", "继母", SonAndDaughter);
            InstallDirectRelative(preset, "养母", "养母", SonAndDaughter);
            
            InstallDirectRelative(preset, "母亲", "伯伯", GrandSonAndGrandDaughter);
            InstallDirectRelative(preset, "外婆", "伯伯", GrandSonAndGrandDaughter);
            InstallDirectRelative(preset, "外公", "叔叔", GrandSonAndGrandDaughter);
            InstallDirectRelative(preset, "爷爷", "伯伯", GrandSonAndGrandDaughter);
            InstallDirectRelative(preset, "奶奶", "叔叔", GrandSonAndGrandDaughter);
            
            InstallCollateralRelative(preset, "伯伯", "伯伯", Te);
            InstallCollateralRelative(preset, "叔叔", "叔叔", Te);
            InstallCollateralRelative(preset, "婶婶", "婶婶", Te);
            InstallCollateralRelative(preset, "阿姨", "阿姨", Te);
            InstallCollateralRelative(preset, "姑姑", "姑姑", Te);
            InstallCollateralRelative(preset, "姨丈", "姨丈", Te);
            
            
            InstallConjugalRelative(preset, "夫妻", "丈夫", "妻子");
            InstallConjugalRelative(preset, "未婚夫妻", "未婚夫", "未婚妻");
            InstallConjugalRelative(preset, "对象", "对象", "对象");
            InstallConjugalRelative(preset, "暗恋", "喜欢", "朋友 / 陌生人");
        }

        protected static void InstallDirectRelative(PresetProperty preset, string name, string callToSource, string callToTarget)
        {
            var rp = new RelativePreset
            {
                Id                 = ID.Get(),
                CallOfSource       = callToSource,
                CallOfTarget       = callToTarget,
                DirectRelative     = true,
                CollateralRelative = false,
                ConjugalRelative   = false,
                Name               = name
            };
            
            preset.RelativePresets.Add(rp);
        }
        
        protected static void InstallCollateralRelative(PresetProperty preset, string name, string callToSource, string callToTarget)
        {
            var rp = new RelativePreset
            {
                Id                 = ID.Get(),
                CallOfSource       = callToSource,
                CallOfTarget       = callToTarget,
                DirectRelative     = false,
                CollateralRelative = true,
                ConjugalRelative   = false,
                Name               = name
            };
            preset.RelativePresets.Add(rp);
        }
        
        protected static void InstallConjugalRelative(PresetProperty preset, string name, string callToSource, string callToTarget)
        {
            var rp = new RelativePreset
            {
                Id                 = ID.Get(),
                CallOfSource       = callToSource,
                CallOfTarget       = callToTarget,
                DirectRelative     = false,
                CollateralRelative = false,
                ConjugalRelative   = true,
                Name               = name
            };
            preset.RelativePresets.Add(rp);
        }

        protected override void OnFixInstance(PresetProperty instance)
        {
        }

        protected override bool IsInvalidated(PresetProperty instance)
        {
            return false;
        }
    }
}