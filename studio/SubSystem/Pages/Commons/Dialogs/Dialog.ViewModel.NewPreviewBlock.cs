using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
using Acorisoft.FutureGL.MigaStudio.Models.Presentations;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    // TODO:
    // 1) 部分ModuleBlockDataUI需要最小宽度和高度
    // 2) 实现CardAction的替代
    // 3) 实现其他控件的替代
    public class NewPresentationViewModel : ImplicitDialogVM
    {
        private PresentationUI _previewItem;
        private object       _maybeMetadataKind;
        private MetadataKind _type;

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public MetadataKind Type
        {
            get => _type;
            set
            {
                SetValue(ref _type, value);
                PresentationItem = PresentationUI.GetUI(ModuleBlockFactory.GetPresentation(_type));
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="MaybeMetadataKind"/> 属性。
        /// </summary>
        public object MaybeMetadataKind
        {
            get => _maybeMetadataKind;
            set
            {
                SetValue(ref _maybeMetadataKind, value);

                if (value is MetadataKind k)
                {
                    Type = k;
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="PresentationItem"/> 属性。
        /// </summary>
        public PresentationUI PresentationItem
        {
            get => _previewItem;
            set => SetValue(ref _previewItem, value);
        }

        public static Task<Op<Presentation>> New()
        {
            return DialogService()
                .Dialog<Presentation>(new NewPresentationViewModel());
        }

        protected override bool IsCompleted() => _previewItem is not null;

        protected override void Finish()
        {
            Result = ModuleBlockFactory.GetPresentation(_type);
        }

        protected override string Failed() => "未选择";
    }
}