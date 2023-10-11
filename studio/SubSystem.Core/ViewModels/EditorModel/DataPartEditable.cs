using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public abstract class DataPartEditable<TCache, TDocument> : KeywordEditable<TCache, TDocument>
        where TDocument : class, IDataPartPackage
        where TCache : class, IDataCache
    {
        
        protected readonly Dictionary<string, DataPart> DataPartTrackerOfId;
        protected readonly Dictionary<Type, DataPart>   DataPartTrackerOfType;

        protected DataPartEditable()
        {
            DataPartTrackerOfId   = new Dictionary<string, DataPart>(StringComparer.OrdinalIgnoreCase);
            DataPartTrackerOfType = new Dictionary<Type, DataPart>();
        }

        protected void HasDataPart<T>(Func<T> expression) where T : DataPart
        {
            if (Document.Parts.FirstOrDefault(x => x is T) is null)
            {
                var part = expression?.Invoke();

                if (part is null)
                {
                    return;
                }
                
                Document.Parts
                        .Add(part);
            }
        }

        protected void AddDataPartToDocument<T>(T part) where T : DataPart
        {
            Document.Parts.Add(part);
        }

        protected void AddDataPart(DataPart dataPart)
        {
            if (DataPartTrackerOfId.TryAdd(dataPart.Id, dataPart))
            {
                DataPartTrackerOfType.TryAdd(dataPart.GetType(), dataPart);
            }
        }
        protected void AddDataPartFromDocument(TDocument document)
        {
            var logger     = Xaml.Get<ILogger>();
            var hasChanged = false;
            var indexList  = new List<DataPart>();
            
            for (var i = 0; i < document.Parts.Count; i ++)
            {
                var part = document.Parts[i];

                if (part is PartOfModule &&
                    !string.IsNullOrEmpty(part.Id) && 
                    DataPartTrackerOfId.ContainsKey(part.Id))
                {
                    indexList.Add(part);
                    logger.Warn($"部件没有ID或者部件重复不予添加，部件ID：{part.Id}");
                    hasChanged = true;
                    continue;
                }
                
                if(DataPartTrackerOfType.ContainsKey(part.GetType()))
                {
                    indexList.Add(part);
                    logger.Warn($"部件没有ID或者部件重复不予添加，部件ID：{part.Id}");
                    hasChanged = true;
                    continue;
                }

                if (OnDataPartAddingBefore(part))
                {
                    continue;
                }
                
                OnDataPartAddingAfter(part);
            }

            if (hasChanged)
            {
                indexList.ForEach(x => document.Parts.Remove(x));
                SetDirtyState();
            }
        }

        protected void ClearDataPart()
        {
            DataPartTrackerOfId.Clear();
            DataPartTrackerOfId.Clear();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override void LoadDocumentBefore(TCache cache, TDocument document)
        {
            AddDataPartFromDocument(document);
            IsDataPartExistence(document);

            if (DataPartTrackerOfType.Count <= 0)
            {
                AddDataPartFromDocument(document);
            }
        }

        protected abstract bool OnDataPartAddingBefore(DataPart part);
        protected abstract void OnDataPartAddingAfter(DataPart part);
        
        /// <summary>
        /// 判断部件是否存在，如果不存在，使用HasDataPart，添加到文档的同时也添加到容器当中
        /// </summary>
        /// <param name="document">要检查的文档</param>
        protected abstract void IsDataPartExistence(TDocument document);
    }
}