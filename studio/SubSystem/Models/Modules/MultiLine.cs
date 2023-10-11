namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public class MultiLineBlockDataUI : ModuleBlockDataUI<MultiLineBlock, string>, IMultiLineBlockDataUI
    {
        public MultiLineBlockDataUI(MultiLineBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public MultiLineBlockDataUI(MultiLineBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            EnableExpression = block.EnableExpression;
            CharacterLimited = block.CharacterLimited;
            Value            = block.Value;
        }

        protected override string OnValueChanged(string oldValue, string newValue)
        {
            newValue = string.IsNullOrEmpty(newValue) ? string.Empty : newValue;
            
            if (CharacterLimited > 0 && newValue.Length > 0)
            {
                TargetBlock.Value = newValue[..Math.Clamp(newValue.Length, 1, CharacterLimited)];
            }
            else
            {
                TargetBlock.Value = newValue;
            }

            return newValue;
        }

        /// <summary>
        /// 开启表达式
        /// </summary>
        public bool EnableExpression { get; }

        /// <summary>
        /// 字数限制，如果值为-1，则表示没有限制。
        /// </summary>
        public int CharacterLimited { get; }
    }

    public class MultiLineBlockEditUI : ModuleBlockEditUI<MultiLineBlock, string>, IMultiLineBlockEditUI
    {
        private int  _characterLimited;
        private bool _enableExpression;

        public MultiLineBlockEditUI(MultiLineBlock block) : base(block)
        {
            EnableExpression = block.EnableExpression;
            CharacterLimited = block.CharacterLimited == 0? 100 : block.CharacterLimited;
        }

        protected override MultiLineBlock CreateInstanceOverride()
        {
            return new MultiLineBlock
            {
                Id               = Id,
                Name             = Name,
                EnableExpression = EnableExpression,
                CharacterLimited = CharacterLimited,
                Metadata         = Metadata,
                Fallback         = Fallback,
                ToolTips         = ToolTips,
            };
        }
        
        /// <summary>
        /// 开启表达式
        /// </summary>
        public bool EnableExpression
        {
            get => _enableExpression;
            set => SetValue(ref _enableExpression, value);
        }

        /// <summary>
        /// 字数限制，如果值为-1，则表示没有限制。
        /// </summary>
        public int CharacterLimited
        {
            get => _characterLimited;
            set => SetValue(ref _characterLimited, value);
        }
    }
}