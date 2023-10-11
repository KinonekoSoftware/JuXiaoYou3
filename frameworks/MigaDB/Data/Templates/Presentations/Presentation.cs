using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations
{
    public abstract class Presentation : StorageUIObject
    {
        private string _name;
        public int Index { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public MetadataKind Type { get; init; }
        
        public sealed override string ToString()
        {
            return Name;
        }

    }

    public sealed class RarityPresentation : Presentation
    {

        public bool IsMetadata { get; set; }
        public double Scale { get; init; }
        public string ValueSourceID { get; set; }
    }

    public sealed class StringPresentation : Presentation
    {
        public bool IsMetadata { get; set; }
        public string ValueSourceID { get; set; }
    }

    public sealed class GroupingPresentation : Presentation
    {
        public ObservableCollection<IPresentationData> DataLists { get; init; }
    }

    public sealed class ChartPresentation : Presentation
    {
        public string ValueSourceID { get; set; }
        public ChartType ChartType { get; init; }
        public bool IsMetadata { get; set; }
    }
}