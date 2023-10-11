

namespace Acorisoft.Miga.Doc.Engines
{


    /// <summary>
    /// <see cref="StorageService"/> 类型表示一个数据库存储集合。
    /// </summary>
    public abstract class StorageService 
    {
        private volatile bool            _initialized;

        #region Override Methods

        protected internal abstract void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property);
        protected internal abstract void OnRepositoryClosing(RepositoryContext context);

        protected internal virtual void OnRepositorySetting(UserCredential credential)
        {
        }
        #endregion


        private void RepositorySetting(UserCredential credential)
        {
            Credential = credential;
            OnRepositorySetting(credential);
        }

        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="directory">要检查的目录</param>
        /// <returns>返回目录路径。</returns>
        protected static string CheckDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                return string.Empty;
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }


        /// <summary>
        /// 手动初始化，适合懒加载模式。
        /// </summary>
        /// <remarks>此操作有效能防止启动卡顿，在低配电脑上效果显著。但是如果硬盘IO本身就非常低，这个优化是效果甚微。</remarks>
        public bool ManualInitialized()
        {
            
            return true;
        }

        /// <summary>
        /// 用户凭证
        /// </summary>
        public UserCredential Credential { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLazyMode { get; protected set; }

        public bool IsInitialized => _initialized;

        public bool IsDisposed { get; private set; }
    }

    public static class NoSqlUtils
    {

        public static bool AlwaysTrue<T>(T _) => true;
    }
}