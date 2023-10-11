using System.Diagnostics.CodeAnalysis;
using Acorisoft.FutureGL.MigaDB.Exceptions;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    public abstract class DatabaseUpdater : IDatabaseUpdater
    {
        /// <summary>
        /// 执行升级操作。
        /// </summary>
        /// <param name="database">指定要升级的数据库，要求不能为空。</param>
        protected abstract void Execute([NotNull]IDatabase database);
        
        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="database">指定要升级的数据库。</param>
        /// <returns>返回新的版本</returns>
        /// <exception cref="InvalidOperationException">当前升级器作出修改之后未提升版本。</exception>
        public bool Update(IDatabase database)
        {
            //
            // 确保代码不会出现空引用。
            if (database is null) return false;
            var oldVersion = database.Version;
            
            //
            // 判断当前版本是否符合当前升级器的目标
            if (oldVersion != TargetVersion) return false;

            try
            {
                //
                // 执行
                Execute(database);
                database.UpdateVersion(ResultVersion);
            }
            catch(Exception ex)
            {
                throw new UpdaterException(ex.Message, ex);
            }
            
            return true;
        }

        /// <summary>
        /// 升级操作针对的数据库版本。
        /// </summary>
        public abstract int TargetVersion { get; }
        
        
        /// <summary>
        /// 升级操作完成之后的数据库版本。
        /// </summary>
        public abstract int ResultVersion { get; }
        
        
    }
}