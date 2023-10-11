namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    /// <summary>
    /// 表示数字内容块。
    /// </summary>
    public class NumberBlockDataUI : ModuleBlockDataUI<NumberBlock, int>, INumberBlockDataUI
    {
        public NumberBlockDataUI(NumberBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public NumberBlockDataUI(NumberBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            Maximum = block.Maximum;
            Minimum = block.Minimum;
            Suffix  = block.Suffix;
            Value   = block.Value;
        }

        protected override int OnValueChanged(int oldValue, int newValue)
        {
            newValue          = newValue == -1 ? Fallback : Math.Clamp(newValue, Minimum, Maximum);
            TargetBlock.Value = newValue;
            return newValue;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; }
    }

    /// <summary>
    /// 表示数字内容块。
    /// </summary>
    public class NumberBlockEditUI : ModuleBlockEditUI<NumberBlock, int>, INumberBlockEditUI
    {
        private string _suffix;
        private int    _maximum;
        private int    _minimum;

        public NumberBlockEditUI(NumberBlock block) : base(block)
        {
            Suffix  = block.Suffix;
            Maximum = block.Maximum;
            Minimum = block.Minimum;
        }

        protected override NumberBlock CreateInstanceOverride()
        {
            return new NumberBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                Fallback = Fallback,
                Suffix   = Suffix,
                ToolTips = ToolTips,
                Maximum  = Maximum,
                Minimum  = Minimum,
            };
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum
        {
            get => _minimum;
            set => SetValue(ref _minimum, value);
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set => SetValue(ref _maximum, value);
        }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix
        {
            get => _suffix;
            set => SetValue(ref _suffix, value);
        }
    }
}