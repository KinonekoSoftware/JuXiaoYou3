namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public class DegreeBlockDataUI : ModuleBlockDataUI<DegreeBlock, int>, IDegreeBlockDataUI
    {
        public DegreeBlockDataUI(DegreeBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public DegreeBlockDataUI(DegreeBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            Maximum    = block.Maximum;
            Minimum    = block.Minimum;
            DivideLine = block.DivideLine;
            Positive   = block.Positive;
            Negative   = block.Negative;
            Value = block.Value;
        }

        protected override int OnValueChanged(int oldValue, int newValue)
        {
            var value = newValue == -1 ?
                Fallback : 
                Math.Clamp(newValue, Minimum, Maximum);
            
            TargetBlock.Value = value;
            DisplayValue      = value >= DivideLine ? Positive : Negative;
            RaiseUpdated(nameof(DisplayValue));
            return value;
        }

        public int DivideLine { get; }
        public string Negative { get; }
        public string Positive { get; }
        
        /// <summary>
        /// 用于绑定的值
        /// </summary>
        public string DisplayValue { get; private set; }
        public int Maximum { get; }
        public int Minimum { get; }
    }

    public class DegreeBlockEditUI : ModuleBlockEditUI<DegreeBlock, int>, IDegreeBlockEditUI
    {
        public DegreeBlockEditUI(DegreeBlock block) : base(block)
        {
            Maximum    = block.Maximum;
            Minimum    = block.Minimum;
            DivideLine = block.DivideLine;
            Positive   = block.Positive;
            Negative   = block.Negative;
        }

        private int    _divideLine;
        private int    _maximum;
        private int    _minimum;
        private string _negative;
        private string _positive;

        protected override DegreeBlock CreateInstanceOverride()
        {
            return new DegreeBlock
            {
                Id         = Id,
                Name       = Name,
                Metadata   = Metadata,
                Fallback   = Fallback,
                Positive   = Positive,
                Negative   = Negative,
                DivideLine = DivideLine,
                ToolTips   = ToolTips,
                Maximum    = Maximum,
                Minimum    = Minimum,
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
        /// 分割线
        /// </summary>
        public int DivideLine
        {
            get => _divideLine;
            set => SetValue(ref _divideLine, value);
        }

        /// <summary>
        /// 正面值
        /// </summary>
        public string Positive
        {
            get => _positive;
            set => SetValue(ref _positive, value);
        }

        /// <summary>
        /// 负面值
        /// </summary>
        public string Negative
        {
            get => _negative;
            set => SetValue(ref _negative, value);
        }
    }
}