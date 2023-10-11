using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Utils;
using static Acorisoft.FutureGL.MigaDB.Constants;

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    public class ComposeEngine : DataEngine
    {
        #region Add

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="document">指定要添加的文档</param>
        /// <returns>返回操作结果</returns>
        public EngineResult AddComposeCache(ComposeCache document)
        {
            if (document is null)
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }

            if (string.IsNullOrEmpty(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.MissingId);
            }

            if (ComposeDB.HasID(document.Id) ||
                ComposeCacheDB.HasID(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.Duplicated);
            }


            ComposeCacheDB.Insert(document);

            //
            // 一致性检查
            //if (!ComposeDB.HasID(document.Id) ||
            //    !ComposeCacheDB.HasID(document.Id))
            //{
            //    return EngineResult.Failed(EngineFailedReason.ConsistencyBroken);
            //}

            //
            //
            Modified();
            return EngineResult.Successful;
        }

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="document">指定要添加的文档</param>
        /// <returns>返回操作结果</returns>
        public EngineResult AddCompose(Compose document)
        {
            if (document is null)
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }

            if (string.IsNullOrEmpty(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.MissingId);
            }

            if (document.Parts is null || document.Metas is null)
            {
                return EngineResult.Failed(EngineFailedReason.InputDataError);
            }

            if (!ComposeCacheDB.HasID(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.ConsistencyBroken);
            }

            ComposeDB.Insert(document);

            //
            // 一致性检查
            if (!ComposeDB.HasID(document.Id) ||
                !ComposeCacheDB.HasID(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.ConsistencyBroken);
            }

            //
            //
            Modified();
            return EngineResult.Successful;
        }

        #endregion

        #region Remove

        /// <summary>
        /// 移除文档
        /// </summary>
        /// <param name="document">指定要移除的文档</param>
        public void RemoveCompose(Compose document)
        {
            if (document is null)
            {
                return;
            }

            var cache = ComposeCacheDB.FindById(document.Id);


            if (cache is null)
            {
                return;
            }

            cache.IsDeleted      = true;
            cache.TimeOfModified = DateTime.Now;


            Modified();
            ComposeCacheDB.Update(cache);
        }


        /// <summary>
        /// 移除文档
        /// </summary>
        /// <param name="cache">指定要移除的文档</param>
        public void RemoveComposeCache(ComposeCache cache)
        {
            if (cache is null)
            {
                return;
            }

            cache.IsDeleted      = true;
            cache.TimeOfModified = DateTime.Now;
            ComposeCacheDB.Update(cache);
            Modified();
        }

        #endregion

        #region Update

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="document">指定要更新的文档</param>
        /// <param name="cache">指定要更新的文档</param>
        public void UpdateCompose(Compose document, ComposeCache cache)
        {
            if (document is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(document.Id))
            {
                return;
            }

            if (document.Parts is null || document.Metas is null)
            {
                return;
            }

            if (!ComposeDB.HasID(document.Id) ||
                !ComposeCacheDB.HasID(cache.Id))
            {
                return;
            }

            cache.TimeOfModified = DateTime.Now;
            ComposeCacheDB.Update(cache);
            ComposeDB.Update(document);
        }


        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="cache">指定要更新的文档</param>
        public void UpdateCompose(ComposeCache cache)
        {
            if (cache is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(cache.Id))
            {
                return;
            }

            if (!ComposeCacheDB.HasID(cache.Id))
            {
                return;
            }

            ComposeCacheDB.Update(cache);
        }

        #endregion

        #region GetCompose / GetComposes

        /// <summary>
        /// 获得指定的文档
        /// </summary>
        public IEnumerable<ComposeCache> GetComposes()
        {
            return ComposeCacheDB.Find(x => !x.IsDeleted);
        }

        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="type">指定的文档id</param>
        public IEnumerable<ComposeCache> GetComposes(ComposeType type)
        {
            return ComposeCacheDB.Find(x => !x.IsDeleted && x.Type == type);
        }

        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="id">指定的文档id</param>
        public Compose GetCompose(string id)
        {
            return ComposeDB.FindById(id);
        }


        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="cache">指定的文档缓存</param>
        public Compose GetCompose(ComposeCache cache)
        {
            if (cache is null)
            {
                return null;
            }

            return cache.IsDeleted ? null : GetCompose(cache.Id);
        }

        #endregion


        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            var database = session.Database;
            ComposeDB      = database.GetCollection<Compose>(Name_Compose);
            ComposeCacheDB = database.GetCollection<ComposeCache>(Name_Cache_Compose);
        }

        protected override void OnDatabaseClosing()
        {
            ComposeDB      = null;
            ComposeCacheDB = null;
        }


        /// <summary>
        /// 清空所有文档
        /// </summary>
        public void ReduceSize()
        {
            var expression = Query.EQ(nameof(ComposeCache.IsDeleted), true);

            var markAsDeletedComposeCache = ComposeCacheDB.Find(expression)
                                                          .Select(x => x.Id)
                                                          .ToHashSet();
            ComposeCacheDB.DeleteMany(x => x.IsDeleted);
            ComposeDB.DeleteMany(x => markAsDeletedComposeCache.Contains(x.Id));

            Modified();
        }

        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<ComposeCache> ComposeCacheDB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<Compose> ComposeDB { get; private set; }
    }
}