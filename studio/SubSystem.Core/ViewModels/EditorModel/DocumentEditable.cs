using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public abstract class DocumentEditable<TCache, TDocument> : TabViewModel
        where TDocument : class, IData
        where TCache : class, IDataCache
    {

        #region OnStart

        private void PrepareOpeningDocument(Parameter parameter)
        {
            PrepareOpeningDocument(Cache, Document);
            Cache    = (TCache)parameter.Args[0];
            Document = GetDocumentById(Cache.Id);
        }

        /// <summary>
        /// 在打开文档之前的准备
        /// </summary>
        /// <param name="cache">文档索引</param>
        /// <param name="document">文档本体</param>
        protected abstract void PrepareOpeningDocument(TCache cache, TDocument document);
        
        /// <summary>
        /// 打开文档
        /// </summary>
        /// <param name="cache">文档索引</param>
        /// <param name="document">文档本体</param>
        protected abstract void OpeningDocument(TCache cache, TDocument document);
        
        /// <summary>
        /// 完成文档打开
        /// </summary>
        /// <param name="cache">文档索引</param>
        /// <param name="document">文档本体</param>
        protected abstract void LoadDocumentAfter(TCache cache, TDocument document);
        
        protected sealed override void OnStart(Parameter parameter)
        {
            PrepareOpeningDocument(parameter);
            OpeningDocument(Cache, Document);
            OpeningDocumentAfter();
        }

        private void OpeningDocumentAfter()
        {
            //
            // 创建文档
            Document ??= CreateDocument(OnCreateDocument);

            // 加载文档
            LoadDocumentBefore(Cache, Document);
            
            //
            // 完成打开
            LoadDocumentAfter(Cache, Document);
        }

        protected abstract TDocument CreateDocument(Action<TDocument> callback);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document">要检查的文档</param>
        /// <remarks>
        /// <para>创建部件时，仅添加到文档！</para>
        /// </remarks>
        protected abstract void OnCreateDocument(TDocument document);

        protected abstract void LoadDocumentBefore(TCache cache, TDocument document);
        
        #endregion

        protected abstract TDocument GetDocumentById(string id);
        
        [NullCheck(UniTestLifetime.Startup)]
        public DocumentType Type { get; protected set; }
        
        [NullCheck(UniTestLifetime.Startup)]
        public TDocument Document { get; protected set; }
        
        [NullCheck(UniTestLifetime.Startup)]
        public TCache Cache { get; protected set; }
    }
}