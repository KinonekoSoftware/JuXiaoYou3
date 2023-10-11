using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Relatives;
using Acorisoft.FutureGL.MigaDB.Utils;
using static Acorisoft.FutureGL.MigaDB.Constants;
// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    [ConceptProvider]
    public class DocumentEngine : KnowledgeEngine
    {
        protected override void OnDatabaseOpeningOverride(DatabaseSession session)
        {
            var database = session.Database;
            RelDB     = database.GetCollection<CharacterRelationship>(Name_Relationship_Character);
            DocumentDB      = database.GetCollection<Document>(Name_Document);
            DocumentCacheDB = database.GetCollection<DocumentCache>(Name_Cache_Document);
        }

        protected override void OnDatabaseClosingOverride()
        {
            DocumentDB      = null;
            DocumentCacheDB = null;
            RelDB     = null;
        }


        public override Knowledge GetKnowledge(string id)
        {
            var cache = DocumentCacheDB.FindById(id);

            if (cache is null)
            {
                return null;
            }

            return new Knowledge
            {
                Id     = cache.Id,
                Name   = cache.Name,
                Intro  = cache.Intro,
                Avatar = cache.Avatar
            };
        }

        #region Add

        
        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="document">指定要添加的文档</param>
        /// <returns>返回操作结果</returns>
        public EngineResult AddDocumentCache(DocumentCache document)
        {
            if (document is null)
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }

            if (string.IsNullOrEmpty(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.MissingId);
            }

            if (HasDocumentID(document.Id) ||
                HasDocumentCacheID(document.Id)||
                HasDocumentName(document.Id, document.Name))
            {
                return EngineResult.Failed(EngineFailedReason.Duplicated);
            }


            AddConcept(document.Id, document.Name, KnowledgeHandler.DocumentEngine);
            DocumentCacheDB.Insert(document);

            //
            // 一致性检查
            //if (!HasDocumentID(document.Id) ||
            //    !HasDocumentCacheID(document.Id))
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
        public EngineResult AddDocument(Document document)
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

            if (!HasDocumentCacheID(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.ConsistencyBroken);
            }

            DocumentDB.Insert(document);

            //
            // 一致性检查
            if (!HasDocumentID(document.Id) ||
                !HasDocumentCacheID(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.ConsistencyBroken);
            }

            if (HasDocumentName(document.Id, document.Name))
            {
                return EngineResult.Failed(EngineFailedReason.Duplicated);
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
        public void RemoveDocument(Document document)
        {
            if (document is null)
            {
                return;
            }

            var cache = DocumentCacheDB.FindById(document.Id);


            if (cache is null)
            {
                return;
            }

            cache.IsDeleted      = true;
            cache.TimeOfModified = DateTime.Now;


            Modified();
            RemoveConcept(document.Id);
            DocumentCacheDB.Update(cache);
        }
        
        
        /// <summary>
        /// 移除文档
        /// </summary>
        /// <param name="cache">指定要移除的文档</param>
        public void RemoveDocumentCache(DocumentCache cache)
        {
            if (cache is null)
            {
                return;
            }

            cache.IsDeleted      = true;
            cache.TimeOfModified = DateTime.Now;
            DocumentCacheDB.Update(cache);
            RemoveConcept(cache.Id);
            Modified();
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        /// <param name="cache">指定要移除的文档</param>
        public void HardRemoveDocumentCache(DocumentCache cache)
        {
            if (cache is null)
            {
                return;
            }
            DocumentCacheDB.Delete(cache.Id);
            RemoveConcept(cache.Id);
            Modified();
        }

        #endregion

        #region Update

        
        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="document">指定要更新的文档</param>
        /// <param name="cache">指定要更新的文档</param>
        public void UpdateDocument(Document document, DocumentCache cache)
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

            if (!HasDocumentID(document.Id) ||
                !HasDocumentCacheID(cache.Id) ||
                HasDocumentName(document.Id, document.Name))
            {
                return;
            }

            AddConcept(document.Id, document.Name, KnowledgeHandler.DocumentEngine);
            DocumentCacheDB.Update(cache);
            DocumentDB.Update(document);
        }

        
        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="cache">指定要更新的文档</param>
        public void UpdateDocument(DocumentCache cache)
        {
            if (cache is null)
            {
                return;
            }
        
            if (string.IsNullOrEmpty(cache.Id))
            {
                return;
            }
        
            if (!HasDocumentCacheID(cache.Id) ||
                HasDocumentName(cache.Id, cache.Name))
            {
                return;
            }
        
            AddConcept(cache.Id, cache.Name, KnowledgeHandler.DocumentEngine);
            DocumentCacheDB.Update(cache);
        }
        
        
        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="document">指定要更新的文档</param>
        internal void UpdateDocument(Document document)
        {
            if (document is null)
            {
                return;
            }
        
            if (string.IsNullOrEmpty(document.Id))
            {
                return;
            }
        
            if (!HasDocumentID(document.Id) ||
                HasDocumentName(document.Id, document.Name))
            {
                return;
            }
        
            DocumentDB.Update(document);
        }

        #endregion

        #region GetDocument / GetDocuments

        
        /// <summary>
        /// 获得指定的文档
        /// </summary>
        public IEnumerable<DocumentCache> GetDocuments()
        {
            return DocumentCacheDB.Find(x => !x.IsDeleted);
        }
        
        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="type">指定的文档id</param>
        public IEnumerable<DocumentCache> GetDocuments(DocumentType type)
        {
            return DocumentCacheDB.Find(x => !x.IsDeleted && x.Type == type);
        }
        
        
        public IEnumerable<DocumentCache> GetRemovedDocuments()
        {
            return DocumentCacheDB.Find(x => x.IsDeleted );
        }
        
        public IEnumerable<DocumentCache> GetDocuments(DocumentType type, HashSet<string> hash)
        {
            return DocumentCacheDB.Find(x => !x.IsDeleted && x.Type == type && hash.Contains(x.Id));
        }

        public IEnumerable<DocumentCache> GetDocuments(HashSet<string> hash)
        {
            return DocumentCacheDB.Find(x => !x.IsDeleted  && hash.Contains(x.Id));
        }
        
        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="type">指定的文档id</param>
        public IEnumerable<DocumentCache> GetRecycledDocuments(DocumentType type)
        {
            return DocumentCacheDB.Find(x => x.IsDeleted && x.Type == type);
        }

        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="id">指定的文档id</param>
        public Document GetDocument(string id)
        {
            return DocumentDB.FindById(id);
        }
        

        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="cache">指定的文档缓存</param>
        public Document GetDocument(DocumentCache cache)
        {
            if (cache is null)
            {
                return null;
            }

            return cache.IsDeleted ? null : GetDocument(cache.Id);
        }

        #endregion

        #region GetRelatives

        public IEnumerable<CharacterRelationship> GetRelatives(DocumentType type)
        {
            return RelDB.Include(x => x.Source)
                        .Include(x => x.Target)
                        .Find(x => x.Type == type &&
                                   !x.Source.IsDeleted &&
                                   !x.Target.IsDeleted);
        }
        
        public IEnumerable<CharacterRelationship> GetRelatives(DocumentType type, string id)
        {
            return RelDB.Include(x => x.Source)
                        .Include(x => x.Target)
                        .Find(x => x.Type == type &&
                                   (x.Source.Id == id || x.Target.Id == id) &&
                                   !x.Source.IsDeleted &&
                                   !x.Target.IsDeleted);
        }

        #endregion

        #region GetCaches
        
        public IEnumerable<DocumentCache> GetCaches(DocumentType type)
        {
            return DocumentCacheDB.Find(x => x.Type == type);
        }
        
        public IEnumerable<DocumentCache> GetCaches(DocumentType type, ISet<string> idPool)
        {
            return DocumentCacheDB.Find(x => x.Type == type && idPool.Contains(x.Id));
        }
        
        public IEnumerable<DocumentCache> GetCachesExclude(DocumentType type, ISet<string> idPool)
        {
            return DocumentCacheDB.Find(x => x.Type == type && !idPool.Contains(x.Id));
        }
        
        public IEnumerable<DocumentCache> GetCachesExclude(DocumentType type, string target)
        {
            return DocumentCacheDB.Find(x => x.Type == type && target != x.Id);
        }


        public IEnumerable<DocumentCache> GetCaches(HashSet<string> idPool)
        {
            return DocumentCacheDB.Find(x => idPool.Contains(x.Id));
        }

        public IEnumerable<DocumentCache> GetCaches()
        {
            return DocumentCacheDB.Find(x => !x.IsDeleted);
        }

        public IEnumerable<DocumentCache> GetCharacterCaches()
        {
            return DocumentCacheDB.Find(x => x.Type == DocumentType.Character && !x.IsDeleted);
        }
        
        public IEnumerable<DocumentCache> GetNPCCaches()
        {
            return DocumentCacheDB.Find(x => x.Type == DocumentType.NPC);
        }

        public IEnumerable<DocumentCache> GetCharacterWitNPCCaches()
        {
            return DocumentCacheDB.Find(x => x.Type == DocumentType.Character || x.Type == DocumentType.NPC);
        }

        public DocumentCache GetCache(string id)
        {
            return DocumentCacheDB.FindById(id);
        }

        public int GetCacheCount() => DocumentCacheDB.Count();
        
        #endregion

        #region Has

        public bool HasDocumentName(string id, string name) => DocumentCacheDB.Exists(x => !x.IsDeleted &&
                                                                                           x.Name == name &&
                                                                                           x.Id != id);

        public bool HasDocumentID(string id)
        {
            return DocumentDB.Exists(Query.EQ(litedb.ID, id));
        }
        
        
        public bool HasDocumentCacheID(string id)
        {
            return DocumentCacheDB.Exists(Query.EQ(litedb.ID, id));
        }

        #endregion

        #region Relationship : Add / Remove

        
        public void AddRelationship(CharacterRelationship rel)
        {
            // ReSharper disable once MergeSequentialChecks
            if (rel is null ||
                rel.Source is null ||
                rel.Target is null)
            {
                return;
            }

            RelDB.Insert(rel);
        }

        public void EditRelationship(CharacterRelationship rel)
        {
            // ReSharper disable once MergeSequentialChecks
            if (rel is null ||
                rel.Source is null ||
                rel.Target is null)
            {
                return;
            }

            RelDB.Update(rel);
        }
        

        public void RemoveRelationship(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }
            
            RelDB.Delete(id);
        }

        #endregion


        /// <summary>
        /// 清空所有文档
        /// </summary>
        public void ReduceSize()
        {
            var expression = Query.EQ(nameof(DocumentCache.IsDeleted), true);
            var markAsDeletedDocumentCache = DocumentCacheDB.Find(expression)
                                                            .Select(x => x.Id)
                                                            .ToHashSet();
            DocumentCacheDB.DeleteMany(x => x.IsDeleted);
            DocumentDB.DeleteMany(x => markAsDeletedDocumentCache.Contains(x.Id));

            if(markAsDeletedDocumentCache.Count > 0)
                Modified();
        }
        
        internal bool DataConsistencyCheck { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<CharacterRelationship> RelDB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<DocumentCache> DocumentCacheDB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<Document> DocumentDB { get; private set; }
    }
}