namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public interface IGroupBlock : IModuleBlock
    {
        List<ModuleBlock> Items { get; }
    }

    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public interface IGroupBlockEditUI : IModuleBlockEditUI
    {
    }

    public class GroupBlock : ModuleBlock, IGroupBlock
    {
        public override string GetLanguageId() => IdOfGroup;
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            if (block is not GroupBlock gb)
            {
                return false;
            }

            if (gb.Items.Count != Items.Count)
            {
                return false;
            }

            for (var i = 0; i < Items.Count; i++)
            {
                var t = gb.Items[i];
                var s = Items[i];

                if (!s.CompareTemplate(t))
                {
                    return false;
                }
            }

            return true;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            if (block is not GroupBlock gb)
            {
                return false;
            }

            if (gb.Items.Count != Items.Count)
            {
                return false;
            }

            for (var i = 0; i < Items.Count; i++)
            {
                var t = gb.Items[i];
                var s = Items[i];

                if (!s.CompareValue(t))
                {
                    return false;
                }
            }

            return true;
        }

        public override void ClearValue()
        {
            foreach (var item in Items)
            {
                item.ClearValue();
            }
        }
        

        public override bool CopyTo(ModuleBlock newBlock)
        {
            return false;
        }
        
        public sealed override Metadata ExtractMetadata()
        {
            return null;
        }

        public List<ModuleBlock> Items { get; init; }
        public sealed override MetadataKind? ExtractType => null;
    }
}