using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public abstract class MonoSelectorDataUI : ModuleBlockDataUI<MonoSelectorBlock, string>, IMonoSelectorBlockDataUI
    {
        protected MonoSelectorDataUI(MonoSelectorBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            Items = new List<OptionItemUI>();

            if (block.Items is not null && block.Items.Count > 0)
            {
                Items.AddMany(block.Items
                                    .Select(x => new OptionItemUI(
                                        GetId(),
                                        Fallback,
                                        x,
                                        OnSelected)));

                
            }
            if (Items.Count > 0)
            {
                string v;
                
                if (Items.All(x => x.Value != block.Value))
                {
                    v           = block.Fallback;
                    block.Value = v;
                    Value       = v;
                }
                
                if(Items.All(x => x.Value != block.Fallback))
                {
                    //
                    // 如果选项的值不是集合中的值，则随机选一个
                    v = Items.First()
                             .Value;
                    block.Value = v;
                    Value       = v;
                    return;
                }
                
                v           = string.IsNullOrEmpty(block.Value) ? block.Fallback : block.Value;
                block.Value = v;
                Value       = v;
            }
            else
            {
                Value = string.IsNullOrEmpty(block.Value) ? block.Fallback : block.Value;
            }
        }

        private string GetId() => string.IsNullOrEmpty(Id) ? GetHashCode().ToString() : Id;

        private void OnSelected(OptionItemUI item)
        {
            Value = item.Value;
        }

        protected override string OnValueChanged(string oldValue, string newValue)
        {
            if (Items is null)
            {
                return newValue;
            }

            TargetBlock.Value = newValue;
            return newValue;
        }

        public List<OptionItemUI> Items { get; }
    }

    public abstract class MonoSelectorEditUI : ModuleBlockEditUI<MonoSelectorBlock, string>, IMonoSelectorBlockEditUI
    {
        private bool _allowDiffValue;

        protected MonoSelectorEditUI(MonoSelectorBlock block) : base(block)
        {
            Items = new ObservableCollection<OptionItem>();

            if (block.Items is not null && block.Items.Count > 0)
            {
                Items.AddMany(block.Items);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="AllowDiffValue"/> 属性。
        /// </summary>
        public bool AllowDiffValue
        {
            get => _allowDiffValue;
            set => SetValue(ref _allowDiffValue, value);
        }

        public ObservableCollection<OptionItem> Items { get; }
    }


    public sealed class SequenceBlockDataUI : MonoSelectorDataUI
    {
        public SequenceBlockDataUI(SequenceBlock block) : base(block, ModuleBlockFactory.EmptyHandler)
        {
        }

        public SequenceBlockDataUI(SequenceBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
        }
    }

    public sealed class SequenceBlockEditUI : MonoSelectorEditUI
    {
        public SequenceBlockEditUI(SequenceBlock block) : base(block)
        {
            AddCommand    = new AsyncRelayCommand(AddImpl);
            RemoveCommand = new AsyncRelayCommand<OptionItem>(RemoveImpl);
            EditCommand   = new AsyncRelayCommand<OptionItem>(EditImpl);
            UpCommand     = new RelayCommand<OptionItem>(UpImpl);
            DownCommand   = new RelayCommand<OptionItem>(DownImpl);
        }

        protected override MonoSelectorBlock CreateInstanceOverride()
        {
            return new SequenceBlock
            {
                Id       = Id,
                Name     = Name,
                Items    = new List<OptionItem>(Items),
                Metadata = Metadata,
                Fallback = Fallback,
                ToolTips = ToolTips,
            };
        }

        private async Task AddImpl()
        {
            var r = await SingleLineViewModel.String(Language.GetText("text.AddItem"));

            if (!r.IsFinished)
            {
                return;
            }

            var    name = r.Value;
            string value;

            if (AllowDiffValue)
            {
                r = await SingleLineViewModel.String(Language.GetText("text.AddValue"));

                if (!r.IsFinished)
                {
                    return;
                }

                value = r.Value;
            }
            else
            {
                value = name;
            }

            if (Items.Any(x => x.Value == value))
            {
                await Xaml.Get<IDialogService>()
                          .Notify(
                              CriticalLevel.Warning,
                              SubSystemString.Notify,
                              Language.ItemDuplicatedText);
                return;
            }

            Items.Add(new OptionItem
            {
                Name  = name,
                Value = value
            });
        }

        private async Task EditImpl(OptionItem item)
        {
            if (item is null)
            {
                return;
            }

            var r = await SingleLineViewModel.String(Language.GetText("text.EditName"), item.Name);

            if (r.IsFinished)
            {
                item.Value = r.Value;
                item.Name = r.Value;
            }
        }

        private async Task RemoveImpl(OptionItem item)
        {
            if (item is null)
            {
                return;
            }

            var r = await Xaml.Get<IDialogService>()
                              .Danger(SubSystemString.Notify,
                                  SubSystemString.AreYouSureCreateNew);

            if (!r)
            {
                return;
            }

            if (Items.Remove(item))
            {
                await Xaml.Get<IDialogService>()
                          .Notify(
                              CriticalLevel.Success,
                              SubSystemString.Notify,
                              SubSystemString.OperationOfRemoveIsSuccessful);
            }
        }

        private void UpImpl(OptionItem item)
        {
            if (item is null)
            {
                return;
            }

            var index = Items.IndexOf(item);

            if (index < 1)
            {
                return;
            }

            Items.Move(index, index - 1);
        }

        private void DownImpl(OptionItem item)
        {
            if (item is null)
            {
                return;
            }

            var index = Items.IndexOf(item);

            if (index >= Items.Count - 1)
            {
                return;
            }

            Items.Move(index, index + 1);
        }

        public AsyncRelayCommand AddCommand { get; }
        public AsyncRelayCommand<OptionItem> EditCommand { get; }
        public AsyncRelayCommand<OptionItem> RemoveCommand { get; }
        public RelayCommand<OptionItem> UpCommand { get; }
        public RelayCommand<OptionItem> DownCommand { get; }
    }
}