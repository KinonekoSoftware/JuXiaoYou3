using System.Collections.Specialized;
using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaUtils.Collections
{
    public static class CollectionProperties
    {
        public static readonly PropertyChangedEventArgs         CountPropertyChanged   = new PropertyChangedEventArgs("Count");
        public static readonly PropertyChangedEventArgs         IndexerPropertyChanged = new PropertyChangedEventArgs("Item[]");
        public static readonly NotifyCollectionChangedEventArgs ResetCollectionChanged = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    }
}