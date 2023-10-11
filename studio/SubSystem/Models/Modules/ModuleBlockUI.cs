namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlockDataUI : ObservableObject, IModuleBlockDataUI
    {
        protected readonly Action<ModuleBlockDataUI, ModuleBlock> Handler;
        
        protected ModuleBlockDataUI(ModuleBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler)
        {
            Handler = handler;
            Id       = block.Id;
            Metadata = block.Metadata;
            Name     = block.Name;
            ToolTips = block.ToolTips;
        }

        public abstract bool CompareTemplate(ModuleBlock block);
        public abstract bool CompareValue(ModuleBlock block);

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 喵喵咒语
        /// </summary>
        public string Metadata { get; }

        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        public string ToolTips { get; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; }
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlockDataUI<TBlock, TValue> : ModuleBlockDataUI where TBlock : ModuleBlock, IModuleBlock<TValue>
    {
        private          TValue                                 _value;

        protected ModuleBlockDataUI(TBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            
            TargetBlock = block;
            Fallback    = block.Fallback;
        }

        public override bool CompareTemplate(ModuleBlock block)
        {
            return TargetBlock.CompareTemplate(block);
        }

        public override bool CompareValue(ModuleBlock block)
        {
            return TargetBlock.CompareValue(block);
        }

        protected void SetCoerceValue(TValue value)
        {
            _value = value;
        }

        private void OnValueChangedIntern(TValue oldValue, TValue newValue)
        {
            _value = OnValueChanged(oldValue, newValue);
            Handler?.Invoke(this, TargetBlock);
        }
        
        protected abstract TValue OnValueChanged(TValue oldValue, TValue newValue);
        
        protected TBlock TargetBlock { get; }

        /// <summary>
        /// 默认值
        /// </summary>
        public TValue Fallback { get; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public TValue Value
        {
            get => _value;
            set
            {
                var old = _value;
                
                if (SetValue(ref _value, value))
                {
                    OnValueChangedIntern(old, value);
                }
            }
        }
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlockEditUI : ObservableObject, IModuleBlockEditUI
    {
        private string _name;
        private string _toolTips;
        private string _metadata;
        
        protected ModuleBlockEditUI(IModuleBlock block)
        {
            Id       = string.IsNullOrEmpty(block.Id) ? ID.Get() : block.Id; 
            Name     = block.Name;
            ToolTips = block.ToolTips;
            Metadata = block.Metadata;
        }
        
        public abstract ModuleBlock CreateInstance();

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 喵喵咒语
        /// </summary>
        public string Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }

        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        public string ToolTips
        {
            get => _toolTips;
            set => SetValue(ref _toolTips, value);
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }


    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlockEditUI<TBlock, TValue> : ModuleBlockEditUI, IModuleBlockEditUI<TValue> 
        where TBlock : ModuleBlock, IModuleBlock, IModuleBlock<TValue>
    {
        private TValue _fallback;

        protected ModuleBlockEditUI(TBlock block) : base(block)
        {
            Fallback    = block.Fallback;
            TargetBlock = block;
        }

        public sealed override ModuleBlock CreateInstance()
        {
            return CreateInstanceOverride();
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns></returns>
        protected abstract TBlock CreateInstanceOverride();
        
        /// <summary>
        /// 目标内容块
        /// </summary>
        protected TBlock TargetBlock { get; }
        
        /// <summary>
        /// 默认值
        /// </summary>
        public TValue Fallback
        {
            get => _fallback;
            set => SetValue(ref _fallback, value);
        }
    }
}