namespace Acorisoft.FutureGL.MigaDB.Core
{
    public abstract class DatabaseMaintainer<T> : IDatabaseMaintainer where T : class
    {
        protected abstract T OnCreateInstance();

        protected abstract void OnFixInstance(T instance);

        /// <summary>
        /// 是否为无效的数据。
        /// </summary>
        /// <param name="instance">要检查的数据类型。</param>
        /// <returns>如果为无效的数据，则返回True，否则返回False。</returns>
        protected abstract bool IsInvalidated(T instance);

        public void Maintain(IDatabase database)
        {
            T instance;

            if (database.Has<T>())
            {
                instance = database.Get<T>();
                if (IsInvalidated(instance))
                {
                    OnFixInstance(instance);
                    database.Set<T>(instance);
                }
            }
            else
            {
                instance = OnCreateInstance();
                database.Set<T>(instance);
            }
        }
    }
}