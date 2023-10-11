using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations
{
    public interface IPresentationData : INotifyPropertyChanged
    {
        string Name { get; }
        string ValueSourceID { get; }
        bool IsMetadata { get; }
    }
}