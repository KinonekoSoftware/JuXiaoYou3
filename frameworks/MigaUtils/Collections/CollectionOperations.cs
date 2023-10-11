namespace Acorisoft.FutureGL.MigaUtils.Collections
{
    public static class CollectionOperations
    {
        #region AddMany

        public static void AddMany<T>(this ICollection<T> source, IEnumerable<T> target, bool clear = false)
        {
            if (source is null || target is null)
            {
                return;
            }

            if (source.IsReadOnly)
            {
                return;
            }

            if (clear)
            {
                source.Clear();
            }

            foreach (var item in target)
            {
                source.Add(item);
            }
        }

        public static void AddMany<T>(this Stack<T> source, IEnumerable<T> target, bool clear = false)
        {
            if (source is null || target is null)
            {
                return;
            }
            
            if (clear)
            {
                source.Clear();
            }

            foreach (var item in target)
            {
                source.Push(item);
            }
        }

        public static void AddMany<T>(this Queue<T> source, IEnumerable<T> target, bool clear = false)
        {
            if (source is null || target is null)
            {
                return;
            }
            
            if (clear)
            {
                source.Clear();
            }

            foreach (var item in target)
            {
                source.Enqueue(item);
            }
        }

        #endregion

        #region RemoveMany

        public static void RemoveMany<T>(this ICollection<T> source, IEnumerable<T> target)
        {
            if (source is null || target is null)
            {
                return;
            }

            if (source.IsReadOnly)
            {
                return;
            }

            foreach (var item in target)
            {
                source.Remove(item);
            }
        }

        #endregion

        #region Flat

        
        public static List<T> Flat<TSource, T>(
            this IEnumerable<TSource> collection, 
            Func<TSource, T> selectorA, 
            Func<TSource, T> selectorB)
        {
            if (collection is null ||
                selectorA is null ||
                selectorB is null)
            {
                return null;
            }

            var array = new List<T>(32);
            foreach (var item in collection)
            {
                array.Add(selectorA(item));
                array.Add(selectorB(item));
            }

            return array;
        }

        public static List<T> Flat<TSource, T>(
            this IEnumerable<TSource> collection,
            Func<TSource, T> selectorA,
            Func<TSource, T> selectorB, 
            Func<TSource, T> selectorC)
        {
            if (collection is null ||
                selectorA is null ||
                selectorB is null ||
                selectorC is null)
            {
                return null;
            }

            var array = new List<T>(32);
            foreach (var item in collection)
            {
                array.Add(selectorA(item));
                array.Add(selectorB(item));
                array.Add(selectorC(item));
            }

            return array;
        }
        
        public static List<T> Flat<TSource, T>(
            this IEnumerable<TSource> collection,
            Func<TSource, T> selectorA,
            Func<TSource, T> selectorB, 
            Func<TSource, T> selectorC, 
            Func<TSource, T> selectorD)
        {
            if (collection is null ||
                selectorA is null ||
                selectorB is null ||
                selectorC is null)
            {
                return null;
            }

            var array = new List<T>(32);
            foreach (var item in collection)
            {
                array.Add(selectorA(item));
                array.Add(selectorB(item));
                array.Add(selectorC(item));
                array.Add(selectorD(item));
            }

            return array;
        }
        
        public static List<T> Flat<TSource, T>(
            this IEnumerable<TSource> collection,
            Func<TSource, T> selectorA,
            Func<TSource, T> selectorB, 
            Func<TSource, T> selectorC, 
            Func<TSource, T> selectorD, 
            Func<TSource, T> selectorE)
        {
            if (collection is null ||
                selectorA is null ||
                selectorB is null ||
                selectorC is null)
            {
                return null;
            }

            var array = new List<T>(32);
            foreach (var item in collection)
            {
                array.Add(selectorA(item));
                array.Add(selectorB(item));
                array.Add(selectorC(item));
                array.Add(selectorD(item));
                array.Add(selectorE(item));
            }

            return array;
        }

        #endregion

        #region Diff


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
            modified = new List<TItem>();
            added    = new List<TItem>();

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

        #endregion

    }
}