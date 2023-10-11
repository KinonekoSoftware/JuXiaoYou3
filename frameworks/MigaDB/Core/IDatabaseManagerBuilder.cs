using MediatR;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    /// <summary>
    /// <see cref="IDatabaseManagerBuilder"/> 类型接口表示一个数据库管理器建造工具。用来初始化数据库所需的环境。
    /// </summary>
    public interface IDatabaseManagerBuilder
    {
        /// <summary>
        /// 注册数据引擎。
        /// </summary>
        /// <typeparam name="TEngine">指定的引擎类型。</typeparam>
        /// <param name="lazyMode">是否为懒加载模式</param>
        /// <returns>返回一个<see cref="IDatabaseManagerBuilder"/></returns>
        IDatabaseManagerBuilder Setup<TEngine>(bool lazyMode = false) where TEngine :
            class,
            IDataEngine,
            INotificationHandler<DatabaseOpenNotification>,
            INotificationHandler<DatabaseCloseNotification>;

        /// <summary>
        /// 注册升级器。
        /// </summary>
        /// <typeparam name="TMigration">指定的升级器类型。</typeparam>
        /// <returns>返回一个<see cref="IDatabaseManagerBuilder"/></returns>
        IDatabaseManagerBuilder Update<TMigration>() where TMigration : class, IDatabaseUpdater;

        /// <summary>
        /// 注册数据维护工具，用于检测、初始化数据库中的数据。
        /// </summary>
        /// <typeparam name="TMaintainer">指定的维护工具类型。</typeparam>
        /// <returns>返回一个<see cref="IDatabaseManagerBuilder"/></returns>
        IDatabaseManagerBuilder Maintain<TMaintainer>() where TMaintainer : class, IDatabaseMaintainer;
        
        /// <summary>
        /// 构建引擎。
        /// </summary>
        /// <param name="databaseVersion">数据库当前版本。</param>
        /// <returns>返回一个<see cref="IDatabaseManager"/></returns>
        DatabaseManager Build(int databaseVersion);
    }
}