using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public abstract class ChartBlockDataUI : ModuleBlockDataUI, IChartBlockDataUI
    {
        private readonly List<BindableAxis> _value;
        private List<int> _data;
        
        protected ChartBlockDataUI(ChartBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            TargetBlock   = block;
            Fallback      = block.Fallback;
            
            _value        = new List<BindableAxis>();
            Axis          = new List<string>(block.Axis);
            Color         = block.Color;
            Maximum       = block.Maximum;
            Minimum       = block.Minimum;

            if (block.Value is null)
            {
                block.Value = new int[block.Fallback.Length];
                Array.Copy(block.Fallback, block.Value, block.Value.Length);
            }

            for (var i = 0; i < block.Value.Length; i++)
            {
                Value.Add(new BindableAxis(OnValueChanged, Axis[i], i, block.Value[i], Maximum));
            }
        }

        public override bool CompareTemplate(ModuleBlock block)
        {
            return TargetBlock.CompareTemplate(block);
        }

        public override bool CompareValue(ModuleBlock block)
        {
            return TargetBlock.CompareValue(block);
        }

        protected void OnValueChanged(int index, int value)
        {
            TargetBlock.Value[index] = value;
            Data = new List<int>(TargetBlock.Value);
            Handler?.Invoke(this, TargetBlock);
        }
        

        protected ChartBlock TargetBlock { get; }

        /// <summary>
        /// 默认值
        /// </summary>
        public int[] Fallback { get; }

        /// <summary>
        /// 当前值
        /// </summary>
        public List<BindableAxis> Value
        {
            get => _value;
            set { }
        }

        /// <summary>
        /// 获取或设置 <see cref="Data"/> 属性。
        /// </summary>
        public List<int> Data
        {
            get => _data;
            set => SetValue(ref _data, value);
        }


        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; }

        public List<string> Axis { get; }

        public string Color { get; }
    }

    public class BindableAxis : ObservableObject
    {
        public BindableAxis(Action<int, int> callback, string name, int index, int value, int max)
        {
            Callback = callback;
            Index = index;
            Name = name;
            Maximum = max;
            Value = value;
        }

        private string _name;
        private int _value;

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public int Value
        {
            get => _value;
            set
            {
                SetValue(ref _value, value);
                Callback(Index, value);
            }
        }

        private int _maximum;

        /// <summary>
        /// 获取或设置 <see cref="Maximum"/> 属性。
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set => SetValue(ref _maximum, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public Action<int, int> Callback { get; }

        public int Index { get; }
    }

    public abstract class ChartBlockEditUI : ModuleBlockEditUI<ChartBlock, int[]>, IChartBlockEditUI
    {
        private int    _maximum;
        private int    _minimum;
        private string _color;
        private int _fallbackValue;

        protected ChartBlockEditUI(ChartBlock block) : base(block)
        {
            Axis          = new ObservableCollection<string>();
            Maximum       = block.Maximum;
            Minimum       = block.Minimum;
            Color         = block.Color;
            AddCommand    = new AsyncRelayCommand(AddImpl);
            RemoveCommand = new AsyncRelayCommand<string>(RemoveImpl);
            EditCommand   = new AsyncRelayCommand<string>(EditImpl);
            UpCommand     = new RelayCommand<string>(UpImpl);
            DownCommand   = new RelayCommand<string>(DownImpl);

            if (block.Axis is not null)
            {
                Axis.AddMany(block.Axis);
            }
        }
        
        
        private async Task AddImpl()
        {
            var r  = await SingleLineViewModel.String(Language.GetText("text.Axis"));
            
            if (!r.IsFinished)
            {
                return;
            }
            
            Axis.Add(r.Value);
            
        }

        private async Task EditImpl(string item)
        {
            if (string.IsNullOrEmpty(item))
            {
                return;
            }

            var r = await SingleLineViewModel.String(Language.GetText("text.Edit"), item);

            if (r.IsFinished && !string.IsNullOrEmpty(r.Value))
            {
                var index = Axis.IndexOf(item);
                Axis[index] = r.Value;
            }
        }

        private async Task RemoveImpl(string item)
        {
            if (item is null)
            {
                return;
            }

            var r = await Xaml.Get<IDialogService>()
                              .Danger(
                                  SubSystemString.Notify,
                                  SubSystemString.AreYouSureCreateNew);

            if (!r)
            {
                return;
            }

            Axis.Remove(item);
        }

        private void UpImpl(string item)
        {
            if (item is null)
            {
                return;
            }

            var index = Axis.IndexOf(item);

            if (index < 1)
            {
                return;
            }

            Axis.Move(index, index - 1);
        }

        private void DownImpl(string item)
        {
            if (item is null)
            {
                return;
            }

            var index = Axis.IndexOf(item);

            if (index >= Axis.Count - 1)
            {
                return;
            }

            Axis.Move(index, index + 1);
        }

        public AsyncRelayCommand AddCommand { get; }
        public AsyncRelayCommand<string> EditCommand { get; }
        public AsyncRelayCommand<string> RemoveCommand { get; }
        public RelayCommand<string> UpCommand { get; }
        public RelayCommand<string> DownCommand { get; }

        /// <summary>
        /// 获取或设置 <see cref="FallbackValue"/> 属性。
        /// </summary>
        public int FallbackValue
        {
            get => _fallbackValue;
            set
            {
                SetValue(ref _fallbackValue, Math.Clamp(value, Minimum, Maximum));
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public string Color
        {
            get => _color;
            set => SetValue(ref _color, value);
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum
        {
            get => _minimum;
            set
            {
                if (value >= _maximum)
                {
                    return;
                }
                SetValue(ref _minimum, value);
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set
            {
                if (_minimum >= value)
                {
                    return;
                }
                SetValue(ref _maximum, value);
            }
        }

        public ObservableCollection<string> Axis { get; }
    }

    public class RadarBlockDataUI : ChartBlockDataUI
    {
        public RadarBlockDataUI(RadarBlock block) : base(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public RadarBlockDataUI(RadarBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
        }
    }

    public class RadarBlockEditUI : ChartBlockEditUI
    {
        public RadarBlockEditUI(RadarBlock block) : base(block)
        {
        }

        protected override ChartBlock CreateInstanceOverride()
        {
            var f = new int[Axis.Count];
            for (var i = 0; i < Axis.Count; i++)
            {
                f[i] = FallbackValue;
            }
            
            return new RadarBlock
            {
                Id = Id,
                Name = Name,
                Metadata = Metadata,
                Fallback = f,
                ToolTips = ToolTips,
                Maximum = Maximum,
                Minimum = Minimum,
                Axis = Axis.ToArray(),
                Color = Color,
            };
        }
    }
    
    

    public class HistogramBlockDataUI : ChartBlockDataUI
    {
        
        public HistogramBlockDataUI(HistogramBlock block) : base(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public HistogramBlockDataUI(HistogramBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
        }
    }

    public class HistogramBlockEditUI : ChartBlockEditUI
    {
        public HistogramBlockEditUI(HistogramBlock block) : base(block)
        {
        }

        protected override ChartBlock CreateInstanceOverride()
        {
            var f = new int[Axis.Count];
            for (var i = 0; i < Axis.Count; i++)
            {
                f[i] = FallbackValue;
            }

            return new HistogramBlock
            {
                Id = Id,
                Name = Name,
                Metadata = Metadata,
                Fallback = f,
                ToolTips = ToolTips,
                Maximum = Maximum,
                Minimum = Minimum,
                Axis = Axis.ToArray(),
                Color = Color,
            };
        }
    }
}