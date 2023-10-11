using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
using ListBox = Acorisoft.FutureGL.Forest.Controls.ListBox;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class EditPresentationViewModel : ImplicitDialogVM
    {
        
        public static Task<Op<object>> Edit(GroupingPresentation hb, DataPartCollection dataPartCollection)
        {
            var blockCollection = new List<ModuleBlock>(64);
            var blocks = dataPartCollection.Where(x => x is PartOfModule)
                                           .OfType<PartOfModule>()
                                           .Select(x => x.Blocks)
                                           .SelectMany(x => x)
                                           .ToArray();

            var simpleBlock = blocks.Where(x => x is not GroupBlock)
                                    .Where(x => x.ExtractType == hb.Type);

            var blockInGroup = blocks.Where(x => x is GroupBlock)
                                     .OfType<GroupBlock>()
                                     .SelectMany(x => x.Items)
                                     .Where(x => x.ExtractType == hb.Type);

            blockCollection.AddMany(blockInGroup);
            blockCollection.AddMany(simpleBlock);
            return Xaml.Get<IDialogService>()
                       .Dialog(new EditPresentationViewModel(), new Parameter
                       {
                           Args = new object[]
                           {
                               hb,
                               blockCollection
                           }
                       });
        }
        
        

        protected override bool IsCompleted() => true;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var param = parameter.Parameter;
            Block = param.Args[0] as GroupingPresentation;
            var array = param.Args[1] as IEnumerable<ModuleBlock>;
            Templates.AddMany(array, true);
        }

        protected override void Finish()
        {
            if (TargetElement is null)
            {
                return;
            }

            var r = TargetElement.SelectedItems
                                 .Cast<ModuleBlock>()
                                 .ToArray();
            Block.DataLists
                 .AddMany(Create(r));
        }

        private IEnumerable<IPresentationData> Create(IEnumerable<ModuleBlock> blockCollection)
        {
            return Block.Type switch
            {
                MetadataKind.Text       => CreateText(blockCollection),
                MetadataKind.Degree     => CreateDegree(blockCollection),
                MetadataKind.Star       => CreateStar(blockCollection),
                MetadataKind.Heart      => CreateHeart(blockCollection),
                MetadataKind.Progress   => CreateProgress(blockCollection),
                MetadataKind.Switch     => CreateSwitch(blockCollection),
                MetadataKind.Color      => CreateColor(blockCollection),
                MetadataKind.Likability => CreateLikability(blockCollection),
                MetadataKind.Rate       => CreateRate(blockCollection),
                _                       => throw new InvalidOperationException(),
            };
        }

        private static IEnumerable<IPresentationData> CreateText(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PresentationTextData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPresentationData> CreateDegree(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                var scale = 1d;
                if (x is DegreeBlock number)
                {
                    scale = 8d / number.Maximum;
                }

                return new PresentationDegreeData
                {
                    Name = x.Name,
                    Scale = scale,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata = !useID
                };
            });
        }

        private static IEnumerable<IPresentationData> CreateStar(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PresentationStarData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPresentationData> CreateHeart(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PresentationHeartData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPresentationData> CreateProgress(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                var scale = 1d;
                if (x is INumberBlock number)
                {
                    scale = 100d / number.Maximum;
                }

                return new PresentationProgressData
                {
                    Name          = x.Name,
                    Scale         = scale,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPresentationData> CreateLikability(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                var scale = 1d;

                if (x is INumberBlock number)
                {
                    scale = 100d / number.Maximum;
                }

                return new PresentationLikabilityData
                {
                    Name          = x.Name,
                    Scale         = scale,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }
        
        
        private static IEnumerable<IPresentationData> CreateRate(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                var scale = 1d;

                if (x is INumberBlock number)
                {
                    scale = 100d / number.Maximum;
                }

                return new PresentationRateData
                {
                    Name          = x.Name,
                    Scale         = scale,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPresentationData> CreateSwitch(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PresentationSwitchData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }


        private static IEnumerable<IPresentationData> CreateColor(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PresentationColorData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }
        
        protected override string Failed() => SubSystemString.Unknown;

        private ListBox _targetElement;

        /// <summary>
        /// 获取或设置 <see cref="TargetElement"/> 属性。
        /// </summary>
        public ListBox TargetElement
        {
            get => _targetElement;
            set => SetValue(ref _targetElement, value);
        }
        
        public GroupingPresentation Block { get; private set; }
        public ObservableCollection<ModuleBlock> Templates { get; } = new ObservableCollection<ModuleBlock>();
    }
}