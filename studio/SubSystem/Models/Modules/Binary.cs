
namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public class BinaryBlockDataUI : ModuleBlockDataUI<BinaryBlock, bool>, IBinaryBlockDataUI
    {
        public BinaryBlockDataUI(BinaryBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public BinaryBlockDataUI(BinaryBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            Negative     = block.Negative;
            Positive     = block.Positive;
            Value        = block.Value;
        }
        
        protected override bool OnValueChanged(bool oldValue, bool newValue)
        {
            TargetBlock.Value = newValue;
            DisplayValue      = newValue ? Positive : Negative;
            RaiseUpdated(nameof(DisplayValue));
            return newValue;
        }
        
        /// <summary>
        /// 用于绑定的值
        /// </summary>
        public string DisplayValue { get; private set; }
        public string Negative { get; }
        public string Positive { get; }
    }

    public class BinaryBlockEditUI : ModuleBlockEditUI<BinaryBlock, bool>, IBinaryBlockEditUI
    {
        public BinaryBlockEditUI(BinaryBlock block) : base(block)
        {
            Positive   = block.Positive;
            Negative   = block.Negative;
        }
        private string _negative;
        private string _positive;

        protected override BinaryBlock CreateInstanceOverride()
        {
            return new BinaryBlock
            {
                Id         = Id,
                Name       = Name,
                Metadata   = Metadata,
                Fallback   = Fallback,
                Positive   = Positive,
                Negative   = Negative,
                ToolTips   = ToolTips,
            };
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