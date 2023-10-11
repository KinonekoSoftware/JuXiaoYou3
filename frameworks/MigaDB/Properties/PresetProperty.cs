using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data.Relatives;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaDB.Data
{
    public class PresetProperty : ObservableObject
    {
        #region ModulePreset

        

        public ModulePreset GetModulePreset(DocumentType type)
        {
            return DefaultModulePreset.ContainsKey(type) ? 
                ModulePresets.FirstOrDefault(x => x.Id == DefaultModulePreset[type]) :
                null;
        }
        
        public void SetModulePreset(DocumentType type, ModulePreset preset)
        {
            if (preset is null)
            {
                return;
            }

            preset.Type               = type;
            DefaultModulePreset[type] = preset.Id;
        }
        
        #endregion

        #region PresentationPreset

        
        public void SetPresentationPreset(DocumentType type, PartOfPresentation manifest)
        {
            if (manifest is null)
            {
                return;
            }

            DefaultPresentationPreset[type] = manifest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback">保存新建的展示清单回调</param>
        /// <returns></returns>
        public PartOfPresentation GetPresentationPreset(DocumentType type, Action<PresetProperty> callback)
        {
            if (!DefaultPresentationPreset.TryGetValue(type, out var pop))
            {
                pop = new PartOfPresentation
                {
                    Id     = ID.Get(),
                    Blocks = new ObservableCollection<Presentation>()
                };
                DefaultPresentationPreset.Add(type, pop);
                callback?.Invoke(this);
            }

            return pop;
        }

        #endregion

        public Dictionary<DocumentType, string> DefaultModulePreset { get; init; }
        public Dictionary<DocumentType, PartOfPresentation> DefaultPresentationPreset { get; init; }

        public ObservableCollection<ModulePreset> ModulePresets { get; init; }
        public ObservableCollection<RelativePreset> RelativePresets { get; init; }
        public ObservableCollection<ObjectEntry> EntryPresets { get; init; }
        public string LastSocialCharacterID { get; set; }
    }
}