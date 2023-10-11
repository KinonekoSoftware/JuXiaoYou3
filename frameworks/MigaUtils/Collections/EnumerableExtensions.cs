using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaUtils.Collections
{
    public static class EnumerableExtensions
    {
        private static void Empty<T>(T a, int b, int c){}

        public static int IndexOf<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (predicate is null)
            {
                return -1;
            }

            if (collection is null)
            {
                return -1;
            }

            var i = 0;
            
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        #region ForEach

        
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> handler)
        {
            if (collection is null)
            {
                return;
            }

            if (handler is null)
            {
                return;
            }

            foreach (var item in collection)
            {
                handler(item);
            }
        }

        #endregion
        
        #region ShiftUp

                
        public static void ShiftUp<T>(this ObservableCollection<T> source, T target)
        {
            ShiftUp(source, target, Empty); 
        }
        
        public static void ShiftUp<T>(this ObservableCollection<T> source, T target, Action callback)
        {
            ShiftUp(source, target, (_, _, _) => callback?.Invoke()); 
        }
        
        public static void ShiftUp<T>(this List<T> source, T target)
        {
            ShiftUp(source, target, Empty); 
        }
        
        public static void ShiftUp<T>(this List<T> source, T target, Action callback)
        {
            ShiftUp(source, target, (_, _, _) => callback?.Invoke()); 
        }

        public static void ShiftUp<T>(this List<T> source, T target, Action<T, int, int> callback)
        {
            if (ReferenceEquals(target, default(T)))
            {
                return;
            }

            if (source is null)
            {
                return;
            }

            var index = source.IndexOf(target);


            if (index < 1)
            {
                return;
            }
            
            source.RemoveAt(index);
            source.Insert(index - 1, target);
            callback?.Invoke(target, index, index - 1);
        }

        public static void ShiftUp<T>(this ObservableCollection<T> source, T target, Action<T, int, int> callback)
        {
            if (ReferenceEquals(target, default(T)))
            {
                return;
            }

            if (source is null)
            {
                return;
            }

            var index = source.IndexOf(target);

            if (index < 1)
            {
                return;
            }
            
            source.Move(index, index - 1);
            
            callback?.Invoke(target, index, index - 1);
        }
        
        #endregion

        #region ShiftDown

        public static void ShiftDown<T>(this ObservableCollection<T> source, T target)
        {
            ShiftDown(source, target, Empty); 
        }
        
        public static void ShiftDown<T>(this List<T> source, T target)
        {
            ShiftDown(source, target, Empty); 
        }
        
        public static void ShiftDown<T>(this List<T> source, T target, Action callback)
        {
            ShiftDown(source, target, (_, _, _) => callback?.Invoke());
        }
        
        public static void ShiftDown<T>(this ObservableCollection<T> source, T target, Action callback)
        {
            ShiftDown(source, target, (_, _, _) => callback?.Invoke());
        }

        public static void ShiftDown<T>(this List<T> source, T target, Action<T, int, int> callback)
        {
            if (ReferenceEquals(target, default(T)))
            {
                return;
            }

            if (source is null)
            {
                return;
            }

            var index = source.IndexOf(target);

            if (index >= source.Count - 1)
            {
                return;
            }
            
            source.RemoveAt(index);
            source.Insert(index + 1, target);
            callback?.Invoke(target, index, index + 1);
        }
        
        
        
        public static void ShiftDown<T>(this ObservableCollection<T> source, T target, Action<T, int, int> callback)
        {
            if (ReferenceEquals(target, default(T)))
            {
                return;
            }

            if (source is null)
            {
                return;
            }

            var index = source.IndexOf(target);

            if (index >= source.Count - 1)
            {
                return;
            }
            
            source.Move(index, index + 1);
            callback?.Invoke(target, index, index + 1);
        }
        
        #endregion

        public static void Diff<TItem, TKey>(
            IEnumerable<TItem> source, 
            IEnumerable<TItem> target, 
            Func<TItem, TKey> keySelector, 
            out List<TItem> added, 
            out List<TItem> modified,
            out List<TItem> removed)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var sourceHash = new HashSet<TKey>(source.Select(keySelector));
            var targetHash = new HashSet<TKey>();
            modified   = new List<TItem>();
            added      = new List<TItem>();
            
            foreach (var item in target)
            {
                var key = keySelector(item);
                
                if (sourceHash.Remove(key))
                {
                    //
                    // 相同
                    modified.Add(item);
                }
                else
                {
                    added.Add(item);
                }
            }
            removed = source.Where(x => sourceHash.Contains(keySelector(x)))
                                .ToList();
        }
    }
}