namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public class SingleLineBlockDataUI : ModuleBlockDataUI<SingleLineBlock, string>, ISingleLineBlockDataUI
    {
        public SingleLineBlockDataUI(SingleLineBlock block) : this(block, ModuleBlockFactory.EmptyHandler) 
        {
        }
        
        public SingleLineBlockDataUI(SingleLineBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            Suffix = block.Suffix;
            Value  = block.Value;
        }

        protected override string OnValueChanged(string oldValue, string newValue)
        {
            newValue          = string.IsNullOrEmpty(newValue) ? Fallback : newValue;
            TargetBlock.Value = newValue;
            return newValue;
        }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; }
    }
    
    public class SingleLineBlockEditUI : ModuleBlockEditUI<SingleLineBlock, string>, ISingleLineBlockEditUI
    {
        private string _suffix;
        
        public SingleLineBlockEditUI(SingleLineBlock block) : base(block)
        {
            Suffix = block.Suffix;
        }

        protected override SingleLineBlock CreateInstanceOverride()
        {
            return new SingleLineBlock
            {
                Id         = Id,
                Name       = Name,
                Metadata   = Metadata,
                Fallback   = Fallback,
                ToolTips   = ToolTips,
                Suffix = Suffix
            };
        }

        /// <summary>
        /// 获取或设置 <see cref="Suffix"/> 属性。
        /// </summary>
        public string Suffix
        {
            get => _suffix;
            set => SetValue(ref _suffix, value);
        }
    }
}