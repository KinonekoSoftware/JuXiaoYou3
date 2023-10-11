namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public class SwitchBlockDataUI : ModuleBlockDataUI<OptionBlock, bool>, IOptionBlockDataUI
    {
        public SwitchBlockDataUI(OptionBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public SwitchBlockDataUI(OptionBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            Value = block.Value;
        }

        protected override bool OnValueChanged(bool oldValue, bool newValue)
        {
            TargetBlock.Value = newValue;
            return newValue;
        }
    }
    
    public class StarBlockDataUI : ModuleBlockDataUI<StarBlock, bool>, IOptionBlockDataUI
    {
        
        public StarBlockDataUI(StarBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        public StarBlockDataUI(StarBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            Value = block.Value;
        }
        protected override bool OnValueChanged(bool oldValue, bool newValue)
        {
            TargetBlock.Value = newValue;
            return newValue;
        }
    }
    
    public class HeartBlockDataUI : ModuleBlockDataUI<HeartBlock, bool>, IOptionBlockDataUI
    {
        public HeartBlockDataUI(HeartBlock block) : this(block, ModuleBlockFactory.EmptyHandler)
        {
        }
        
        public HeartBlockDataUI(HeartBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler) : base(block, handler)
        {
            Value = block.Value;
        }
        protected override bool OnValueChanged(bool oldValue, bool newValue)
        {
            TargetBlock.Value = newValue;
            return newValue;
        }
    }

    public class HeartBlockEditUI : ModuleBlockEditUI<HeartBlock, bool>, IOptionBlockEditUI
    {
        public HeartBlockEditUI(HeartBlock block) : base(block)
        {
        }

        protected override HeartBlock CreateInstanceOverride()
        {
            return new HeartBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Fallback = Fallback
            };
        }
    }
    
    public class SwitchBlockEditUI : ModuleBlockEditUI<SwitchBlock, bool>, IOptionBlockEditUI
    {
        public SwitchBlockEditUI(SwitchBlock block) : base(block)
        {
        }

        protected override SwitchBlock CreateInstanceOverride()
        {
            return new SwitchBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Fallback = Fallback
            };
        }
    }
    
    public class StarBlockEditUI : ModuleBlockEditUI<StarBlock, bool>, IOptionBlockEditUI
    {
        public StarBlockEditUI(StarBlock block) : base(block)
        {
        }

        protected override StarBlock CreateInstanceOverride()
        {
            return new StarBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Fallback = Fallback
            };
        }
    }
}