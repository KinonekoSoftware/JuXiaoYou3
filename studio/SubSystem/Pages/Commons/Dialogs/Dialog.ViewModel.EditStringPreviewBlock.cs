using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class EditStringPresentationViewModel : ImplicitDialogVM
    {
        public class FakeModuleBlock : ModuleBlock, IMetadataTextSource
        {
            protected override bool CompareTemplateOverride(ModuleBlock block)
            {
                return true;
            }

            protected override bool CompareValueOverride(ModuleBlock block)
            {
                return true;
            }

            public override string GetLanguageId()
            {
                return Name;
            }

            public override Metadata ExtractMetadata()
            {
                return new Metadata
                {
                    Name       = Name,
                    Value      = string.Empty,
                    Parameters = string.Empty,
                    Type       = MetadataKind.Text
                };
            }

            public override bool CopyTo(ModuleBlock newBlock)
            {
                return true;
            }

            public override void ClearValue()
            {
                
            }

            public override MetadataKind? ExtractType => MetadataKind.Text;

            public string GetValue() => string.Empty;
        }


        public static Task<Op<object>> Edit(StringPresentation hb, DataPartCollection dataPartCollection)
        {
            var blockCollection = new List<ModuleBlock>(64);
            var blocks = dataPartCollection.Where(x => x is PartOfModule)
                                           .OfType<PartOfModule>()
                                           .Select(x => x.Blocks)
                                           .SelectMany(x => x)
                                           .ToArray();

            var simpleBlock = blocks.Where(x => x is not GroupBlock)
                                    .Where(x => x.ExtractType == MetadataKind.Text);

            var blockInGroup = blocks.Where(x => x is GroupBlock)
                                     .OfType<GroupBlock>()
                                     .SelectMany(x => x.Items)
                                     .Where(x => x.ExtractType == MetadataKind.Text);

            var fakeBlocks = dataPartCollection.OfType<PartOfBasic>()
                                               .SelectMany(x => x.Buckets)
                                               .Select(x => new FakeModuleBlock
                                               {
                                                   Id       = x.Key,
                                                   Metadata = x.Key,
                                                   Name     = Language.GetText(x.Key)
                                               });
            blockCollection.AddMany(fakeBlocks);

            blockCollection.AddMany(blockInGroup);
            blockCollection.AddMany(simpleBlock);
            return Xaml.Get<IDialogService>()
                       .Dialog(new EditStringPresentationViewModel(), new Parameter
                       {
                           Args = new object[]
                           {
                               hb,
                               blockCollection,
                           }
                       });
        }

        protected override bool IsCompleted() => true;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var param = parameter.Parameter;
            Block = param.Args[0] as StringPresentation;
            var array = param.Args[1] as IEnumerable<ModuleBlock>;
            Templates.AddMany(array, true);
        }

        protected override void Finish()
        {
            if (SelectedBlock is null)
            {
                return;
            }

            var r = SelectedBlock;
            
            
            var spb = Block;
            spb.IsMetadata    = !string.IsNullOrEmpty(r.Metadata);
            spb.ValueSourceID = spb.IsMetadata ? r.Metadata : r.Id; 
        }

        protected override string Failed() => SubSystemString.Unknown;

        private ModuleBlock _selectedBlock;

        /// <summary>
        /// 获取或设置 <see cref="SelectedBlock"/> 属性。
        /// </summary>
        public ModuleBlock SelectedBlock
        {
            get => _selectedBlock;
            set => SetValue(ref _selectedBlock, value);
        }

        public StringPresentation Block { get; private set; }
        public ObservableCollection<ModuleBlock> Templates { get; } = new ObservableCollection<ModuleBlock>();
    }
}