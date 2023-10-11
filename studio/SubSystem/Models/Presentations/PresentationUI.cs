using System.Windows.Media;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using Brush = System.Windows.Media.Brush;

namespace Acorisoft.FutureGL.MigaStudio.Models.Presentations
{
    public abstract class PresentationUI : StorageUIObject
    {
        public static PresentationUI GetUI(Presentation block)
        {
            return block switch
            {
                GroupingPresentation gpb => new GroupingPresentationUI
                {
                    Source = gpb
                },
                ChartPresentation cpb => cpb.ChartType switch
                {
                    ChartType.Histogram => new HistogramPresentationUI
                    {
                        Source = cpb
                    },
                    ChartType.Radar => new RadarPresentationUI
                    {
                        Source = cpb
                    },
                    _ => throw new InvalidOperationException()
                },
                RarityPresentation rpb2 => new RarityPresentationUI()
                {
                    Source = rpb2
                },
                StringPresentation sp => new StringPresentationUI
                {
                    Source = sp
                },
                _ => null
            };
        }

        public abstract void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker);

        private string _name;

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public Presentation BaseSource { get; protected init; }
    }

    public class RarityPresentationUI : PresentationUI
    {
        private string _hex;
        private string _value;

        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            var cs = Xaml.Get<IColorService>();
            if (Source.IsMetadata)
            {
                Value = metadataTracker(ValueSource)?.Value;
            }
            else
            {
                var v = blockTracker(ValueSource) as IMetadataTextSource;
                Value = v?.GetValue().SubString(200);
            }

            Hex = cs.GetColor(Value);
            Brush = new SolidColorBrush(Xaml.FromHex(Hex));
        }

        public string ValueSource { get; init; }

        public RarityPresentation Source
        {
            get => (RarityPresentation)BaseSource;
            init
            {
                ValueSource = value.ValueSourceID;
                Name        = value.Name;
                BaseSource  = value;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public string Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Hex"/> 属性。
        /// </summary>
        public string Hex
        {
            get => _hex;
            set => SetValue(ref _hex, value);
        }

        private Brush _brush;

        /// <summary>
        /// 获取或设置 <see cref="Brush"/> 属性。
        /// </summary>
        public Brush Brush
        {
            get => _brush;
            set => SetValue(ref _brush, value);
        }
    }

    public class GroupingPresentationUI : PresentationUI
    {
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            DataLists.ForEach(x => x.Update(metadataTracker, blockTracker));
        }

        public GroupingPresentation Source
        {
            get => (GroupingPresentation)BaseSource;
            init
            {
                BaseSource = value;
                Name       = value.Name;
                NameLists  = new ObservableCollection<string>();
                DataLists  = new ObservableCollection<PresentationDataUI>();

                foreach (var data in value.DataLists)
                {
                    NameLists.Add(data.Name);
                    DataLists.Add(PresentationDataUI.GetDataUI(data));
                }
            }
        }

        public ObservableCollection<string> NameLists { get; private init; }
        public ObservableCollection<PresentationDataUI> DataLists { get; private init; }
    }

    public class StringPresentationUI : PresentationUI
    {
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            if (Source.IsMetadata)
            {
                Value = metadataTracker(ValueSource)?.Value
                                                    .SubString(200);
            }
            else
            {
                var v = blockTracker(ValueSource) as IMetadataTextSource;
                Value = v?.GetValue().SubString(200);
            }
        }

        public StringPresentation Source
        {
            get => (StringPresentation)BaseSource;
            init
            {
                BaseSource  = value;
                Name        = value.Name;
                ValueSource = value.ValueSourceID;
            }
        }

        private string _value;

        public string ValueSource { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public string Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }

    public class HistogramPresentationUI : PresentationUI
    {
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            var block = (ChartBlock)blockTracker(ValueSource);

            if (block is null)
            {
                return;
            }

            if (Source.IsMetadata)
            {
                var unparsedValue = metadataTracker(ValueSource)?.Value;
                MetadataProcessor.ExtractChartBaseFormatted(unparsedValue,
                    out var axis,
                    out var value,
                    out _,
                    out _,
                    out _,
                    out var color);
                Color = color;
                Axis.AddMany(axis, true);
                Value = new List<int>(value);
                RaiseUpdated(nameof(Value));
            }
            else if (block.Value is not null)
            {
                Color = block.Color;
                Axis.AddMany(block.Axis, true);
                Value = new List<int>(block.Value);
                RaiseUpdated(nameof(Value));
            }
        }

        public ChartPresentation Source
        {
            get => (ChartPresentation)BaseSource;
            init
            {
                BaseSource  = value;
                Name        = value.Name;
                ValueSource = value.ValueSourceID;
                Color       = "#007ACC";
                Value       = new List<int>();
                Axis        = new List<string>();
            }
        }

        public string ValueSource { get; init; }
        public string Color { get; private set; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; private set; }
    }

    public class RadarPresentationUI : PresentationUI
    {
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {

            if (Source.IsMetadata)
            {
                var unparsedValue = metadataTracker(ValueSource)?.Parameters;
                MetadataProcessor.ExtractChartBaseFormatted(unparsedValue,
                    out var axis,
                    out var value,
                    out _,
                    out _,
                    out _,
                    out var color);
                Color = color;
                Axis.AddMany(axis, true);
                Value = new List<int>(value);
                RaiseUpdated(nameof(Value));
            }
            else
            {
                var block = (ChartBlock)blockTracker(ValueSource);

                if (block?.Value is null)
                {
                    return;
                }
                
                Color = block.Color;
                Axis.AddMany(block.Axis, true);
                Value = new List<int>(block.Value);
                RaiseUpdated(nameof(Value));
            }
        }

        public ChartPresentation Source
        {
            get => (ChartPresentation)BaseSource;
            init
            {
                BaseSource  = value;
                Name        = value.Name;
                ValueSource = value.ValueSourceID;
                Color       = "#007ACC";
                Value       = new List<int>();
                Axis        = new List<string>();
            }
        }

        public string ValueSource { get; init; }
        public string Color { get; private set; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; private set; }
    }
}