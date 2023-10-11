using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Exceptions;
using Acorisoft.FutureGL.MigaDB.Core.Maintainers;
using Acorisoft.FutureGL.MigaDB.Core.Migrations;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Concepts;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Data.Worldview;
using DryIoc;
using Acorisoft.FutureGL.MigaDB.Utils;
using MediatR;
using NLog;
using Directory = System.IO.Directory;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    public partial class DatabaseManager : Disposable, IDatabaseManager
    {
        /// <summary>
        /// 创建构建器。
        /// </summary>
        /// <returns>返回一个新的构建器实例。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDatabaseManagerBuilder CreateBuilder(ILogger logger)
        {
            return new DatabaseManagerBuilder(logger);
        }

        /// <summary>
        /// 创建默认数据库管理器。
        /// </summary>
        /// <returns>返回一个新的数据库管理器实例。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DatabaseManager GetDefaultDatabaseManager(ILogger logger)
        {
            return CreateBuilder(logger)
                   .Setup<TemplateEngine>()
                   .Setup<DocumentEngine>()
                   .Setup<ComposeEngine>()
                   .Setup<ImageEngine>()
                   .Setup<MusicEngine>()
                   .Setup<KeywordEngine>()
                   .Setup<SocialEngine>()
                   .Setup<ProjectEngine>()
                   .Setup<EntityEngine>()
                   .Maintain<DatabasePresetMaintainer>()
                   .Maintain<DatabasePropertiesMaintainer>()
                   .Maintain<ServicePropertyMaintainer>()
                   .Update<UpdaterOfVer001>()
                   .Build(Constants.DatabaseCurrentVersion);
        }
        
    }
}