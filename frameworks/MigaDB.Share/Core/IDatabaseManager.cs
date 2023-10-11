using Acorisoft.FutureGL.MigaDB.Services;
using DryIoc;
using MediatR;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    /// <summary>
    /// 表示一个数据库管理器。
    /// </summary>
    public interface IDatabaseManager : IDisposable
    {
        /// <summary>
        /// 加载指定的世界观
        /// </summary>
        /// <param name="directory">世界观目录</param>
        /// <param name="mode">数据模式</param>
        /// <returns>返回一个操作结果</returns>
        Task<DatabaseResult> LoadAsync(string directory, DatabaseMode mode = DatabaseMode.Release);

        /// <summary>
        /// 在指定目录创建世界观
        /// </summary>
        /// <param name="directory">世界观目录</param>
        /// <param name="property">基础属性</param>
        /// <returns>返回一个操作结果</returns>
        Task<DatabaseResult> CreateAsync(string directory, DatabaseProperty property);

        /// <summary>
        /// 保存当前数据库的对象更改。
        /// </summary>
        /// <returns>返回一个操作结果</returns>
        Task<DatabaseResult> CacheAsync();

        /// <summary>
        /// 关闭世界观。
        /// </summary>
        /// <returns>返回一个操作结果</returns>
        Task<DatabaseResult> CloseAsync();

        /// <summary>
        /// 获取引擎。
        /// </summary>
        /// <typeparam name="TEngine">指定的引擎类型。</typeparam>
        /// <returns>返回指定的引擎类型（如果存在），否则返回null。</returns>
        TEngine GetEngine<TEngine>() where TEngine : IDataEngine;
        
        /// <summary>
        /// 获得所有数据引擎。
        /// </summary>
        /// <returns>返回一个数据引擎的枚举。</returns>
        IEnumerable<IDataEngine> GetEngines();
        
        /// <summary>
        /// 当前打开的引擎。
        /// </summary>
        /// <remarks>
        /// 如果没有打开，则为null。
        /// </remarks>
        IObservableProperty<IDatabase> Database { get; }
        
        /// <summary>
        /// 当前打开的引擎。
        /// </summary>
        /// <remarks>
        /// 如果没有打开，则为null。
        /// </remarks>
        IObservableProperty<DatabaseProperty> Property { get; }
        
        /// <summary>
        /// 是否加载数据库。
        /// </summary>
        IObservableState IsOpen { get; }
        
        /// <summary>
        /// 是否加载数据库。
        /// </summary>
        IObservableState NeedUpdate { get; }
        
        /// <summary>
        /// Ioc容器，请勿在第三方框架中使用。
        /// </summary>
        IContainer Container { get; }
        
        /// <summary>
        /// 中介器
        /// </summary>
        IMediator Mediator { get; }
        
        
        /// <summary>
        /// 数据库升级工具
        /// </summary>
        IEnumerable<IDatabaseUpdater> Updaters { get; }

        /// <summary>
        /// 数据库维护工具
        /// </summary>
        IEnumerable<IDatabaseMaintainer> Maintainers { get; }
    }
}