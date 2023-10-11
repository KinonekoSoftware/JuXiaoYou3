using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Core;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public interface IInMemoryDatabaseService
    {
        /// <summary>
        /// 当数据库第一次打开时。
        /// </summary>
        /// <param name="databaseManager">数据库</param>
        void Start(IDatabaseManager databaseManager);

        /// <summary>
        /// 令数据失效，重新同步
        /// </summary>
        void InvalidateDataSource();
        
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="container">IoC容器</param>
        void Register(IContainer container);
        
        /// <summary>
        /// 取消注册服务
        /// </summary>
        /// <param name="container">IoC容器</param>
        void Unregister(IContainer container);

        /// <summary>
        /// 当数据库关闭时
        /// </summary>
        void Stop();
        
        int TrackingVersion { get; }
    }
}