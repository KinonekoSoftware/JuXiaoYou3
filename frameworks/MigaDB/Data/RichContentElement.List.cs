using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data
{
    public class ListContentElement : RichContentElement
    {
        public ObservableCollection<ListContentElementItem> Items { get; init; }
    }

    public class ListContentElementItem : ObservableObject
    {
        
    }
}