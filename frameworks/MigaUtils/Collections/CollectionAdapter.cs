using System.Collections;

namespace Acorisoft.FutureGL.MigaUtils.Collections
{
    public class CollectionAdapter<T> : IList<T>, IList, IReadOnlyList<T>
    {
        private readonly IList<T> _items;


        public CollectionAdapter(IList<T> list)
        {
            if (ReferenceEquals(list, this))
            {
                throw new InvalidOperationException(nameof(list));
            }
            
            _items = list ?? throw new ArgumentNullException(nameof(list));
        }

        public int Count => _items.Count;

        protected IList<T> Items => _items;

        public T this[int index]
        {
            get => _items[index];
            set
            {
                if (_items.IsReadOnly)
                    throw new NotSupportedException("只读集合，不支持修改");
                if ((uint)index >= (uint)_items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                SetItem(index, value);
            }
        }

        public void Add(T item)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException("只读集合，不支持修改");
            InsertItem(_items.Count, item);
        }

        public void Clear()
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException("只读集合，不支持修改");
            ClearItems();
        }

        public void CopyTo(T[] array, int index) => _items.CopyTo(array, index);

        public bool Contains(T item) => _items.Contains(item);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        public int IndexOf(T item) => _items.IndexOf(item);

        public void Insert(int index, T item)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException("只读集合，不支持修改");
            if ((uint)index >= (uint)_items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            InsertItem(index, item);
        }

        public bool Remove(T item)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException("只读集合，不支持修改");
            var index = _items.IndexOf(item);
            if (index < 0)
                return false;
            RemoveItem(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException("只读集合，不支持修改");
            if ((uint)index >= (uint)_items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            RemoveItem(index);
        }

        protected virtual void ClearItems() => _items.Clear();

        protected virtual void InsertItem(int index, T item) => _items.Insert(index, item);

        protected virtual void RemoveItem(int index) => _items.RemoveAt(index);

        protected virtual void SetItem(int index, T item) => _items[index] = item;

        bool ICollection<T>.IsReadOnly => _items.IsReadOnly;

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        bool ICollection.IsSynchronized => false;


        object ICollection.SyncRoot => ((IList)_items).SyncRoot;


        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new InvalidOperationException(nameof(array));
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new InvalidOperationException(nameof(array));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            
            if (array.Length - index < Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            
            if (array is T[] array1)
            {
                _items.CopyTo(array1, index);
            }
            else
            {
                var elementType = array.GetType().GetElementType();
                var c           = typeof(T);
                if (elementType != null && 
                    !elementType.IsAssignableFrom(c) && 
                    !c.IsAssignableFrom(elementType))
                {
                    throw new InvalidCastException(nameof(array));
                }
                
                if (array is not object[] objArray)
                {
                    throw new InvalidCastException(nameof(array));
                }
                
                var count = _items.Count;
                try
                {
                    for (var index1 = 0; index1 < count; ++index1)
                    {
                        var index2 = index++;
                        // ISSUE: variable of a boxed type
                        var local = (object)_items[index1];
                        objArray[index2] = local;
                    }
                }
                catch (ArrayTypeMismatchException ex)
                {
                    throw new InvalidCastException(nameof(array), ex);
                }
            }
        }


        object IList.this[int index]
        {
            get => _items[index];
            set
            {
                if (_items.IsReadOnly)
                {
                    throw new NotSupportedException("只读集合，不支持修改");
                }

                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this[index] = (T)value;
            }
        }

        bool IList.IsReadOnly => _items.IsReadOnly;

        bool IList.IsFixedSize => _items is IList list ? list.IsFixedSize : _items.IsReadOnly;


#nullable disable
        int IList.Add(object value)
        {
            if (_items.IsReadOnly)
            {
                throw new NotSupportedException("只读集合，不支持修改");
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Add((T)value);
            return Count - 1;
        }

        bool IList.Contains(object value) => IsCompatibleObject(value) && Contains((T)value);

        int IList.IndexOf(object value) => IsCompatibleObject(value) ? IndexOf((T)value) : -1;

        void IList.Insert(int index, object value)
        {
            if (_items.IsReadOnly)
            {
                throw new NotSupportedException("只读集合，不支持修改");
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException("只读集合，不支持修改");
            if (!IsCompatibleObject(value))
                return;
            Remove((T)value);
        }

        private static bool IsCompatibleObject(object value)
        {
            if (value is T)
                return true;
            return value == null && default(T) == null;
        }
    }
}