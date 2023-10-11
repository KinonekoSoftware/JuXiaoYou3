using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public abstract class MetadataEditable<TCache, TDocument> : DataPartEditable<TCache, TDocument>
        where TDocument : class, IMetadataPackage
        where TCache : class, IDataCache
    {
        protected MetadataEditable()
        {
            MetadataTrackerByName = new Dictionary<string, Metadata>(StringComparer.OrdinalIgnoreCase);
        }
        
        /*
         * MetadataIndexCache would tracking the metadata in document metadata collection
         * so,
         */
        
        [NullCheck(UniTestLifetime.Constructor)]
        protected readonly Dictionary<string, Metadata> MetadataTrackerByName;
        

        #region Metadata

        

        protected Metadata GetMetadataById(string metadata)
        {
            return MetadataTrackerByName.TryGetValue(metadata, out var meta) ? meta : null;
        }
        
        protected void AddMetadata(Metadata metadata, bool sync = true)
        {
            if(metadata is null || string.IsNullOrEmpty(metadata.Name))
            {
                return;
            }

            if (MetadataTrackerByName.TryGetValue(metadata.Name, out var insideMetadata))
            {
                insideMetadata.Value      = metadata.Value;
                insideMetadata.Parameters = metadata.Parameters;
            }
            else
            {
                insideMetadata = metadata;
                MetadataTrackerByName.Add(metadata.Name, insideMetadata);

                if (sync)
                {
                    //
                    // 更新在文档中的
                    var insideMetadata2 = Document.Metas
                                                  .Find(x => x.Name == metadata.Name);

                    if (insideMetadata2 is null)
                    {
                        Document.Metas.Add(insideMetadata);
                        return;
                    }
                    
                    if (ReferenceEquals(insideMetadata, insideMetadata2))
                    {
                        return;
                    }
                    
                    insideMetadata2.Value     = insideMetadata.Value;
                    insideMetadata.Parameters = metadata.Parameters;
                }
            }
        }
        
        protected void RemoveMetadata(string metadata)
        {
            if (MetadataTrackerByName.TryGetValue(metadata, out var cache))
            {
                //
                // the problem is that when we remove a cache from document
                // the cache position will be changed
                // 
                Document.Metas
                        .Remove(cache);
                MetadataTrackerByName.Remove(metadata);
            }
        }

        protected void RemoveMetadata(Metadata metadata)
        {
            if (metadata is null || string.IsNullOrEmpty(metadata.Name))
            {
                return;
            }
            RemoveMetadata(metadata.Name);
        }

        protected void ClearMetadata()
        {
            MetadataTrackerByName.Clear();
            Document.Metas.Clear();
        }


        #endregion
    }
}