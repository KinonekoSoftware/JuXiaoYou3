
using System.Windows.Media;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
// ReSharper disable IdentifierTypo

namespace Acorisoft.FutureGL.MigaStudio.Models.Presentations
{
    public abstract class PresentationDataUI : ObservableObject
    {
        public static PresentationDataUI GetDataUI(IPresentationData value)
        {
            return value switch
            {
                PresentationTextData ptd => new PresentationTextDataUI(ptd),
                PresentationDegreeData pdd => new PresentationDegreeDataUI(pdd),
                PresentationStarData psd1 => new PresentationStarDataUI(psd1),
                PresentationSwitchData psd2 => new PresentationSwitchDataUI(psd2),
                PresentationHeartData phd => new PresentationHeartDataUI(phd),
                PresentationProgressData ppd => new PresentationProgressDataUI(ppd),
                PresentationRateData  prd => new PresentationRateDataUI(prd),
                PresentationLikabilityData pld => new PresentationLikabilityDataUI(pld),
                PresentationColorData pld => new PresentationColorDataUI(pld),
                _ => throw new InvalidOperationException("没有这种数据")
            };
        }

        protected PresentationDataUI(IPresentationData data)
        {
            IsMetadata = data.IsMetadata;
        }

        public virtual void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            
        }

        public bool GetBooleanValue(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            if (IsMetadata)
            {
                return bool.TryParse(metadataTracker(Metadata)?.Value, out var n) && n;
            }
            
            return ((IMetadataBooleanSource)blockTracker(Metadata))?.GetValue() ?? false;
        }
        
        public int GetNumberValue(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            if (IsMetadata)
            {
                var v = metadataTracker(Metadata)?.Value;
                return int.TryParse(v, out var n) ? n : 0;
            }
            
            return ((IMetadataNumericSource)blockTracker(Metadata))?.GetValue() ?? 0;
        }
        
        public string GetStringValue(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            return IsMetadata ? metadataTracker(Metadata)?.Value : ((IMetadataTextSource)blockTracker(Metadata))?.GetValue();
        }
        
        public bool IsMetadata { get; }
        public string Metadata { get; protected init; }
        public string Name { get; init; }
    }

    public sealed class PresentationStarDataUI : PresentationDataUI
    {
        private bool _value;
        
        public PresentationStarDataUI(IPresentationData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;

        }

        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            Value = GetBooleanValue(metadataTracker, blockTracker);
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public bool Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    public sealed class PresentationSwitchDataUI : PresentationDataUI
    {
        private bool _value;
        
        public PresentationSwitchDataUI(IPresentationData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
        }


        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            Value = GetBooleanValue(metadataTracker, blockTracker);
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public bool Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    public sealed class PresentationRateDataUI : PresentationDataUI
    {
        private int _value;
        private int _metadataValue;
        
        public PresentationRateDataUI(PresentationRateData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
            Scale    = value.Scale;
        }
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            MetadataValue = GetNumberValue(metadataTracker, blockTracker);
            Value         = (int)(MetadataValue * Scale);
        }
        
        public double Scale { get; }

        /// <summary>
        /// 获取或设置 <see cref="MetadataValue"/> 属性。
        /// </summary>
        public int MetadataValue
        {
            get => _metadataValue;
            set => SetValue(ref _metadataValue, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public int Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    
    public sealed class PresentationLikabilityDataUI : PresentationDataUI
    {
        private int _value;
        private int _metadataValue;
        
        public PresentationLikabilityDataUI(PresentationLikabilityData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
            Scale    = value.Scale;
        }
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            MetadataValue = GetNumberValue(metadataTracker, blockTracker);
            Value         = (int)(MetadataValue * Scale);
        }
        public double Scale { get; }

        /// <summary>
        /// 获取或设置 <see cref="MetadataValue"/> 属性。
        /// </summary>
        public int MetadataValue
        {
            get => _metadataValue;
            set => SetValue(ref _metadataValue, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public int Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }

    public sealed class PresentationDegreeDataUI : PresentationDataUI
    {
        private int _value;
        private int _metadataValue;
        
        public PresentationDegreeDataUI(PresentationDegreeData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
            Scale    = value.Scale;
        }
        
        
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            MetadataValue = GetNumberValue(metadataTracker, blockTracker);
            Value         = (int)(MetadataValue * Scale);
        }
        
        public double Scale { get; }
        
        /// <summary>
        /// 获取或设置 <see cref="MetadataValue"/> 属性。
        /// </summary>
        public int MetadataValue
        {
            get => _metadataValue;
            set => SetValue(ref _metadataValue, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public int Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    public sealed class PresentationProgressDataUI : PresentationDataUI
    {
        private int _value;
        private int _metadataValue;
        
        public PresentationProgressDataUI(PresentationProgressData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
            Scale    = value.Scale;
        }

        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            MetadataValue = GetNumberValue(metadataTracker, blockTracker);
            Value         = (int)(MetadataValue * Scale);
        }
        public double Scale { get; }
        /// <summary>
        /// 获取或设置 <see cref="MetadataValue"/> 属性。
        /// </summary>
        public int MetadataValue
        {
            get => _metadataValue;
            set => SetValue(ref _metadataValue, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public int Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    public sealed class PresentationColorDataUI : PresentationDataUI
    {
        private SolidColorBrush _value;
        
        public PresentationColorDataUI(IPresentationData value) : base(value)
        {
            Name       = value.Name;
            Metadata   = value.ValueSourceID;
        }

        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            var raw = GetStringValue(metadataTracker, blockTracker);
            var color = Xaml.FromHex(raw);
            Value = new SolidColorBrush(color);
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public SolidColorBrush Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    public sealed class PresentationTextDataUI : PresentationDataUI
    {
        private string _value;
        
        public PresentationTextDataUI(IPresentationData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
        }

        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            Value = GetStringValue(metadataTracker, blockTracker);
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public string Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }

    public sealed class PresentationHeartDataUI : PresentationDataUI
    {
        private bool _value;
        
        public PresentationHeartDataUI(IPresentationData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
        }


        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            Value = GetBooleanValue(metadataTracker, blockTracker);
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public bool Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
}