using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaUtils.Collections
{
    public class ObservableCollectionAdapter<T> : IList<T>, IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private PropertyChangedEventHandler         PropertyChangedHandler;
        private NotifyCollectionChangedEventHandler CollectionChangedHandler;

        private readonly object   _sync;
        private          IList<T> _List;
        private          bool     _ReadOnly;

        #region EmptyEnumerator

        public struct EmptyEnumerator : IEnumerator<T>
        {
            public bool MoveNext() => false;

            public void Reset(){}

            public T Current => default(T);

            object IEnumerator.Current => Current;

            public void Dispose(){}
        }

        #endregion

        public ObservableCollectionAdapter()
        {
            _sync     = new object();
            _ReadOnly = true;
        }

        #region Handlers


        protected internal void RaiseUpdated(PropertyChangedEventArgs e)
        {
            PropertyChangedHandler?.Invoke(this, e);
        }

        protected void RaiseUpdated(NotifyCollectionChangedEventArgs e)
        {
            CollectionChangedHandler?.Invoke(this, e);
        }

        #endregion

        public void Adapt(IList<T> list)
        {
            if (list is null)
            {
                return;
            }

            _ReadOnly = list.IsReadOnly;
            _List     = list;
            
            Sync();
        }

        public void Sync()
        {
            //
            // 清空集合
            OnIndexerPropertyChanged();
            OnCountPropertyChanged();
            OnCollectionReset();
        }

        #region Insert / Add

        public void Insert(int index, T item)
        {
            if (_List is null)
            {
                return;
            }

            if (_ReadOnly)
            {
                return;
            }
            
            InsertItem(index, item);
        }

        public void Insert(int index, object value)
        {
            if (value is T instance)
            {
                Insert(index, instance);
            }
        }
        
        public void Add(T item)
        {
            if (IsReadOnly)
            {
                return;
            }
            
            InsertItem(_List.Count, item);
        }

        public int Add(object value)
        {
            if (value is T instance)
            {
                Add(instance);
                return _List.Count - 1;
            }

            return -1;
        }

        

        #endregion

        #region Remove

        public void Clear()
        {
            if (_List is null)
            {
                return;
            }
            
            if (IsReadOnly)
            {
                return;
            }

            var oldItem = _List.ToArray();
            ClearItems(oldItem);
        }

        public void Remove(object value)
        {
            if (value is T instance)
            {
                Remove(instance);
            }
        }

        void IList.RemoveAt(int index)
        {
            if (_List is null)
            {
                return;
            }

            if (_ReadOnly)
            {
                return;
            }

            if (index < 0 || index >= _List.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            
            RemoveItem(index, _List[index]);
        }

        void IList<T>.RemoveAt(int index)
        {
            if (_List is null)
            {
                return;
            }

            if (_ReadOnly)
            {
                return;
            }

            if (index < 0 || index >= _List.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            
            RemoveItem(index, _List[index]);
        }

        public bool Remove(T item)
        {
            if (_List is null)
            {
                return false;
            }

            if (_ReadOnly)
            {
                return false;
            }

            var count = _List.Count;
            RemoveItem(IndexOf(item), item);
            return count != _List.Count;
        }

        #endregion

        #region IndexOf / Contains
        

        public int IndexOf(object value)
        {
            return value is T instance ? IndexOf(instance) : -1;
        }
        
        public int IndexOf(T item)
        {
            return _List?.IndexOf(item) ?? -1;
        }



        #endregion

        #region CopyTo

        
        public void CopyTo(Array array, int index)
        {
            return;
        }
        
        public void CopyTo(T[] array, int arrayIndex)
        {
            _List.CopyTo(array, arrayIndex);
        }

        #endregion
        
        #region Override

        protected virtual void InsertItem(int index, T item)
        {
            _List.Insert(index, item);
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        protected virtual void ClearItems(T[] item)
        {
            _List.Clear();
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionReset();
        }

        protected virtual void SetItem(int index, T oldItem, T newItem)
        {
            _List[index] = newItem;
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, newItem, index);
        }

        protected virtual void RemoveItem(int index, T oldItem)
        {
            _List.Remove(oldItem);
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, oldItem, index);
        }
        
        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            var obj = this[oldIndex];
            RemoveItem(oldIndex, obj);
            InsertItem(newIndex, obj);
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Move, obj, newIndex, oldIndex);
        }
        #endregion

        #region Contains


        public bool Contains(object value)
        {
            return value is T instance && Contains(instance);
        }

        
        public bool Contains(T item)
        {
            return _List is not null && _List.Contains(item);
        }

        #endregion

        #region GetEnumerator

        
        public IEnumerator<T> GetEnumerator()
        {
            return _List is null ? new EmptyEnumerator() : _List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region OnCollectionChanged

        private void OnCollectionChanged(
            NotifyCollectionChangedAction action,
            object item,
            int index,
            int oldIndex)
        {
            
            CollectionChangedHandler?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        private void OnCollectionChanged(
            NotifyCollectionChangedAction action,
            object oldItem,
            object newItem,
            int index)
        {
            
            CollectionChangedHandler?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        private void OnCollectionReset()
        {
            CollectionChangedHandler?.Invoke(this, CollectionProperties.ResetCollectionChanged);
        }
        
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            CollectionChangedHandler?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item, index));
        }
        
        private void OnCountPropertyChanged() => RaiseUpdated(CollectionProperties.CountPropertyChanged);
        private void OnIndexerPropertyChanged() => RaiseUpdated(CollectionProperties.IndexerPropertyChanged);

        #endregion
        
        #region SetItem / GetItem

        
        
        protected T GetItem(int index)
        {
            if (_List is null)
            {
                return default(T);
            }

            if (index < 0 || index >= _List.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _List[index];
        }


        protected void SetItem(int index, T value)
        {
            if (_List is null)
            {
                return;
            }

            if (_ReadOnly)
            {
                return;
            }
                
            if (index < 0 || index >= _List.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var oldItem = _List[index];
            SetItem(index, oldItem, value);
        }
        
        #endregion

        /// <summary>
        /// 所有元素
        /// </summary>
        protected IList<T> Items => _List;

        /// <summary>
        /// 集合索引器
        /// </summary>
        /// <param name="index">指定要访问的索引位置。</param>
        /// <exception cref="ArgumentOutOfRangeException">索引位置超出了集合边界。</exception>
        public T this[int index]
        {
            get => GetItem(index);
            set => SetItem(index, value);
        }
        
        /// <summary>
        /// 获取当前集合的元素个数。
        /// </summary>
        public int Count => _List?.Count ?? 0;

        /// <summary>
        /// 是否为同步集合
        /// </summary>
        public bool IsSynchronized => true;
        
        /// <summary>
        /// 同步对象
        /// </summary>
        public object SyncRoot => _sync;

        /// <summary>
        /// 当前集合是否只读。
        /// </summary>
        public bool IsReadOnly => _List?.IsReadOnly ?? true;

        /// <summary>
        /// 是否为固定大小
        /// </summary>
        public bool IsFixedSize => (_List as IList)?.IsFixedSize ?? true;

        object IList.this[int index]
        {
            get => GetItem(index);
            set
            {
                if (value is T instance)
                {
                    SetItem(index, instance);
                }
            }
        }

        
        public IList<T> List => _List;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add => PropertyChangedHandler += value;
            remove => PropertyChangedHandler -= value;
        }
        
        
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => CollectionChangedHandler += value;
            remove => CollectionChangedHandler += value;
        }

    }
}