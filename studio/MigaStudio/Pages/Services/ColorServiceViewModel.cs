using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    partial class UniverseViewModel
    {
        private ColorMapping _selected;
        private string       _color;
        private string       _selectedKeyword;
        private bool       _isEmpty;


        private async Task AddMappingImpl()
        {
            var r = await SingleLineViewModel.String(SubSystemString.EditNameTitle);
            if (!r.IsFinished)
            {
                return;
            }

            var m = new ColorMapping
            {
                Id       = ID.Get(),
                Name     = r.Value,
                Color    = "#007ACC",
                Keywords = new ObservableCollection<string> { r.Value },
            };

            Mappings.Add(m);
            RarityProperty.Mappings
                    .Add(m);
            SetDirtyState();
        }

        private async Task RemoveMappingImpl(ColorMapping mapping)
        {
            if (mapping is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Mappings.Remove(mapping);
            RarityProperty.Mappings
                    .Remove(mapping);
            SetDirtyState();
        }

        private async Task EditMappingImpl(ColorMapping mapping)
        {
            if (mapping is null)
            {
                return;
            }

            var r = await SingleLineViewModel.String(SubSystemString.EditNameTitle, mapping.Name);
            if (!r.IsFinished)
            {
                return;
            }

            mapping.Name = r.Value;
            SetDirtyState();
        }

        private async Task AddKeywordImpl(ColorMapping mapping)
        {
            if (mapping is null)
            {
                return;
            }

            var r = await SingleLineViewModel.String(SubSystemString.EditNameTitle);
            if (!r.IsFinished)
            {
                return;
            }

            var k = r.Value;

            if (string.IsNullOrEmpty(k) ||
                mapping.Keywords.Contains(k))
            {
                return;
            }

            mapping.Keywords.Add(k);
            Keywords.Add(k);
            SetDirtyState();
        }
        
        private async Task EditKeywordImpl(string keyword)
        {
            if (Selected is null || string.IsNullOrEmpty(keyword))
            {
                return;
            }

            var r = await SingleLineViewModel.String(SubSystemString.EditNameTitle, keyword);
            if (!r.IsFinished)
            {
                return;
            }

            var k = r.Value;

            if (string.IsNullOrEmpty(k) ||
                Selected.Keywords.Contains(k))
            {
                return;
            }

            Selected.Keywords.Remove(keyword);
            Selected.Keywords.Add(k);
            Keywords.Remove(keyword);
            Keywords.Add(k);
            SetDirtyState();
        }
        
        private async Task RemoveKeywordImpl(string keyword)
        {
            if (Selected is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(keyword))
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }
            
            
            Selected.Keywords.Remove(keyword);
            Keywords.Remove(keyword);
            SetDirtyState();
        }

        public ColorService ColorService { get; }
        public ColorServiceProperty RarityProperty { get; }

        /// <summary>
        /// 获取或设置 <see cref="SelectedKeyword"/> 属性。
        /// </summary>
        public string SelectedKeyword
        {
            get => _selectedKeyword;
            set { 
                SetValue(ref _selectedKeyword, value);

                AddKeywordCommand.NotifyCanExecuteChanged();
                EditKeywordCommand.NotifyCanExecuteChanged();
                RemoveKeywordCommand.NotifyCanExecuteChanged();
            }
        }
        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public string Color
        {
            get => _color;
            set
            {
                SetValue(ref _color, value);

                if (Selected is not null)
                {
                    SetDirtyState();
                    Selected.Color = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public ColorMapping Selected
        {
            get => _selected;
            set
            {
                SetValue(ref _selected, value);
                IsEmpty = Selected is null;
                AddKeywordCommand.NotifyCanExecuteChanged();
                EditKeywordCommand.NotifyCanExecuteChanged();
                RemoveKeywordCommand.NotifyCanExecuteChanged();
                EditMappingCommand.NotifyCanExecuteChanged();
                RemoveMappingCommand.NotifyCanExecuteChanged();
                Keywords.Clear();
                if (value is not null)
                {
                    _color = value.Color;
                    RaiseUpdated(nameof(Color));
                    Keywords.AddMany(value.Keywords);
                }
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="IsEmpty"/> 属性。
        /// </summary>
        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetValue(ref _isEmpty, value);
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<string> Keywords { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ColorMapping> Mappings { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand SaveCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddMappingCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ColorMapping> EditMappingCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ColorMapping> RemoveMappingCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ColorMapping> AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> EditKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> RemoveKeywordCommand { get; }
    }
}