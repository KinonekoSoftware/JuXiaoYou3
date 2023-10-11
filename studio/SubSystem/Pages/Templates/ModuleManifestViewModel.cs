using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Templates
{
    public class ModuleManifestViewModel : DialogViewModel
    {
        private ModulePreset _selectedPreset;
        
        public ModuleManifestViewModel()
        {
            Property = Studio.Database()
                             .Get<PresetProperty>();
            
            
            AddManifestCommand    = AsyncCommand(AddManifestImpl);
            EditManifestCommand   = AsyncCommand<ModulePreset>(EditManifestImpl, x => x is not null);
            RemoveManifestCommand = AsyncCommand<ModulePreset>(RemoveManifestImpl, x => x is not null);
            AddTemplateCommand    = AsyncCommand(AddTemplateImpl);
            RemoveTemplateCommand = Command<ModuleTemplateCache>(RemoveTemplateImpl);
        }

        private void Save()
        {
            Studio.Database()
                  .Upsert(Property);
        }

        private async Task AddManifestImpl()
        {
            var ds = DialogService();
            var r  = await SingleLineViewModel.String(SubSystemString.EditNameTitle);

            if (!r.IsFinished)
            {
                return;
            }

            var r1 = await ds.Dialog<IEnumerable<ModuleTemplateCache>, ModuleSelectorViewModel>(new Parameter
            {
                Args = new object[]
                {
                    Studio.Engine<TemplateEngine>()
                          .TemplateCacheDB
                          .FindAll()
                }
            });
            if (!r1.IsFinished)
            {
                return;
            }

            var manifest = new ModulePreset
            {
                Id        = ID.Get(),
                Name      = r.Value,
                Templates = new ObservableCollection<ModuleTemplateCache>(r1.Value)
            };
            
            Manifests.Add(manifest);
            Save();
        }
        
        private async Task EditManifestImpl(ModulePreset preset)
        {
            if (preset is null)
            {
                return;
            }

            var r = await SingleLineViewModel.String(SubSystemString.EditNameTitle, preset.Name);

            if (!r.IsFinished)
            {
                return;
            }

            preset.Name = r.Value;
            Save();
        }
        
        private async Task RemoveManifestImpl(ModulePreset preset)
        {
            if (preset is null)
            {
                return;
            }

            if (!await  this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }
            
            Manifests.Remove(preset);
            
            if (Property.DefaultModulePreset
                        .Values
                        .Any(x => ReferenceEquals(x, preset)))
            {
                await this.WarningNotification(Language.GetText("text.defaultManifestHasBeenRemoved"));
            }
            
            Save();
        }

        private async Task AddTemplateImpl()
        {
            var ds       = DialogService();
            var template = _selectedPreset.Templates;
            var hash = template .Select(x => x.Id)
                                        .ToHashSet();
            var r = await ds.Dialog<IEnumerable<ModuleTemplateCache>, ModuleSelectorViewModel>(new Parameter
            {
                Args = new object[]
                {
                    Studio.DatabaseManager()
                        .GetEngine<TemplateEngine>()
                        .TemplateCacheDB
                        .FindAll()
                        .Where(x => !hash.Contains(x.Id))
                        .ToArray()
                }
            });
            
            if (!r.IsFinished)
            {
                return;
            }

            template.AddMany(r.Value);
            Save();
        }
        
        private void RemoveTemplateImpl(ModuleTemplateCache template)
        {
            if (template is null)
            {
                return;
            }

            if (_selectedPreset is null)
            {
                return;
            }
            
            SelectedPreset.Templates.Remove(template);
            Save();
        }

        public PresetProperty Property { get; }

        public ModulePreset SelectedPreset
        {
            get => _selectedPreset;
            set
            {
                SetValue(ref _selectedPreset, value);
                RemoveManifestCommand.NotifyCanExecuteChanged();
                EditManifestCommand.NotifyCanExecuteChanged();
            }
        }

        public ModulePreset Skill
        {
            get => Property.GetModulePreset(DocumentType.Skill);
            set
            {
                Property.SetModulePreset(DocumentType.Skill, value);
                RaiseUpdated();
                Save();
            }
        }
        public ModulePreset Character
        {
            get => Property.GetModulePreset(DocumentType.Character);
            set
            {
                Property.SetModulePreset(DocumentType.Character, value);
                RaiseUpdated();
                Save();
            }
        }
        public ModulePreset Geography
        {
            get => Property.GetModulePreset(DocumentType.Geography);
            set
            {
                Property.SetModulePreset(DocumentType.Geography, value);
                RaiseUpdated();
                Save();
            }
        }
        
        public ModulePreset Item
        {
            get => Property.GetModulePreset(DocumentType.Item);
            set
            {
                Property.SetModulePreset(DocumentType.Item, value);
                RaiseUpdated();
                Save();
            }
        }
        
        public ModulePreset Other
        {
            get => Property.GetModulePreset(DocumentType.Other);
            set
            {
                Property.SetModulePreset(DocumentType.Other, value);
                RaiseUpdated();
                Save();
            }
        }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ModulePreset> Manifests => Property.ModulePresets;
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddManifestCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ModulePreset> EditManifestCommand { get; }
        
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddTemplateCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ModulePreset> RemoveManifestCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ModuleTemplateCache> RemoveTemplateCommand { get; }

    }
}