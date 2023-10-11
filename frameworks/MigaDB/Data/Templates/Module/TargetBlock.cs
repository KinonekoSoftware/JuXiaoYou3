namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示目标内容块。
    /// </summary>
    public interface ITargetBlock : IModuleBlock
    {
        /// <summary>
        /// 数据的名字
        /// </summary>
        string TargetName { get; }

        /// <summary>
        /// 数据源
        /// </summary>
        string TargetSource { get; }

        /// <summary>
        /// 数据的缩略图
        /// </summary>
        string TargetThumbnail { get; }
    }

    /// <summary>
    /// 表示引用数据源。
    /// </summary>
    public enum ReferenceSource
    {
        // TODO: 与概念挂钩

        /// <summary>
        /// 设定
        /// </summary>
        Document,
        
        /// <summary>
        /// 剧情
        /// </summary>
        Compose,
        
        
    }
    
    /// <summary>
    /// 表示引用内容块。
    /// </summary>
    public interface IReferenceBlock : ITargetBlock
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ITargetBlockDataUI : ITargetBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ITargetBlockEditUI : IModuleBlockEditUI
    {
    }

    public abstract class TargetBlock : ModuleBlock, ITargetBlock
    {
        protected override bool CompareTemplateOverride(ModuleBlock block) => true;

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var tb = (TargetBlock)block;
            return TargetName == tb.TargetName &&
                   TargetSource == tb.TargetSource &&
                   TargetThumbnail == tb.TargetThumbnail;
        }
        

        public override bool CopyTo(ModuleBlock newBlock)
        {
            if (newBlock is TargetBlock nb && CompareTemplateOverride(nb))
            {
                nb.TargetName   = TargetName;
                nb.TargetSource = TargetSource;
                nb.TargetThumbnail   = TargetThumbnail;
                return true;
            }

            return false;
        }

        public override void ClearValue()
        {
            TargetName      = string.Empty;
            TargetSource    = string.Empty;
            TargetThumbnail = string.Empty;
        }

        /// <summary>
        /// 数据的名字
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public string TargetSource { get; set; }

        /// <summary>
        /// 数据的缩略图
        /// </summary>
        public string TargetThumbnail { get; set; }
    }

    public sealed class AudioBlock : TargetBlock
    {
        
        public override string GetLanguageId() => IdOfAudio;
        
        public override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name       = Metadata,
                Value      = TargetName,
                Type       = MetadataKind.Audio,
                Parameters = TargetSource
            };
        }
        public override MetadataKind? ExtractType => MetadataKind.Audio;
    }
    
    public sealed class VideoBlock : TargetBlock
    {
        public override string GetLanguageId() => IdOfVideo;
        
        public override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name       = Metadata,
                Value      = TargetName,
                Type       = MetadataKind.Video,
                Parameters = TargetSource
            };
        }
        public override MetadataKind? ExtractType => MetadataKind.Video;
    }
    
    public sealed class MusicBlock : TargetBlock
    {
        
        public override string GetLanguageId() => IdOfMusic;
        public override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name       = Metadata,
                Value      = TargetName,
                Type       = MetadataKind.Music,
                Parameters = TargetSource
            };
        }
        public override MetadataKind? ExtractType => MetadataKind.Music;
    }
    
    public sealed class ImageBlock : TargetBlock
    {
        public override string GetLanguageId() => IdOfImage;
        
        public override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name       = Metadata,
                Value      = TargetName,
                Type       = MetadataKind.Image,
                Parameters = TargetSource
            };
        }
        public override MetadataKind? ExtractType => MetadataKind.Image;
    }
    
    public sealed class FileBlock : TargetBlock
    {
        public override string GetLanguageId() => IdOfFile;
        
        public override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name       = Metadata,
                Value      = TargetName,
                Type       = MetadataKind.File,
                Parameters = TargetSource
            };
        }
        public override MetadataKind? ExtractType => MetadataKind.File;
    }
    
    public sealed class ReferenceBlock : TargetBlock, IReferenceBlock
    {
        public override string GetLanguageId() => IdOfReference;
        
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var rb = (ReferenceBlock)block;
            return DataSource == rb.DataSource;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var tb = (TargetBlock)block;
            return TargetName == tb.TargetName &&
                   TargetSource == tb.TargetSource &&
                   TargetThumbnail == tb.TargetThumbnail;
        }
        /// <summary>
        /// 数据来源
        /// </summary>
        public ReferenceSource DataSource { get; init; }
        public override MetadataKind? ExtractType => MetadataKind.Reference;
        
        
        public override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name       = Metadata,
                Value      = TargetName,
                Type       = MetadataKind.Reference,
                Parameters = TargetSource
            };
        }
    }
}