using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Metadatas
{
    /// <summary>
    /// <see cref="MetadataCollection"/> 类型表示一个元数据集合。
    /// </summary>
    public class MetadataCollection : ObservableCollection<Metadata>
    {
        private readonly Dictionary<string, int> IndexByType = new Dictionary<string, int>();

        protected override void InsertItem(int index, Metadata item)
        {
            if (item is null)
            {
                return;
            }
            
            if (IndexByType.TryGetValue(item.Name, out var inside))
            {
                this[inside].Value = item.Value;
            }
            else
            {
                IndexByType.Add(item.Name, index);
            }
            
            base.InsertItem(index, item);
        }

        protected override void ClearItems()
        {
            IndexByType.Clear();
            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            var value = this[index];
            IndexByType.Remove(value.Name);
            
            base.RemoveItem(index);
        }

        public void Replace(Metadata metadata)
        {
            for (var i = 0; i < Count; i++)
            {
                var inside = this[i];

                if (inside.Name == metadata.Name)
                {
                    inside.Value = metadata.Value;
                }
            }
        }

        public void Update(Metadata metadata)
        {
            if (IndexByType.TryGetValue(metadata.Name, out var index))
            {
                this[index].Value = metadata.Value;
            }
            else
            {
                Add(metadata);
            }
        }
        
        public Metadata GetMetadata(string metaName)
        {
            return IndexByType.TryGetValue(metaName, out var val) ? this[val] : null;
        }

        public string GetMetadataValue(string metaName)
        {
            return IndexByType.TryGetValue(metaName, out var val) ? this[val].Value : string.Empty;
        }
        
        public bool GetMetadataValue(string metaName, out string val)
        {
            if (IndexByType.TryGetValue(metaName, out var metadata))
            {
                val = this[metadata].Value;
                return true;
            }

            val = string.Empty;
            return false;
        }

    }
}